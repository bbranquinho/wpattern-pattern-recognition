using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces
{
    public interface IGeneticAlgorithm
    {
        IndividualBean Execute(SampleBean sample, GeneticAlgorithmParametersBean parameters);
    }
}