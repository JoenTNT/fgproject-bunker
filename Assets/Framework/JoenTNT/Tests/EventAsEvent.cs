using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EventAsEvent
{
    private class SampleClassForSubscribeEvent
    {
        public void SampleMethod() { }
    }

    private event Action e;
    private SampleClassForSubscribeEvent[] _samples = null;
    
    [SetUp]
    public void InitSampleObjects()
    {
        _samples = new SampleClassForSubscribeEvent[2500];

        for (int i = 0; i < _samples.Length; i++)
            _samples[i] = new SampleClassForSubscribeEvent();
    }

    [Test]
    public void EventAsEventSimplePasses()
    {
        SubscribeSamples();
        RunSamples();
        UnsubscribeSamples();
    }

    private void SubscribeSamples()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            e += _samples[i].SampleMethod;
        }
    }

    private void RunSamples()
    {
        e?.Invoke();
    }

    private void UnsubscribeSamples()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            e -= _samples[i].SampleMethod;
        }
    }

    [TearDown]
    public void DestroySampleObjects()
    {
        _samples = null;
    }
}
