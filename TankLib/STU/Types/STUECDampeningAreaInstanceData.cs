// Generated by TankLibHelper
using TankLib.Math;
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x9AB01A35, 48)]
    public class STUECDampeningAreaInstanceData : STUComponentInstanceData
    {
        [STUField(0x2746D7E4, 8)] // size: 16
        public teUUID m_2746D7E4;

        [STUField(0xA92070A2, 24)] // size: 4
        public Enum_06FA22BB m_A92070A2;

        [STUField(0x30CD4137, 28)] // size: 4
        public float m_30CD4137 = 0.95f;

        [STUField(0x4579B961, 32)] // size: 4
        public float m_4579B961 = 0f;

        [STUField(0x2C0246C1, 36)] // size: 4
        public float m_2C0246C1 = 1f;

        [STUField(0xA3B3682C, 40)] // size: 1
        public byte m_A3B3682C = 0x0;
    }
}
