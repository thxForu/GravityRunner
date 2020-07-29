using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private ParticleSystem ps;
    public static float hSliderValue = 20.0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.rateOverTime = hSliderValue;
    }

    void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 45, 100, 30), hSliderValue, 5.0f, 200.0f);
    }
}