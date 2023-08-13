using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CrystalFormula
{
    private const int MAGIC_VALUE_3DIGIT = 3158068;
    private const int MAGIC_VALUE_4DIGIT = 808464433;

    private int value1 = 336;

    [Test]
    public void Run()
    {
        Debug.Log(_3Digit(value1));

        //if (value1 / 1000 > 1)
        //    Debug.Log(_4Digit(value1));
        //else if (value1 / 100 > 1)
        //    Debug.Log(_3Digit(value1));
        //Debug.Log(value1);
    }

    private int _3Digit(int v)
    {
        int firstDigit = v / 100;
        int secondDigit = v % 100 / 10;
        int thirdDigit = v % 10;

        var result = (firstDigit - 1) + (secondDigit * 256)
            + (thirdDigit * (int)Mathf.Pow(256f, 2f));

        return MAGIC_VALUE_3DIGIT + result;
    }

    private int _4Digit(int v)
    {
        return MAGIC_VALUE_4DIGIT;
    }
}
