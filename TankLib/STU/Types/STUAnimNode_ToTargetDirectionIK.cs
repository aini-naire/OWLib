// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0xB89B3EF9, 696)]
    public class STUAnimNode_ToTargetDirectionIK : STUAnimNode_Base
    {
        [STUField(0x8096CD9B, 96, ReaderType = typeof(InlineInstanceFieldReader))] // size: 248
        public STUAnimBlendDriverParam m_8096CD9B;

        [STUField(0x4ABE6B19, 344, ReaderType = typeof(InlineInstanceFieldReader))] // size: 248
        public STUAnimBlendDriverParam m_4ABE6B19;

        [STUField(0x0F88291F, 592, ReaderType = typeof(InlineInstanceFieldReader))] // size: 32
        public STU_25B808BD m_weightDriver;

        [STUField(0xE33AA964, 624, ReaderType = typeof(InlineInstanceFieldReader))] // size: 24
        public STU_ABD8FE73 m_E33AA964;

        [STUField(0xC15DCB47, 648, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STUAnimNode_ToTargetDirectionIKItem[] m_IKItems;

        [STUField(0xAB06D14B, 664, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STUAnimConfigBoneWeight_Item[] m_AB06D14B;

        [STUField(0x436BEFE6, 680, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_2F6BD485 m_child;

        [STUField(0xCC95F1B0, 688, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 8
        public STU_5861C542 m_CC95F1B0;
    }
}
