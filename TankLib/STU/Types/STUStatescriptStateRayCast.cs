// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x019E0826, 368)]
    public class STUStatescriptStateRayCast : STUStatescriptState
    {
        [STUField(0x03AD3873, 232, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 16
        public STUConfigVar[] m_ignoreEntities;

        [STUField(0x1A44BF69, 248, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_startPosWS;

        [STUField(0x40FFACD9, 256, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_dirWS;

        [STUField(0xBC946B94, 264, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_castLength;

        [STUField(0x1F94170C, 272, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_castRadius;

        [STUField(0x8CBE558B, 280, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_8CBE558B;

        [STUField(0x1FE42229, 288, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_1FE42229;

        [STUField(0x3B0AD973, 296, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_3B0AD973;

        [STUField(0xEC356953, 304, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_EC356953;

        [STUField(0x7404C882, 312, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVarFilter m_castFilter;

        [STUField(0x23974AAB, 320, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_076E0DBA m_out_HitSomething;

        [STUField(0x70E41E8A, 328, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_076E0DBA m_out_HitEntity;

        [STUField(0xB3FC76F3, 336, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_076E0DBA m_out_HitPointWS;

        [STUField(0x3BDBD513, 344, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_076E0DBA m_out_HitNormal;

        [STUField(0x5E40B7CE, 352, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STUConfigVar m_5E40B7CE;

        [STUField(0x0619C597, 360)] // size: 4
        public Enum_54CE6D16 m_type;
    }
}
