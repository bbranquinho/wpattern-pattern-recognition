using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms.Interfaces
{
    public interface IClassifier
    {
        ClassifierResultBean Classify(SampleBean sample, ClassifierParametersBean classifierParametersBean);
    }
}