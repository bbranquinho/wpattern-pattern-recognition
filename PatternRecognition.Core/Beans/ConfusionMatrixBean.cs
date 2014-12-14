using System;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class ConfusionMatrixBean
    {
        private int AmountAttributes { get; set; }

        public int[][] ConfusionMatrix { get; private set; }

        public ConfusionMatrixBean(int amountAttributes)
        {
            AmountAttributes = amountAttributes;
            ConfusionMatrix = new int[amountAttributes][];
        }

        public void PredicatedClass(int actual, int predicated)
        {
            if (ConfusionMatrix[actual - 1] == null)
            {
                ConfusionMatrix[actual - 1] = new int[AmountAttributes];
            }

            ConfusionMatrix[actual - 1][predicated - 1]++;
        }
    }
}