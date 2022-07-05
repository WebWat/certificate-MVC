using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers;

public class EnumerableHelper
{
    public static List<string> GetYears(string allValue)
    {
        // Create an integer list, flip it, and convert it to a list of strings.
        var years = Enumerable.Range(2000, DateTime.Now.Year - 1999)
                              .Reverse()
                              .Select(i => i.ToString())
                              .ToList();

        years.Insert(0, allValue);

        return years;
    }
}
