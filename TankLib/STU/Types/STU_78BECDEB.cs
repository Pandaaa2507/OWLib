// Instance generated by TankLibHelper.InstanceBuilder

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0x78BECDEB)]
    public class STU_78BECDEB : STUInstance {
        [STUFieldAttribute(0x11B47C68, "m_id")]
        public teStructuredDataAssetRef<STUIdentifier> m_id;

        [STUFieldAttribute(0x5DB91CE2, "m_displayName")]
        public teStructuredDataAssetRef<STUUXDisplayText> m_displayName;

        [STUFieldAttribute(0x4CCC44C4, ReaderType = typeof(InlineInstanceFieldReader))]
        public STU_5389D651[] m_4CCC44C4;
    }
}
