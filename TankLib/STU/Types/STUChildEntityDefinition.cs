// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x1D9159F2, 64)]
    public class STUChildEntityDefinition : STUInstance
    {
        [STUField(0x436BEFE6, 8)] // size: 16
        public teStructuredDataAssetRef<STUEntityDefinition> m_child;

        [STUField(0x2D3CDFA9, 24)] // size: 16
        public teStructuredDataAssetRef<STUHardPoint> m_hardPoint;

        [STUField(0x49F782CE, 40)] // size: 16
        public teStructuredDataAssetRef<STUIdentifier> m_49F782CE;

        [STUField(0x2263EA37, 56)] // size: 1
        public byte m_2263EA37;
    }
}
