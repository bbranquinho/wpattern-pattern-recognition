using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WPattern.Pattern.Recognition.Core.Beans;
using WPattern.Pattern.Recognition.Core.Interfaces;

namespace WPattern.Pattern.Recognition.Core
{
    public class Loader : ILoader
    {
        public Loader()
        {
        }

        #region Public Methods (ILoader)
        public SampleBean LoadFile(string filepath)
        {
            List<RecordBean> records = new List<RecordBean>();

            using (StreamReader streamReader = new StreamReader(filepath))
            {
                String line = streamReader.ReadLine();
                string[] splitedLine = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                StructureBean sampleStructure = new StructureBean(Int32.Parse(splitedLine[1]), Int32.Parse(splitedLine[2]));

                line = streamReader.ReadLine();
                splitedLine = line.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                sampleStructure.AttributeNames = splitedLine;

                SampleBean sample = new SampleBean(sampleStructure);

                while (!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();

                    if (line != null)
                    {
                        records.Add(ProcessLine(line));
                    }
                }

                sample.Records = records;

                return sample;
            }
        }
        #endregion

        #region Private Methods
        private RecordBean ProcessLine(String line)
        {
            string[] attributes = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            RecordBean record = new RecordBean(attributes.Length - 1);

            for (int i = 0; i < attributes.Length; i++)
            {
                record.SetAttribute(i, Double.Parse(attributes[i].Trim().Replace(".", ",")));
            }

            record.Class = Int32.Parse(attributes[attributes.Length - 1]);

            return record;
        }
        #endregion
    }
}