// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x6E694720, 32)]
    public class STUAvailableContentUnlock : STUInstance
    {
        [STUField(0x45216F79, 0)] // size: 16
        public teString[] m_45216F79;

        [STUField(0xDB803F2F, 16)] // size: 16
        public teStructuredDataAssetRef<STUUnlock>[] m_unlocks;
    }
}
