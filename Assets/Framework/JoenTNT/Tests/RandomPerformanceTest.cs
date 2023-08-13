using System.Collections;
using System.Collections.Generic;
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
        //var start = System.DateTime.Now;

        for (int i = 0; i < 1000000; i++)
        {
            UnityEngine.Random.Range(int.MinValue, int.MinValue);
        }

        //var end = System.DateTime.Now;
        //var result = end - start;

        //Debug.Log($"Runtime (ms) : {(result).TotalSeconds}");
    }

    [Test]
    public void SystemRandomPerformanceTest()
    {
        //var start = System.DateTime.Now;

        for (int i = 0; i < 1000000; i++)
        {
            sysRand.Next(int.MinValue, int.MinValue);
        }

        //var end = System.DateTime.Now;
        //var result = end - start;

        //Debug.Log($"Runtime (ms) : {(result).TotalSeconds}");
    }

}
