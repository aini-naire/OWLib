// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0xF5B49504, 32)]
    public class STUCustomGameBrowserMapCatalogInfo : STUInstance
    {
        [STUField(0x2C05CD95, 0)] // size: 16
        public teStructuredDataAssetRef<STUMapCatalog> m_catalog;

        [STUField(0xB48F1D22, 16)] // size: 16
        public teStructuredDataAssetRef<ulong> m_name;
    }
}
