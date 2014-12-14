using System;
using System.Linq;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms
{
    public class Normalization
    {
        public List<RecordBean> ProcessNormalization(SampleBean sample, NormalizationType normalizationType)
        {
            if (normalizationType == NormalizationType.NONE)
            {
                return sample.Records;
            }

            double[] means = new double[sample.Structure.AmountAttributes];
            double[] standardDeviation = new double[sample.Structure.AmountAttributes];

            // 1. Calculate mean of each attribute.
            foreach(RecordBean record in sample.Records)
            {
                for (int i = 0; i < sample.Structure.AmountAttributes; i++)
                {
                    means[i] += record.Attributes[i];
                }
            }

            for (int i = 0; i < sample.Structure.AmountAttributes; i++)
            {
                means[i] /= sample.Records.Count;
            }

            // 2. Calculate standard deviation of each attribute.
            foreach (RecordBean record in sample.Records)
            {
                for (int i = 0; i < sample.Structure.AmountAttributes; i++)
                {
                    standardDeviation[i] += Math.Pow(record.Attributes[i] - means[i], 2.0);
                }
            }

            for (int i = 0; i < sample.Structure.AmountAttributes; i++)
            {
                int amountRecords = (sample.Records.Count > 1) ? sample.Records.Count - 1 : 1;
                standardDeviation[i] = Math.Sqrt(standardDeviation[i] / amountRecords);
            }
            
            // 3. Normalize each record.
            List<RecordBean> normalizedRecords = new List<RecordBean>(sample.Records.Count);

            foreach (RecordBean record in sample.Records)
            {
                RecordBean normalizedRecord = new RecordBean(sample.Structure.AmountAttributes);

                for (int i = 0; i < sample.Structure.AmountAttributes; i++)
                {
                    normalizedRecord.Attributes[i] = (record.Attributes[i] - means[i]) / standardDeviation[i];
                }

                normalizedRecord.Class = record.Class;
                normalizedRecords.Add(normalizedRecord);
            }

            return normalizedRecords;
        }
    }
}