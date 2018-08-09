// Instance generated by TankLibHelper.InstanceBuilder
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0xAF3DFC87, "STULootBoxRules")]
    public class STULootBoxRules : STUInstance {
        [STUFieldAttribute(0xB7C0634C, "m_rarityRules", ReaderType = typeof(InlineInstanceFieldReader))]
        public STULootBoxRarityRules[] m_rarityRules;

        [STUFieldAttribute(0x96D86FB8, "m_currencyRarityRules", ReaderType = typeof(InlineInstanceFieldReader))]
        public STULootBoxRarityRules[] m_currencyRarityRules;

        [STUFieldAttribute(0x7AB4E3F8, "m_lootboxType")]
        public Enum_BABC4175 m_lootboxType;

        [STUFieldAttribute(0x24391F9B)]
        public int m_24391F9B;

        [STUFieldAttribute(0x53B8C818)]
        public int m_53B8C818;
    }
}