using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces
{
    public interface INaturalSelection
    {
        List<IndividualBean> ExecuteNaturalSelection(List<IndividualBean> population, List<IndividualBean> chieldrens,
                                                     int populationSize, NaturalSelectionType naturalSelectionType);
    }
}