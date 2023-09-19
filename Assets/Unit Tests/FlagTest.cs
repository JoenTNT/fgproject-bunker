using System;
using System.Collections;
using System.Collections.Generic;
using JT.FGP;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FlagTest
{
    [Test]
    public void FlagTestPasses()
    {
        FlagTesting();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator FlagTestWithEnumeratorPasses()
    {
        // Skip Frame now.
        yield return null;

        FlagTesting();
    }

    private void FlagTesting()
    {
        Array e = Enum.GetValues(typeof(InGameLightingType));
        InGameLightingType lightingType;
        for (int i = 0; i < e.Length; i++)
        {
            lightingType = (InGameLightingType)e.GetValue(i);
            Debug.Log(lightingType);
        }

        Assert.IsTrue(true);
    }

}
