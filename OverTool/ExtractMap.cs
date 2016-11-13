﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CASCExplorer;
using OWLib;
using OWLib.Types;
using OWLib.Types.STUD;
using OWLib.ModelWriter;
using OverTool.ExtractLogic;
using OWLib.Types.Map;
using OWLib.Types.STUD.Binding;

namespace OverTool {
  class ExtractMap {
    public static void Parse(Dictionary<ushort, List<ulong>> track, Dictionary<ulong, Record> map, CASCHandler handler, string[] args) {
      if(args.Length < 1) {
        Console.Out.WriteLine("Usage: OverTool.exe overwatch M output [maps] .[C]");
        Console.Out.WriteLine("The last value is to export collision models. The last argument must be exactly \".C\" if you want to export collision models");
        return;
      }

      string output = args[0];
      List<string> maps = args.Skip(1).ToList();
      
      bool skipCmodel = true;
      if(maps.Count > 0) {
        if(maps.Last()[0] == '.') {
          string cmd = maps.Last();
          maps.Remove(cmd);
          if(cmd.Length > 1 && cmd[1] == 'C') {
            skipCmodel = false;
          }
        }
      }

      for(int i = 0; i < maps.Count; ++i) {
        maps[i] = maps[i].ToLowerInvariant();
      }
      bool mapWildcard = maps.Count == 0;
      if(maps.Count > 0 && maps.Contains("*")) {
        mapWildcard = true;
      }

      List<ulong> masters = track[0x9F];
      List<byte> LODs = new List<byte>(new byte[5] { 0, 1, 128, 254, 255 });
      Dictionary<ulong, ulong> replace = new Dictionary<ulong, ulong>();
      foreach(ulong masterKey in masters) {
        if(!map.ContainsKey(masterKey)) {
          continue;
        }
        STUD masterStud = new STUD(Util.OpenFile(map[masterKey], handler));
        if(masterStud.Instances == null) {
          continue;
        }
        MapMaster master = (MapMaster)masterStud.Instances[0];
        if(master == null) {
          continue;
        }
        
        string name = Util.GetString(master.Header.name.key, map, handler);
        if(name == null) {
          continue;
        }
        if(!mapWildcard && !maps.Contains(name.ToLowerInvariant())) {
          continue;
        }

        string outputPath = string.Format("{0}{1}{2}{1}{3:X}{1}", output, Path.DirectorySeparatorChar, Util.SanitizePath(name), APM.keyToIndex(master.Header.data.key));

        if(!map.ContainsKey(master.Header.data.key)) {
          continue;
        }
        
        HashSet<ulong> parsed = new HashSet<ulong>();
        Dictionary<ulong, ulong> animList = new Dictionary<ulong, ulong>();
        using(Stream mapStream = Util.OpenFile(map[master.Header.data.key], handler)) {
          Console.Out.WriteLine("Extracting map {0} with ID {1:X8}", name, APM.keyToIndex(master.Header.data.key));
          Map mapData = new Map(mapStream);
          OWMAPWriter owmap = new OWMAPWriter();
          Dictionary<ulong, List<string>>[] used = null;
          if(!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
          }
          List<ulong> soundData = new List<ulong>();
          HashSet<ulong> soundDone = new HashSet<ulong>();
          VoiceLine.FindSoundsEx(master.Header.audio.key, soundDone, soundData, map, handler, replace);
          using(Stream map2Stream = Util.OpenFile(map[master.DataKey(2)], handler)) {
            Map map2Data = new Map(map2Stream);
            using(Stream map8Stream = Util.OpenFile(map[master.DataKey(8)], handler)) {
              Map map8Data = new Map(map8Stream);
              using(Stream mapBStream = Util.OpenFile(map[master.DataKey(0xB)], handler)) {
                Map mapBData = new Map(mapBStream, true);

                mapBStream.Position = (long)(Math.Ceiling((float)mapBStream.Position / 16.0f) * 16); // Future proofing

                List<STUD> inlineSTUDArray = new List<STUD>();
                try {
                  while(true) { // TODO: Move this to Map.cs
                    if(inlineSTUDArray.Count > 0 && inlineSTUDArray.Last().end >= mapBStream.Length) {
                      break;
                    }
                    STUD tmp = new STUD(mapBStream, true, STUDManager.Instance, true, false);
                    mapBStream.Position = tmp.end;
                    inlineSTUDArray.Add(tmp);
                  }
                } catch { break; }

                for(int i = 0; i < inlineSTUDArray.Count; ++i) {
                  STUD stud = inlineSTUDArray[i];
                  VoiceLine.FindSoundsSTUD(stud, soundDone, soundData, map, handler, replace);
                }

                for(int i = 0; i < mapBData.Records.Length; ++i) {
                  if(mapBData.Records[i] != null && mapBData.Records[i].GetType() != typeof(Map0B)) {
                    continue;
                  }
                  Map0B mapprop = (Map0B)mapBData.Records[i];
                  if(!map.ContainsKey(mapprop.Header.binding)) {
                    continue;
                  }
                  VoiceLine.FindSoundsEx(mapprop.Header.binding, soundDone, soundData, map, handler, replace);
                  using(Stream bindingFile = Util.OpenFile(map[mapprop.Header.binding], handler)) {
                    STUD binding = new STUD(bindingFile, true, STUDManager.Instance, false, true);
                    foreach(ISTUDInstance instance in binding.Instances) {
                      if(instance == null) {
                        continue;
                      }
                      if(instance.Name != binding.Manager.GetName(typeof(ComplexModelRecord))) {
                        continue;
                      }
                      ComplexModelRecord cmr = (ComplexModelRecord)instance;
                      mapprop.MaterialKey = cmr.Data.material.key;
                      mapprop.ModelKey = cmr.Data.model.key;
                      Skin.FindAnimations(cmr.Data.animationList.key, animList, replace, parsed, map, handler, null, null, mapprop.ModelKey);
                      Skin.FindAnimations(cmr.Data.secondaryAnimationList.key, animList, replace, parsed, map, handler, null, null, mapprop.ModelKey);
                      break;
                    }
                  }
                  mapBData.Records[i] = mapprop;
                }

                using (Stream mapLStream = Util.OpenFile(map[master.DataKey(9)], handler)) {
                    Map mapLData = new Map(mapLStream);
                    using (Stream outputStream = File.Open(string.Format("{0}{1}{2}", outputPath, Util.SanitizePath(name), owmap.Format), FileMode.Create, FileAccess.Write)) {
                        used = owmap.Write(outputStream, mapData, map2Data, map8Data, mapBData, mapLData, name);
                    }
                }
              }
            }
          }
          IModelWriter owmdl = new OWMDLWriter();
          IModelWriter owmat = new OWMATWriter();
          using(Stream map10Stream = Util.OpenFile(map[master.DataKey(0x10)], handler)) {
            Map10 physics = new Map10(map10Stream);
            using(Stream outputStream = File.Open(string.Format("{0}physics{1}", outputPath, owmdl.Format), FileMode.Create, FileAccess.Write)) {
              owmdl.Write(physics, outputStream, new object[0]);
            }
          }
          if(used != null) {
            Dictionary<ulong, List<string>> models = used[0];
            Dictionary<ulong, List<string>> materials = used[1];
            Dictionary<ulong, Dictionary<ulong, List<ImageLayer>>> cache = new Dictionary<ulong, Dictionary<ulong, List<ImageLayer>>>();

            foreach(KeyValuePair<ulong, List<string>> modelpair in models) {
              if(!map.ContainsKey(modelpair.Key)) {
                continue;
              }
              if(!parsed.Add(modelpair.Key)) {
                continue;
              }
              using(Stream modelStream = Util.OpenFile(map[modelpair.Key], handler)) {
                Chunked mdl = new Chunked(modelStream);
                foreach(string modelOutput in modelpair.Value) {
                  using(Stream outputStream = File.Open(string.Format("{0}{1}", outputPath, modelOutput), FileMode.Create, FileAccess.Write)) {
                    if(owmdl.Write(mdl, outputStream, LODs, new Dictionary<ulong, List<ImageLayer>>(), new object[5] { null, null, null, null, skipCmodel })) {
                      Console.Out.WriteLine("Wrote model {0}", modelOutput);
                    } else {
                      Console.Out.WriteLine("Failed to write model");
                    }
                  }
                }
              }
            }
            foreach(KeyValuePair<ulong, ulong> kv in animList) {
              ulong parent = kv.Value;
              ulong key = kv.Key;
              string outpath = string.Format("{0}Animations{1}{2:X12}{1}{3:X12}.{4:X3}", outputPath, Path.DirectorySeparatorChar, APM.keyToIndex(parent), APM.keyToIndexID(key), APM.keyToTypeID(key));
              if(!Directory.Exists(Path.GetDirectoryName(outpath))) {
                Directory.CreateDirectory(Path.GetDirectoryName(outpath));
              }
              using(Stream outp = File.Open(outpath, FileMode.Create, FileAccess.Write)) {
                Util.OpenFile(map[key], handler).CopyTo(outp);
                Console.Out.WriteLine("Wrote animation {0}", outpath);
              }
            }
            
            Console.Out.WriteLine("Dumping sounds...");
            string soundPath = string.Format("{0}Sounds{1}", outputPath, Path.DirectorySeparatorChar);
            if(!Directory.Exists(soundPath)) {
              Directory.CreateDirectory(soundPath);
            }

            DumpVoice.Save(soundPath, soundData, map, handler);

            foreach(KeyValuePair<ulong, List<string>> matpair in materials) {
              Dictionary<ulong, List<ImageLayer>> tmp = new Dictionary<ulong, List<ImageLayer>>();
              if(cache.ContainsKey(matpair.Key)) {
                tmp = cache[matpair.Key];
              } else {
                Skin.FindTextures(matpair.Key, tmp, new Dictionary<ulong, ulong>(), new HashSet<ulong>(), map, handler);
                cache.Add(matpair.Key, tmp);
              }
              foreach(KeyValuePair<ulong, List<ImageLayer>> kv in tmp) {
                ulong materialId = kv.Key;
                List<ImageLayer> sublayers = kv.Value;
                foreach(ImageLayer layer in sublayers) {
                  if(!parsed.Add(layer.key)) {
                    continue;
                  }
                  Skin.SaveTexture(layer.key, map, handler, string.Format("{0}{1:X12}.dds", outputPath, APM.keyToIndexID(layer.key)));
                }
              }

              foreach(string matOutput in matpair.Value) {
                using(Stream outputStream = File.Open(string.Format("{0}{1}", outputPath, matOutput), FileMode.Create, FileAccess.Write)) {
                  if(owmat.Write(null, outputStream, null, tmp, new object[0])) {
                    Console.Out.WriteLine("Wrote material {0}", matOutput);
                  } else {
                    Console.Out.WriteLine("Failed to write material");
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
