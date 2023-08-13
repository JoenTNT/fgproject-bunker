using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GetEntityBySetOfComponentType
{
    private int[] types = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    private Dictionary<int, HashSet<char>> registered = new Dictionary<int, HashSet<char>>
    {
        { 0, new HashSet<char> { 'D', 'E', 'H', 'J', 'L', 'M' } },
        { 1, new HashSet<char> { 'A', 'C', 'E', 'G', 'J', 'K' } },
        { 2, new HashSet<char> { 'A', 'B', 'F', 'I', 'K' } },
        { 3, new HashSet<char> { 'A', 'B', 'C', 'E', 'H' } },
        { 4, new HashSet<char> { 'B', 'D', 'E', 'J', 'O' } },
        { 5, new HashSet<char> { 'C', 'F', 'G', 'J', 'K' } },
        { 6, new HashSet<char> { 'A', 'C', 'F', 'J', 'L' } },
        { 7, new HashSet<char> { 'C', 'E', 'F', 'L' } },
        { 8, new HashSet<char> { 'G', 'I', 'K', 'N' } },
        { 9, new HashSet<char> { 'I', 'J', 'L', 'P' } },
        { 10, new HashSet<char> { 'A', 'C', 'G', 'K'} },
        { 11, new HashSet<char> { 'B', 'G', 'L', 'M', 'N'} },
        { 12, new HashSet<char> { 'A', 'H', 'K', 'M', 'O', 'P'} },
        { 13, new HashSet<char> { 'F', 'G', 'H', 'I', 'J', 'L', 'O' } },
        { 14, new HashSet<char> { 'A', 'B', 'C', 'E', 'G', 'K', 'N', 'P'} },
    };

    /// <summary>
    /// 2       : { 'A', 'B', 'F', 'I', 'K' }
    /// 2 & 5   : { 'F', 'K' }
    /// Expected Output: { 'F', 'K' }
    /// </summary>
    private int[] case1 = new int[] { 2, 5 };

    /// <summary>
    /// 1               : { 'A', 'C', 'E', 'G', 'J', 'K' }
    /// 1 & 3           : { 'A', 'C', 'E' }
    /// 1 & 3 & 5       : { 'C', }
    /// 1 & 3 & 5 & 6   : { 'C'}
    /// Expected Output: { 'C' }
    /// </summary>
    private int[] case2 = new int[] { 1, 3, 5, 6 };

    /// <summary>
    /// 5           : { 'C', 'F', 'G', 'J', 'K' }
    /// 5 & 7       : { 'C', 'F' }
    /// 5 & 7 & 9   : { }
    /// Expected Output: { }
    /// </summary>
    private int[] case3 = new int[] { 5, 7, 9 };

    [Test]
    public void GetEntityBySetOfComponentTypeSimplePasses()
    {
        //float startTime = Time.realtimeSinceStartup;

        //---------------------------------------------------------------------------------
        //var output1 = SolveCase(registered, case1);
        //Assert.AreEqual(output1, new char[] { 'F', 'K' });
        //---------------------------------------------------------------------------------

        //Debug.Log($"Solved Case 1 in {(int)(1000f * (Time.realtimeSinceStartup - startTime))} ms");
        //startTime = Time.realtimeSinceStartup;

        //---------------------------------------------------------------------------------
        //var output2 = SolveCase(registered, case2);
        //Assert.AreEqual(output2, new char[] { 'C' });
        //---------------------------------------------------------------------------------

        //Debug.Log($"Solved Case 2 in {(int)(1000f * (Time.realtimeSinceStartup - startTime))} ms");
        //startTime = Time.realtimeSinceStartup;

        //---------------------------------------------------------------------------------
        var output3 = SolveCase(registered, case3);
        //Assert.AreEqual(output3, new char[] { });
        //---------------------------------------------------------------------------------

        //Debug.Log($"Solved Case 3 in {(int)(1000f * (Time.realtimeSinceStartup - startTime))} ms");
    }

    public char[] SolveCase(Dictionary<int, HashSet<char>> dictionary, int[] cases)
    {
        int i, currentIResult, recentDeleteCount, type, placement, resultCount = 0;
        char[] temp = null, result = null;

        for (i = 0; i < cases.Length; i++)
        {
            type = cases[i];

            if (dictionary.ContainsKey(type))
            {
                if (i == 0)
                {
                    resultCount = dictionary[type].Count;
                    temp = new char[resultCount];
                    dictionary[type].CopyTo(temp, 0);
                    continue;
                }

                recentDeleteCount = placement = 0;
                for (currentIResult = 0; currentIResult < resultCount + recentDeleteCount; currentIResult++)
                {
                    if (!dictionary[type].Contains(temp[currentIResult]))
                    {
                        resultCount--;
                        recentDeleteCount++;
                    }
                    else
                    {
                        temp[placement] = temp[currentIResult];
                        placement++;
                    }
                }
            }
        }

        result = new char[resultCount];
        for (i = 0; i < resultCount; i++)
        {
            result[i] = temp[i];
        }

        return result;
    }
}
