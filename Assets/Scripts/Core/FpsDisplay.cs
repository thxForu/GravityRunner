using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FpsCounter))]
public class FpsDisplay : MonoBehaviour
{
    [SerializeField] private Text averageLabel;
    [SerializeField] private Text highestLabel;
    [SerializeField] private Text lowestLabel;

    private FpsCounter _fpsCounter;

    private void Awake()
    {
        _fpsCounter = GetComponent<FpsCounter>();
    }

    private void Update()
    {
        averageLabel.text = Mathf.Clamp(_fpsCounter.AverageFPS,0,60).ToString();
        highestLabel.text = Mathf.Clamp(_fpsCounter.HighestFPS,0,60).ToString();
        lowestLabel.text = Mathf.Clamp(_fpsCounter.LowestFPS,0,60).ToString();

    }
}
