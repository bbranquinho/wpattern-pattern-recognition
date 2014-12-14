using WPattern.Pattern.Recognition.Core.Algorithms.Enums;

namespace WPattern.Pattern.Recognition.Core.Algorithms
{
    public class ClassifierParameters
    {
        public ClassifierType ClassifierType { get; set; }

        public int K { get; set; }

        public int KFold { get; set; }

        public CrossValidationType CrossValidationType { get; set; }

        public NormalizationType NormalizationType { get; set; }
    }
}