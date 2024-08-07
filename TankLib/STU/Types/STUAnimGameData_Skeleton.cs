// Generated by TankLibHelper
using TankLib.Math;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x2BA91EC5, 96)]
    public class STUAnimGameData_Skeleton : STUInstance
    {
        [STUField(0xBBD2BB2F, 8)] // size: 16
        public int[] m_BBD2BB2F;

        [STUField(0x02FE7B57, 24)] // size: 16
        public sbyte[] m_parentIndices;

        [STUField(0x8084B68D, 40)] // size: 16
        public teVec3A[] m_8084B68D;

        [STUField(0x448225F5, 56)] // size: 16
        public teVec3A[] m_448225F5;

        [STUField(0xADA41246, 72)] // size: 16
        public uint[] m_ADA41246;

        [STUField(0x29EFF18D, 88)] // size: 4
        public uint m_crc;
    }
}
