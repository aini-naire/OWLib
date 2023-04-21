// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0xD797394C, 88)]
    public class STUGameModeTeam : STUInstance
    {
        [STUField(0xA2781AA4, 0)] // size: 16
        public teStructuredDataAssetRef<STU_6BE90C5C> m_controllerScript;

        [STUField(0x6F71E9AA, 16, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STUGameModeVarValuePair[] m_6F71E9AA;

        [STUField(0x76E8C82A, 32)] // size: 16
        public teStructuredDataAssetRef<STU_6BE90C5C> m_bodyScript;

        [STUField(0xEA2B516F, 48, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STUGameModeBodyVars[] m_bodyVars;

        [STUField(0x59C86C8D, 64, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_5427ADC1 m_availableHeroes;

        [STUField(0x33B0B2B6, 72)] // size: 4
        public TeamIndex m_team;

        [STUField(0x7FA93ED4, 76)] // size: 4
        public int m_7FA93ED4;

        [STUField(0x8B3CD15B, 80)] // size: 4
        public int m_8B3CD15B;

        [STUField(0x170AA4B8, 84)] // size: 4
        public int m_170AA4B8;
    }
}
