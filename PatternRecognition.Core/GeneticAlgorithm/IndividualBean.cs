using System;
using System.Linq;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm
{
    public class IndividualBean
    {
        #region Properties
        public ClassifierResultBean ClassifierResult { get; set; }

        public bool[] ValidAttributes { get; private set; }

        public double Fitness { get; set; }
        #endregion

        #region Constructors
        public IndividualBean(int amountAttributes)
        {
            ValidAttributes = new bool[amountAttributes];
        }
        #endregion

        #region Public Methods
        public String GetKey()
        {
            return ValidAttributes.Aggregate("", (s, b) => s + (b ? "0" : "1"));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            IndividualBean individual = (IndividualBean) obj;

            for (int i = 0; i < ValidAttributes.Length; i++)
            {
                if (ValidAttributes[i] != individual.ValidAttributes[i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}