using System;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class RecordBean
    {
        public Double[] Attributes { get; private set; }

        public Int32 Class { get; set; }
        
        public RecordBean(int amountAttributes)
        {
            Attributes = new Double[amountAttributes];
        }

        public void SetAttribute(int index, Double value)
        {
            if ((index >= 0) && (index < Attributes.Length))
            {
                Attributes[index] = value;
            }
        }

        public Double GetField(int index)
        {
            return Attributes[index];
        }
    }
}