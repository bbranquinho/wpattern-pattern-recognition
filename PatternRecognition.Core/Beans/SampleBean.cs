using System.Collections.Generic;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class SampleBean
    {
        public StructureBean Structure { get; set; }

        public List<RecordBean> Records { get; set; }

        public SampleBean(StructureBean structure)
        {
            Structure = structure;
            Records = new List<RecordBean>();
        }
    }
}