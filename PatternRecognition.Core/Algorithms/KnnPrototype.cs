using System;
using System.Collections.Generic;
using System.Linq;
using WPattern.Pattern.Recognition.Core.Algorithms.Interfaces;
using WPattern.Pattern.Recognition.Core.Algorithms.Utils;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms
{
    public class KnnPrototype : IClassifier
    {
         #region Properties
        private Normalization Normalization { get; set; }
        #endregion

        #region Constructor
        public KnnPrototype()
        {
            Normalization = new Normalization();
        }
        #endregion

        #region Public Methods
        public ClassifierResultBean Classify(SampleBean sample, ClassifierParametersBean classifierParametersBean)
        {
            ConfusionMatrixBean confusionMatrix = new ConfusionMatrixBean(sample.Structure.AmountClass);
            List<RecordBean> records = Normalization.ProcessNormalization(sample, classifierParametersBean.NormalizationType);
            int amountRecords = records.Count;

            for (int i = 0; i < amountRecords; i++)
            {
                List<RecordDistanceBean> recordDistances = new List<RecordDistanceBean>(amountRecords);
                RecordBean testRecord = records[i];

                for (int j = 0; j < amountRecords; j++)
                {
                    if (i != j)
                    {
                        Double distance = Distance.CalculateEuclideanDistance(testRecord, records[j]);

                        recordDistances.Add(new RecordDistanceBean(records[j], distance));
                    }
                }

                List<RecordBean> aggregatedRecords = ClassifierUtils.CalculateAggregatedRecords(recordDistances, classifierParametersBean.K);
                List<RecordDistanceBean> aggregatedDistance = ClassifierUtils.CalculateAggregatedDistance(testRecord, aggregatedRecords);

                confusionMatrix.PredicatedClass(testRecord.Class, ClassifierUtils.FindAverageClass(testRecord, aggregatedDistance));
            }

            return new ClassifierResultBean() { ConfusionMatrix = confusionMatrix };
        }

        #endregion

        #region Private Methods
        #endregion

        #region Nested Class's
        public class ClassifiedRecordBean
        {
            public Int32 FoundedClass { get; set; }

            public Int32 RecordClass { get; set; }
        }
        #endregion
    }
}