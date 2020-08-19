using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private int frameRange = 60;

    private int[] _fpsBuffer;
    private int _fpsBufferIndex;
    public int AverageFPS { get; private set; }
    public int HighestFPS { get; private set; }
    public int LowestFPS { get; private set; }

  
    void Update()
    {
        if (_fpsBuffer == null || frameRange != _fpsBuffer.Length)
            InitializeBuffer();
        
        UpdateBuffer();
        CalculateFps();
    }

    private void InitializeBuffer()
    {
        if (frameRange <= 0)
            frameRange = 1;

        _fpsBuffer = new int[frameRange];
        _fpsBufferIndex = 0;
    }

    private void UpdateBuffer()
    {
        _fpsBuffer[_fpsBufferIndex++] = (int) (1f / Time.unscaledDeltaTime);
        if (_fpsBufferIndex >= frameRange)
            _fpsBufferIndex = 0;
        
    }

    private void CalculateFps()
    {
        int sum = 0;
        int lowest = int.MaxValue;
        int highest = 0;
        for (int i = 0; i < frameRange; i++)
        {
            int fps = _fpsBuffer[i];
            sum += fps;
            if (fps > highest)
                highest = fps;
            else if (fps < lowest)
                lowest = fps;
        }

        HighestFPS = highest;
        LowestFPS = lowest;
        AverageFPS = sum / frameRange;
    }
}
