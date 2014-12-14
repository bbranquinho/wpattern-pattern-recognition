using System;
using System.Collections.Generic;
using System.Linq;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms.Utils
{
    public class ClassifierUtils
    {
        #region Public Methods
        public static Dictionary<int, AggregatedRecordBean> ExecuteAggregateRecords(Dictionary<int, AggregatedRecordBean> aggregatedRecord, RecordBean record)
        {
            AggregatedRecordBean recordValue;

            if (!aggregatedRecord.TryGetValue(record.Class, out recordValue))
            {
                recordValue = new AggregatedRecordBean();
                recordValue.Record = new RecordBean(record.Attributes.Length);
                record.Attributes.CopyTo(recordValue.Record.Attributes, 0);
                recordValue.AmountRecords = 1;
                recordValue.Record.Class = record.Class;
                aggregatedRecord.Add(recordValue.Record.Class, recordValue);
            }
            else
            {
                for (int i = 0; i < record.Attributes.Length; i++)
                {
                    recordValue.Record.Attributes[i] += record.Attributes[i];
                }

                recordValue.AmountRecords++;
            }

            return aggregatedRecord;
        }

        public static Int32 FindClass(List<RecordDistanceBean> recordDistances, int k)
        {
            Dictionary<Int32, Int32> countClasses = new Dictionary<int, int>();
            Int32 value;

            recordDistances = recordDistances.OrderBy(r => r.Distance).ToList();

            for (int i = 0; i < k; i++)
            {
                if (!countClasses.TryGetValue(recordDistances[i].Record.Class, out value))
                {
                    countClasses.Add(recordDistances[i].Record.Class, 1);
                }
                else
                {
                    countClasses[recordDistances[i].Record.Class]++;
                }
            }

            return countClasses.OrderByDescending(c => c.Value).First().Key;
        }

        public static Int32 FindAverageClass(RecordBean record, List<RecordDistanceBean> recordDistances)
        {
            return recordDistances.OrderBy(r => r.Distance).First().Record.Class;
        }

        public static List<RecordDistanceBean> CalculateAggregatedDistance(RecordBean testRecord, List<RecordBean> aggregatedRecords)
        {
            List<RecordDistanceBean> aggregatedDistance = new List<RecordDistanceBean>(aggregatedRecords.Count);

            for (int i = 0; i < aggregatedRecords.Count; i++)
            {
                Double distance = Distance.CalculateEuclideanDistance(testRecord, aggregatedRecords[i]);

                aggregatedDistance.Add(new RecordDistanceBean(aggregatedRecords[i], distance));
            }

            return aggregatedDistance;
        }

        public static List<RecordBean> CalculateAggregatedRecords(List<RecordDistanceBean> recordDistances, int k)
        {
            Dictionary<Int32, AggregatedRecordBean> aggregatedRecordByClass = new Dictionary<int, AggregatedRecordBean>();
            AggregatedRecordBean recordValue;

            recordDistances = recordDistances.OrderBy(r => r.Distance).ToList();

            // Aggregated "k" records.
            for (int i = 0; i < k; i++)
            {
                RecordBean record = recordDistances[i].Record;

                if (!aggregatedRecordByClass.TryGetValue(record.Class, out recordValue))
                {
                    recordValue = new AggregatedRecordBean();
                    recordValue.Record = new RecordBean(record.Attributes.Length);
                    record.Attributes.CopyTo(recordValue.Record.Attributes, 0);
                    recordValue.AmountRecords = 1;
                    recordValue.Record.Class = record.Class;
                    aggregatedRecordByClass.Add(recordValue.Record.Class, recordValue);
                }
                else
                {
                    for (int j = 0; j < record.Attributes.Length; j++)
                    {
                        recordValue.Record.Attributes[j] += record.Attributes[j];
                    }

                    recordValue.AmountRecords++;
                }
            }

            // Calculate the average for each class.
            List<RecordBean> aggregatedRecords = new List<RecordBean>();

            foreach (AggregatedRecordBean aggregatedRecord in aggregatedRecordByClass.Values)
            {
                for (int i = 0; i < aggregatedRecord.Record.Attributes.Length; i++)
                {
                    aggregatedRecord.Record.Attributes[i] /= aggregatedRecord.AmountRecords;
                }

                aggregatedRecords.Add(aggregatedRecord.Record);
            }

            return aggregatedRecords;
        }
        #endregion
    }
}
