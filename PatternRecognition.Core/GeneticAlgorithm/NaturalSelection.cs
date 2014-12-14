using System.Linq;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm
{
    public class NaturalSelection : INaturalSelection
    {
        public List<IndividualBean> ExecuteNaturalSelection(List<IndividualBean> population, List<IndividualBean> chieldrens, int populationSize, NaturalSelectionType naturalSelectionType)
        {
            switch (naturalSelectionType)
            {
                case NaturalSelectionType.BESTS:
                    foreach (IndividualBean individual in chieldrens.Where(individual => !population.Contains(individual)))
                    {
                        population.Add(individual);
                    }

                    population = population.OrderByDescending(i => i.Fitness).ToList();

                    if (population.Count > populationSize)
                    {
                        population.RemoveRange(populationSize - 1, population.Count - populationSize);
                    }
                    break;
            }

            return population;
        }
    }
}