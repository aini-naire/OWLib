using System.Collections.Generic;
using System.IO;
using DataTool.Helper;
using TankLib;
using TankLib.ExportFormats;
using static DataTool.Helper.EffectParser;

namespace DataTool.SaveLogic {
    public static class Effect {
        public class OverwatchEffect : IExportFormat {
            public string Extension => "oweffect";

            public static void WriteTime(BinaryWriter writer, EffectParser.ChunkPlaybackInfo playbackInfo) {
                writer.Write(playbackInfo.TimeInfo == null);
                if (playbackInfo.TimeInfo != null) {
                    writer.Write(playbackInfo.TimeInfo.StartTime);
                    writer.Write(playbackInfo.TimeInfo.EndTime);
                } else {
                    writer.Write(0f);
                    writer.Write(0f);
                }

                if (playbackInfo.Hardpoint != 0) {
                    writer.Write(OverwatchModel.IdToString("hardpoint", teResourceGUID.Index(playbackInfo.Hardpoint)));
                } else {
                    writer.Write("null");
                }
            }

            public const ushort EffectVersionMajor = 2;
            public const ushort EffectVersionMinor = 0;

            protected readonly FindLogic.Combo.ComboInfo Info;
            protected readonly FindLogic.Combo.EffectInfoCombo EffectInfo;
            protected readonly Dictionary<ulong, HashSet<FindLogic.Combo.VoiceLineInstanceInfo>> VoiceStimuli;

            public OverwatchEffect(
                FindLogic.Combo.ComboInfo info, FindLogic.Combo.EffectInfoCombo effectInfo,
                Dictionary<ulong, HashSet<FindLogic.Combo.VoiceLineInstanceInfo>> voiceStimuli) {
                Info = info;
                EffectInfo = effectInfo;
                VoiceStimuli = voiceStimuli;
            }

            public void Write(Stream stream) {
                using (BinaryWriter writer = new BinaryWriter(stream)) {
                    WriteEffect(writer);
                }
            }

            protected void WriteEffect(BinaryWriter writer) {
                writer.Write("oweffect");
                writer.Write(EffectVersionMajor);
                writer.Write(EffectVersionMinor);

                EffectParser.EffectInfo effect = EffectInfo.Effect;

                writer.Write(teResourceGUID.Index(effect.GUID));
                writer.Write(effect.EffectLength);

                writer.Write(effect.DMCEs.Count);
                writer.Write(effect.CECEs.Count);
                writer.Write(effect.NECEs.Count);
                writer.Write(effect.RPCEs.Count);
                writer.Write(effect.FECEs.Count);
                writer.Write(effect.OSCEs.Count);
                writer.Write(effect.SVCEs.Count);

                foreach (EffectParser.DMCEInfo dmceInfo in effect.DMCEs) {
                    WriteTime(writer, dmceInfo.PlaybackInfo);
                    writer.Write(dmceInfo.Animation);
                    writer.Write(dmceInfo.Material);
                    writer.Write(dmceInfo.Model);
                    FindLogic.Combo.ModelAsset modelInfo = Info.m_models[dmceInfo.Model];
                    writer.Write(Path.Combine("..", "..", "Models", modelInfo.GetName(), modelInfo.GetNameIndex() + ".owmdl"));
                    writer.Write(Path.Combine("..", "..", "Models", modelInfo.GetName(), "Materials", teResourceGUID.AsIndexString(dmceInfo.Material) + ".owmat"));
                    if (dmceInfo.Animation == 0) {
                        writer.Write("null");
                    } else {
                        FindLogic.Combo.AnimationAsset animationInfo = Info.m_animations[dmceInfo.Animation];
                        writer.Write(Path.Combine("..", "..", "Models", modelInfo.GetName(), OverwatchAnimationEffect.AnimationEffectDir, animationInfo.GetNameIndex(), animationInfo.GetNameIndex() + ".owanim"));
                    }
                }

                foreach (EffectParser.CECEInfo ceceInfo in effect.CECEs) {
                    WriteTime(writer, ceceInfo.PlaybackInfo);
                    writer.Write((byte) ceceInfo.Action);
                    writer.Write(ceceInfo.Animation);
                    writer.Write(ceceInfo.Identifier);
                    writer.Write(teResourceGUID.Index(ceceInfo.Identifier));
                    if (ceceInfo.Animation != 0) {
                        FindLogic.Combo.AnimationAsset animationInfo = Info.m_animations[ceceInfo.Animation];
                        writer.Write(Path.Combine(OverwatchAnimationEffect.AnimationEffectDir, animationInfo.GetNameIndex(), animationInfo.GetNameIndex() + ".owanim"));
                    } else {
                        writer.Write("null");
                    }
                }

                foreach (EffectParser.NECEInfo neceInfo in effect.NECEs) {
                    WriteTime(writer, neceInfo.PlaybackInfo);
                    writer.Write(teResourceGUID.AsIndexString(neceInfo.GUID));
                    writer.Write(neceInfo.Identifier);
                    FindLogic.Combo.EntityAsset entityInfo = Info.m_entities[neceInfo.GUID];
                    writer.Write(Path.Combine("..", "..", "Entities", entityInfo.GetName(), entityInfo.GetNameIndex() + ".owentity"));
                }

                foreach (EffectParser.RPCEInfo rpceInfo in effect.RPCEs) {
                    WriteTime(writer, rpceInfo.PlaybackInfo);
                    writer.Write(rpceInfo.Model);
                    // todo: make the materials work
                    //writer.Write(teResourceGUID.AsIndexString(rpceInfo.Material));
                    FindLogic.Combo.ModelAsset modelInfo = Info.m_models[rpceInfo.Model];
                    //writer.Write(rpceInfo.TextureDefiniton);
                    writer.Write(Path.Combine("..", "..", "Models", modelInfo.GetName(), "Materials", teResourceGUID.AsIndexString(rpceInfo.Material) + ".owmat"));

                    writer.Write(Path.Combine("..", "..", "Models", modelInfo.GetName(), modelInfo.GetNameIndex() + ".owmdl"));
                }

                foreach (EffectParser.SVCEInfo svceInfo in effect.SVCEs) {
                    WriteTime(writer, svceInfo.PlaybackInfo);
                    writer.Write(teResourceGUID.Index(svceInfo.VoiceStimulus));
                    if (VoiceStimuli.ContainsKey(svceInfo.VoiceStimulus)) {
                        HashSet<FindLogic.Combo.VoiceLineInstanceInfo> lines = VoiceStimuli[svceInfo.VoiceStimulus];
                        writer.Write(lines.Count);

                        foreach (FindLogic.Combo.VoiceLineInstanceInfo voiceLineInstance in lines) {
                            writer.Write(voiceLineInstance.SoundFiles.Count);
                            foreach (ulong soundFile in voiceLineInstance.SoundFiles) {
                                FindLogic.Combo.SoundFileAsset soundFileInfo =
                                    Info.m_voiceSoundFiles[soundFile];

                                writer.Write(Path.Combine("Sounds", soundFileInfo.GetNameIndex() + ".ogg"));
                            }
                        }
                    } else {
                        writer.Write(0);
                    }
                }

                foreach (EffectParser.FECEInfo feceInfo in effect.FECEs) {
                    WriteTime(writer, feceInfo.PlaybackInfo);
                    writer.Write(feceInfo.GUID);
                    var isAnimEffect = false;
                    if (!Info.m_effects.TryGetValue(feceInfo.GUID, out var effectInfo)) {
                        isAnimEffect = true;
                        if (!Info.m_animationEffects.TryGetValue(feceInfo.GUID, out effectInfo)) {
                            writer.Write((byte) 0);
                            continue;
                        }
                    }
                    writer.Write(Path.Combine(isAnimEffect ? OverwatchAnimationEffect.AnimationEffectDir : "AnimationEffects", effectInfo.GetName(), effectInfo.GetNameIndex() + ".owanim"));
                }

                foreach (EffectParser.OSCEInfo osceInfo in effect.OSCEs) {
                    WriteTime(writer, osceInfo.PlaybackInfo);
                    writer.Write(teResourceGUID.Index(osceInfo.Sound));
                    if (Info.m_sounds.TryGetValue(osceInfo.Sound, out var soundInfo) && soundInfo.SoundFiles != null) {
                        writer.Write(soundInfo.SoundFiles.Count);
                        foreach (var soundFile in soundInfo.SoundFiles.Values) {
                            writer.Write(Path.Combine("Sounds", Info.m_soundFiles[soundFile].GetName() + ".ogg"));
                        }
                    } else {
                        writer.Write(0);
                    }
                }
            }
        }

        public class OverwatchAnimationEffect : IExportFormat {
            public string Extension => "owanim";

            public const string AnimationEffectDir = "AnimationEffects";

            protected readonly FindLogic.Combo.ComboInfo Info;
            protected readonly FindLogic.Combo.AnimationAsset Animation;
            protected readonly FindLogic.Combo.EffectInfoCombo EffectInfo;
            protected readonly ulong Model;

            public const ushort AnimVersionMajor = 2;
            public const ushort AnimVersionMinor = 0;

            public OverwatchAnimationEffect(
                FindLogic.Combo.ComboInfo info,
                FindLogic.Combo.EffectInfoCombo animationEffect,
                FindLogic.Combo.AnimationAsset animation,
                ulong model) {
                Info = info;
                Animation = animation;
                EffectInfo = animationEffect;
                Model = model;
            }

            public enum OWAnimType {
                Unknown = -1,
                Data = 0,
                Reference = 1,
                Reset = 2
            }

            public void Write(Stream stream) {
                using (BinaryWriter writer = new BinaryWriter(stream)) {
                    writer.Write(Extension);
                    writer.Write(AnimVersionMajor);
                    writer.Write(AnimVersionMinor);
                    writer.Write(teResourceGUID.Index(Animation.m_GUID));
                    writer.Write(Animation.m_fps);
                    writer.Write((int) OWAnimType.Data);

                    FindLogic.Combo.ModelAsset modelInfo = Info.m_models[Model];


                    writer.Write(Path.Combine("Models", modelInfo.GetName(), "Animations", Animation.m_priority.ToString(), Animation.GetNameIndex() + ".seanim"));
                    writer.Write(Path.Combine("Models", modelInfo.GetName(), modelInfo.GetNameIndex() + ".owmdl"));
                    writer.Write(Path.Combine(AnimationEffectDir, Animation.GetNameIndex(), EffectInfo.GetNameIndex() + ".oweffect"));
                }
            }
        }

        public class OverwatchAnimationEffectReference : IExportFormat {
            public string Extension => "owanim";

            protected readonly FindLogic.Combo.ComboInfo Info;
            protected readonly FindLogic.Combo.AnimationAsset Animation;
            protected readonly ulong Model;

            public OverwatchAnimationEffectReference(FindLogic.Combo.ComboInfo info, FindLogic.Combo.AnimationAsset animation, ulong model) {
                Info = info;
                Animation = animation;
                Model = model;
            }

            public void Write(Stream stream) {
                using (BinaryWriter writer = new BinaryWriter(stream)) {
                    writer.Write(Extension); // identifier
                    writer.Write(OverwatchAnimationEffect.AnimVersionMajor);
                    writer.Write(OverwatchAnimationEffect.AnimVersionMinor);
                    writer.Write(teResourceGUID.Index(Animation.m_GUID));
                    writer.Write(Animation.m_fps);
                    writer.Write((int) OverwatchAnimationEffect.OWAnimType.Reference);

                    FindLogic.Combo.ModelAsset modelInfo = Info.m_models[Model];

                    writer.Write(Path.Combine("Models", modelInfo.GetName(), OverwatchAnimationEffect.AnimationEffectDir, Animation.GetNameIndex(), Animation.GetNameIndex() + $".{Extension}")); // so I can change it in DataTool and not go mad
                }
            }
        }
    }
}
