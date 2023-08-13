using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SubsetOfKeys
{
    private Type[] set1 = new Type[] { typeof(int), typeof(string), typeof(bool) };
    private Type[] set2 = new Type[] { typeof(int), typeof(bool) };

    private Dictionary<Type, string> dictionary1 = new Dictionary<Type, string>()
    {
        { typeof(string), "A" },
        { typeof(int), "B" },
    };

    [Test]
    public void SubsetOfKeysSimplePasses()
    {
        //Assert.IsFalse(IsSubsetOf(set2, set1));
        //Assert.IsTrue(IsSubsetOf(set1, set2));

        HashSet<Type> hashSet1 = new HashSet<Type>(dictionary1.Keys);
        HashSet<Type> hashSet2 = new HashSet<Type>(set2);

        hashSet2.IsSubsetOf(hashSet1);
        //Assert.IsFalse(hashSet2.IsSubsetOf(hashSet1));
    }

    public bool IsSubsetOf(Type[] super, Type[] sub)
    {
        int i, j;
        bool isSubset = true;
        for (i = 0; i < sub.Length; i++)
        {
            for (j = 0; j < super.Length; j++)
            {
                if (super[j] == sub[i])
                {
                    break;
                }
                
                if (j == super.Length - 1)
                {
                    isSubset = false;
                    break;
                }
            }

            if (!isSubset) break;
        }

        return isSubset;
    }
}
