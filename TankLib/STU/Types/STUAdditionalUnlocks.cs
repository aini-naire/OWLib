// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x5C499B8E, 24)]
    public class STUAdditionalUnlocks : STUInstance
    {
        [STUField(0xDB803F2F, 0)] // size: 16
        public teStructuredDataAssetRef<STUUnlock>[] m_unlocks;

        [STUField(0x2C01908B, 16)] // size: 4
        public uint m_level;
    }
}
