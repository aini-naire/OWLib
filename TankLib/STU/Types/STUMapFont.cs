// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0xE1111C8D, 24)]
    public class STUMapFont : STUInstance
    {
        [STUField(0xA1AD5DD2, 8, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUMapFontHeader m_header;

        [STUField(0x25274294, 16, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUMapFontData m_data;
    }
}
