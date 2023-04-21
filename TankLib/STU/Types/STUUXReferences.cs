// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x10764043, 32)]
    public class STUUXReferences : STUInstance
    {
        [STUField(0x213692AE, 0)] // size: 16
        public teStructuredDataAssetRef<ulong>[] m_resourceDictionaries;

        [STUField(0xB1B74816, 16, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 16
        public STUUXResource[] m_resources;
    }
}
