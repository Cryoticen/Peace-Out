using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickering : MonoBehaviour
{
    Light myLight;
    public float maxInterval = 1f;
    public float maxIntensity = 2f;
    public float minIntensity = 1.9f;

    float targetIntensity;
    float lastIntensity;
    float interval;
    float timer;
    public float maxDisplacement = 0.007f;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        origin = transform.position;
        lastPosition = origin;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            lastIntensity = myLight.intensity;
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0;
            interval = Random.Range(0, maxInterval);
            targetPosition = origin + Random.insideUnitSphere * maxDisplacement;
            lastPosition = myLight.transform.position;
        }
        myLight.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / interval);
        myLight.transform.position = Vector3.Lerp(lastPosition, targetPosition, timer / interval);
    }
}