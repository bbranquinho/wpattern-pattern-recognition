using System;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class StructureBean
    {
        public String[] AttributeNames { get; set; }

        public int AmountAttributes { get; set; }

        public int AmountClass { get; set; }

        public StructureBean(int amountAttributes, int amountClass)
        {
            AmountClass = amountClass;
            AmountAttributes = amountAttributes;
            AttributeNames = new string[amountAttributes];
        }

        public StructureBean(String[] attributeNames, int amountClass)
            : this(attributeNames.Length - 1, amountClass)
        {
            AttributeNames = attributeNames;
        }
    }
}