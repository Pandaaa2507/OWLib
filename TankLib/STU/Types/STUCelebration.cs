// Instance generated by TankLibHelper.InstanceBuilder

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0xACCDE63F, "STUCelebration")]
    public class STUCelebration : STUInstance {
        [STUFieldAttribute(0x71D9486D, "m_celebrationUnlocks", ReaderType = typeof(InlineInstanceFieldReader))]
        public STUUnlocks m_celebrationUnlocks;

        [STUFieldAttribute(0xED999C8B, "m_celebrationType")]
        public teStructuredDataAssetRef<STUIdentifier> m_celebrationType;

        [STUFieldAttribute(0xF81F4386, "m_startTime")]
        public teStructuredDataDateAndTime m_startTime;

        [STUFieldAttribute(0xFBEBAD6F, "m_endTime")]
        public teStructuredDataDateAndTime m_endTime;

        [STUFieldAttribute(0xEDE36CB7)]
        public ulong m_EDE36CB7;
    }
}
