// Generated by TankLibHelper

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x3C6A88BF, 624)]
    public class STUAnimNode_Blend2d : STUAnimNode_Base
    {
        [STUField(0xF0CF068D, 80, ReaderType = typeof(InlineInstanceFieldReader))] // size: 248
        public STUAnimBlendDriverParam m_F0CF068D;
        
        [STUField(0x8D244EC2, 328, ReaderType = typeof(InlineInstanceFieldReader))] // size: 248
        public STUAnimBlendDriverParam m_8D244EC2;
        
        [STUField(0xEEA81ECC, 576, ReaderType = typeof(InlineInstanceFieldReader))] // size: 32
        public STUAnimTriangulationMap m_EEA81ECC;
        
        [STUField(0x134EE5BB, 608, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 16
        public STUAnimNode_Blend2dChild[] m_children;
    }
}
