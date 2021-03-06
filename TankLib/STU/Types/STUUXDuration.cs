// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x1815624A, 32)]
    public class STUUXDuration : STUUXObject
    {
        [STUField(0x42636A6F, 8, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STU_D3733E9D m_timeSpan;
        
        [STUField(0x1930BE9A, 24)] // size: 4
        public Enum_17FD8E7C m_durationBehavior;
    }
}
