using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.Algorithms.Interfaces;

namespace WPattern.Pattern.Recognition.Core.Algorithms.Utils
{
    public class ClassifierFactory
    {
        public static IClassifier GetClassifierInstance(ClassifierType classifierType)
        {
            switch (classifierType)
            {
                case ClassifierType.KNN:
                    return new Knn();

                case ClassifierType.PROTOTYPE:
                    return new Prototype();

                case ClassifierType.KNN_PROTOTYPE:
                    return new KnnPrototype();

                default:
                    return null;
            }
        }
    }
}