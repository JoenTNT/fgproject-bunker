using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StructToObject
{
    // A Test behaves as an ordinary method
    [Test]
    public void StructToObjectSimplePasses()
    {
        object a = new SampleStruct { sampleValue = 5 };

        Assert.IsTrue(a is SampleStruct);
        Assert.IsTrue(a is ISampleInterface);

        if (a is SampleStruct)
        {
            Debug.Log(((SampleStruct)a).sampleValue);
        }

        if (a is ISampleInterface)
        {
            Assert.IsFalse(a == null);
        }
    }
}

public interface ISampleInterface { }

public struct SampleStruct : ISampleInterface
{
    public int sampleValue;
}
