using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class flickering : MonoBehaviour
{
    public new Light light;
    public float fluctiation;
    public float flucSpeed;
    public float initialIntensity;
    public float timer;
    public bool isSteady;


    void Start()
    {
        if (light == null)
        {
            light = gameObject.GetComponent<Light>();
        }
        initialIntensity = light.intensity;
        timer = Random.Range(1,4);
        isSteady = true;

        fluctiation = .2f;
        flucSpeed = 2;
    }

    // Update is called once per frame
    void Update()
    {

        if (isSteady)
        {
            light.intensity = initialIntensity;
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                isSteady = false;
                timer = - Random.Range(1, 3);
            }
        }
        else
        {
            light.intensity = initialIntensity - Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * Random.Range(1, 2)) * fluctiation);
            if (timer < 0)
            {
                timer += Time.deltaTime;
            }
            else
            {
                isSteady = true;
                timer = Random.Range(1, 4);
            }
        }
    }
}
