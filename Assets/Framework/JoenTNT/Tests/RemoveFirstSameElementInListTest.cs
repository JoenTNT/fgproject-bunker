using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RemoveFirstSameElementInListTest
{
    private List<string> _sampleListOfStrings = null;
    private readonly string[] _sampleStringData = { "Andi", "Barbara", "Andi", "Yeoul", "Boyuy", "Andi", "Samuel" };

    [SetUp]
    public void InitListSetup()
    {
        _sampleListOfStrings = new List<string>(_sampleStringData);
    }

    [Test]
    public void RemoveFirstSameElementInListTesterSimplePasses()
    {
        var res = "";

        foreach (var s in _sampleListOfStrings)
        {
            res += $"{s}; ";
        }

        Debug.Log(res);

        res = "";
        _sampleListOfStrings.Remove("Andi");

        foreach (var s in _sampleListOfStrings)
        {
            res += $"{s}; ";
        }

        Debug.Log(res);
    }

    public void DestroyList()
    {
        _sampleListOfStrings = null;
    }
}
