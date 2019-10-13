using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatelLightIntensity : MonoBehaviour
{

    public float amplitude;
    public float frequency;

    Light light;


    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity += Mathf.Sin(Time.time * frequency) * amplitude;
           
    }
}
