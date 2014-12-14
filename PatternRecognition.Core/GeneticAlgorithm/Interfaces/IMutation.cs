using System;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces
{
    public interface IMutation
    {
        List<IndividualBean> ExecuteMutation(List<IndividualBean> individuals, Double mutationRate, MutationType mutationType);
    }
}