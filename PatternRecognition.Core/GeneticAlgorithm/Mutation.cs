using System;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm
{
    public class Mutation : IMutation
    {
        #region Properties
        private Random Random { get; set; }
        #endregion

        #region Constructors
        public Mutation()
        {
            Random = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        }
        #endregion

        #region Public Methods (IMutation)
        public List<IndividualBean> ExecuteMutation(List<IndividualBean> individuals, Double mutationRate, MutationType mutationType)
        {
            if (individuals.Count <= 0)
            {
                return individuals;
            }

            mutationRate /= 100.0;

            int amountAttributes = individuals[0].ValidAttributes.Length;

            foreach (IndividualBean individual in individuals)
            {
                if (Random.NextDouble() < mutationRate)
                {
                    switch (mutationType)
                    {
                        case MutationType.SIMPLE:
                            individual.ValidAttributes[Random.Next(amountAttributes)] = !individual.ValidAttributes[Random.Next(amountAttributes)];
                            break;

                        case MutationType.SWITCH:
                            int startIndex = Random.Next(amountAttributes);
                            int lastIndex = Random.Next(amountAttributes);
                            bool aux = individual.ValidAttributes[startIndex];

                            individual.ValidAttributes[startIndex] = individual.ValidAttributes[lastIndex];
                            individual.ValidAttributes[lastIndex] = aux;
                            break;
                    }
                }
            }

            return individuals;
        }
        #endregion
    }
}