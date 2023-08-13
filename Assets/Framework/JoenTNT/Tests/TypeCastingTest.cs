using System.Collections;
using System.Collections.Generic;
//using JT.ECS;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TypeCastingTest
{
    private Component entityComponent = null;
    //private System.Type type = null;

    //private IEntityComponent entity = null;

    [SetUp]
    public void Setup()
    {
        GameObject sampleObj = new GameObject();
        //entityComponent = sampleObj.AddComponent<EntityID>();
        //type = typeof(IEntityComponent);
    }

    [Test]
    public void TypeCastAll()
    {
        //entity = (IEntityComponent)System.Convert.ChangeType(entityComponent, type);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(entityComponent.gameObject);

        Debug.Log($"Running Time Since Startup: {Time.realtimeSinceStartup}");
    }

}
