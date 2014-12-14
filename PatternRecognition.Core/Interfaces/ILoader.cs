using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.Beans;

namespace WPattern.Pattern.Recognition.Core.Interfaces
{
    public interface ILoader
    {
        SampleBean LoadFile(string filepath);
    }
}