using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HashAsEvent
{
    private class SampleClassForSubscribeEvent
    {
        public void SampleMethod() { }
    }

    private HashSet<Action> _eventMethods = new HashSet<Action>();

    private SampleClassForSubscribeEvent[] _samples = null;
    
    [SetUp]
    public void InitSampleObjects()
    {
        _samples = new SampleClassForSubscribeEvent[2500];

        for (int i = 0; i < _samples.Length; i++)
            _samples[i] = new SampleClassForSubscribeEvent();
    }

    [Test]
    public void HashAsEventSimplePasses()
    {
        SubscribeSamples();
        RunSamples();
        UnsubscribeSamples();
    }

    private void SubscribeSamples()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            _eventMethods.Add(_samples[i].SampleMethod);
        }
    }

    private void RunSamples()
    {
        foreach (var method in _eventMethods)
        {
            //Debug.Log($"Name: {method.Method.Name}; Hash: {method.GetHashCode()}");
            method.Invoke();
        }
    }

    private void UnsubscribeSamples()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            _eventMethods.Remove(_samples[i].SampleMethod);
        }
    }

    [TearDown]
    public void DestroySampleObjects()
    {
        _samples = null;
    }
}
