using System;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class RecordDistanceBean
    {
        public Double Distance { get; set; }

        public RecordBean Record { get; set; }

        public RecordDistanceBean(RecordBean record, double distance)
        {
            Distance = distance;
            Record = record;
        }
    }
}