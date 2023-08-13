using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LinqPerformance
{
    private string[] set1 = new string[] { "New", "Year", "Entity" };
    private string[] set2 = new string[] { "New", "Entity" };

    private Dictionary<string, string> dictionary1 = new Dictionary<string, string>()
    {
        { "A", "Year" },
        { "B", "New" },
    };

    [Test]
    public void LinqPerformanceSimplePasses()
    {
        
    }

}
