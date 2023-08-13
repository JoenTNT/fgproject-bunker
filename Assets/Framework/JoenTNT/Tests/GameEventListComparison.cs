using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class GameEventListComparison
{
    private class SampleClassForSubscribeEvent
    {
        public void SampleMethod() { }
    }

    private UnityEvent _ev = new UnityEvent();
    private List<Action> _evList = new List<Action>();

    private SampleClassForSubscribeEvent[] _samples = null;

    [SetUp]
    public void InitObjectList()
    {
        _samples = new SampleClassForSubscribeEvent[1000];

        for (int i = 0; i < _samples.Length; i++)
            _samples[i] = new SampleClassForSubscribeEvent();
    }

    [Test]
    public void GameEventListComparisonSimplePasses()
    {
        TestUnityEvent();
        TestListEvent();
    }

    [TearDown]
    public void DestroyObjectList()
    {
        _samples = null;
    }

    private void TestUnityEvent()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            _ev.AddListener(_samples[i].SampleMethod);
        }

        RunGameEvent();

        for (int i = 0; i < _samples.Length; i++)
        {
            _ev.RemoveListener(_samples[i].SampleMethod);
        }
    }

    private void TestListEvent()
    {
        for (int i = 0; i < _samples.Length; i++)
        {
            _evList.Add(_samples[i].SampleMethod);
        }

        RunGameEventList();

        for (int i = 0; i < _samples.Length; i++)
        {
            _evList.Remove(_samples[i].SampleMethod);
        }
    }

    private void RunGameEvent() => _ev.Invoke();

    private void RunGameEventList()
    {
        for (int i = 0; i < _samples.Length; i++)
            _evList[i]();
    }
}
