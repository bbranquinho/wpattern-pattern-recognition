using System;
using WPattern.Pattern.Recognition.Core.Algorithms;
using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class GeneticAlgorithmParametersBean
    {
        public ClassifierParametersBean ClassifierParametersBean { get; set; }

        public int PopulationSize { get; set; }

        public int Generations { get; set; }

        public Double CrossoverRate { get; set; }

        public Double MutationRate { get; set; }
        
        public CrossoverType CrossoverType { get; set; }

        public MutationType MutationType { get; set; }

        public NaturalSelectionType NaturalSelection { get; set; }
    }
}