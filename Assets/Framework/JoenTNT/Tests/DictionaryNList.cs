using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DictionaryNList
{
    private Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
    private List<GameObject> list = new List<GameObject>();
    private HashSet<GameObject> hash = new HashSet<GameObject>();

    private HashSet<int> intHash = new HashSet<int>();

    private int caseNumber = 76329;

    [SetUp]
    public void SetUp()
    {
        GameObject go;

        for (int i = 0; i < 1; i++)
        {
            go = new GameObject($"{i}");
            dictionary.Add($"{i}", go);
            list.Add(go);
            hash.Add(go);
        }
    }

    [TearDown]
    public void TearDown()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Object.Destroy(list[i]);
            list.RemoveAt(i);
        }

        dictionary.Clear();
        hash.Clear();
    }

    [Test]
    public void DictionaryVsListPerformance()
    {
        float timeStart = Time.realtimeSinceStartup;

        foreach (var i in list)
        {
            if (i.name == caseNumber.ToString())
            {
                Debug.Log($"[LIST] Found At Time: {Time.realtimeSinceStartup - timeStart}");
                break;
            }
        }

        timeStart = Time.realtimeSinceStartup;

        if (dictionary.ContainsKey(caseNumber.ToString()))
        {
            Debug.Log($"[DICTIONARY] Found At Time: {Time.realtimeSinceStartup - timeStart}");
        }
    }

    [Test]
    public void UsingHashPerformance()
    {
        //var k = dictionary.Keys; // GC : 72 KB

        //var k = new string[dictionary.Count];
        //dictionary.Keys.CopyTo(k, 0);

        for (int i = 0; i < 100000; i++)
        {
            intHash.Add(i);
        }

        for (int i = 0; i < 100000; i++)
        {
            intHash.Remove(i);
        }
    }
}
