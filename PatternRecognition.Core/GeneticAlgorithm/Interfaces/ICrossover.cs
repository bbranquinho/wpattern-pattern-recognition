using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces
{
    public interface ICrossover
    {
        List<IndividualBean> ExecuteCrossover(IndividualBean individual1, IndividualBean individual2);

        List<IndividualBean> SelectIndividualToCrossover(List<IndividualBean> individuals, CrossoverType crossoverType);
    }
}