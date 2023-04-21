// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x0699809D, 32)]
    public class STUXPGain : STUInstance
    {
        [STUField(0xF1CB3BA0, 0)] // size: 16
        public teStructuredDataAssetRef<ulong> m_text;

        [STUField(0xAF872E86, 16)] // size: 8
        public double m_amount;

        [STUField(0x0619C597, 24)] // size: 4
        public STUXPGainType m_type;
    }
}
