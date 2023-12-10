using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RandomPerformanceTest
{
    private System.Random sysRand;

    [SetUp]
    public void SetUp()
    {
        sysRand = new System.Random();
    }

    [Test]
    public void UnityRandomPerformanceTest()
    {
        var start = System.DateTime.Now;

        for (int i = 0; i < 100000000; i++)
        {
            Random.Range(int.MinValue, int.MinValue);
        }

        var end = System.DateTime.Now;
        var result = end - start;

        Debug.Log($"Using Unity Runtime (ms) : {(result).TotalSeconds}");
    }

    [Test]
    public void SystemRandomPerformanceTest()
    {
        var start = System.DateTime.Now;

        for (int i = 0; i < 100000000; i++)
        {
            sysRand.Next(int.MinValue, int.MinValue);
        }

        var end = System.DateTime.Now;
        var result = end - start;

        Debug.Log($"Using System Runtime (ms) : {(result).TotalSeconds}");
    }

}
