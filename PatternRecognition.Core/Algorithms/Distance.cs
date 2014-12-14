using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Algorithms
{
    public class Distance
    {
        public static Double CalculateEuclideanDistance(RecordBean firstRecord, RecordBean secondRecord)
        {
            Double sum = 0.0;
            double[] firstAttributes = firstRecord.Attributes;
            double[] secondAttributes = secondRecord.Attributes;

            for (int i = 0; i < firstAttributes.Length; i++)
            {
                sum += Math.Pow(firstAttributes[i] - secondAttributes[i], 2.0);
            }

            return Math.Sqrt(sum);
        }
    }
}
