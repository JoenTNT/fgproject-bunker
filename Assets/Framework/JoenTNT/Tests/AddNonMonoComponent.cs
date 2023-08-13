using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AddNonMonoComponent
{
    private GameObject _targetObject = null;

    [SetUp]
    public void SetUp()
    {
        
    }

    [Test]
    public void AddNonMonoComponentSimplePasses()
    {
        _targetObject = new GameObject("Sample GameObject", typeof(SampleComponent));
    }
}