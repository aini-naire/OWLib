// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x8DEFF143, 72)]
    public class STUImpactEffectByCategory : STUInstance
    {
        [STUField(0x2C54AEAF, 0)] // size: 16
        public teStructuredDataAssetRef<STUIdentifier> m_category;

        [STUField(0x58D56DD4, 16)] // size: 16
        public teStructuredDataAssetRef<ulong> m_effect;

        [STUField(0x041CE51F, 32)] // size: 16
        public teStructuredDataAssetRef<STUModelLook> m_modelLook;

        [STUField(0x24B94612, 48)] // size: 16
        public teStructuredDataAssetRef<STUIdentifier> m_24B94612;

        [STUField(0x93CDFD2F, 64)] // size: 4
        public Enum_6842E392 m_93CDFD2F;
    }
}
