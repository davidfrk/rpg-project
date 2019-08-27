using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLight : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 1f;

    new Light light;
    private float baseIntensity;

    void Awake()
    {
        light = GetComponent<Light>();
    }

    void Start()
    {
        baseIntensity = light.intensity;
    }

    void Update()
    {
        light.intensity = baseIntensity + amplitude * Mathf.Sin(frequency * Time.time);
    }
}
