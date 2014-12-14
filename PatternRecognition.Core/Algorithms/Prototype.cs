using System;
using System.Collections.Generic;
using System.Linq;
using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.Algorithms.Interfaces;
using WPattern.Pattern.Recognition.Core.Algorithms.Utils;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms
{
    public class Prototype : IClassifier
    {
        #region Properties
        private Normalization Normalization { get; set; }
        #endregion

        #region Constructor
        public Prototype()
        {
            Normalization = new Normalization();
        }
        #endregion

        #region Public Methods
        public ClassifierResultBean Classify(SampleBean sample, ClassifierParametersBean classifierParametersBean)
        {
            List<RecordBean> records = Normalization.ProcessNormalization(sample, classifierParametersBean.NormalizationType);
            Dictionary<int, AggregatedRecordBean> recordsByClass = records.Aggregate(new Dictionary<int, AggregatedRecordBean>(), ClassifierUtils.ExecuteAggregateRecords);
            List<RecordBean> aggregatedRecords = new List<RecordBean>();

            foreach (AggregatedRecordBean aggregatedRecord in recordsByClass.Values)
            {
                for (int i = 0; i < aggregatedRecord.Record.Attributes.Length; i++)
                {
                    aggregatedRecord.Record.Attributes[i] /= aggregatedRecord.AmountRecords;
                }

                aggregatedRecords.Add(aggregatedRecord.Record);
            }

            return ClassifyByAggregatedRecords(aggregatedRecords, records, sample.Structure.AmountClass);
        }
        #endregion

        #region Private Methods
        private ClassifierResultBean ClassifyByAggregatedRecords(List<RecordBean> aggregatedRecords, List<RecordBean> records, int amountClass)
        {
            ConfusionMatrixBean confusionMatrix = new ConfusionMatrixBean(amountClass);
            int amountRecords = records.Count;

            foreach (RecordBean testRecord in records)
            {
                List<RecordDistanceBean> recordDistances = new List<RecordDistanceBean>(amountRecords);

                foreach (RecordBean aggregatedRecord in aggregatedRecords)
                {
                    Double distance = Distance.CalculateEuclideanDistance(testRecord, aggregatedRecord);

                    recordDistances.Add(new RecordDistanceBean(aggregatedRecord, distance));
                }

                recordDistances = recordDistances.OrderBy(r => r.Distance).ToList();

                confusionMatrix.PredicatedClass(testRecord.Class, recordDistances.First().Record.Class);
            }

            return new ClassifierResultBean() { ConfusionMatrix = confusionMatrix };
        }
        #endregion
    }
}