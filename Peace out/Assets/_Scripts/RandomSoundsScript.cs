using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsScript : MonoBehaviour
{
    // Start is called before the first frame update

    new public AudioClip audio;
    bool hasplayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!hasplayed) { 
                AudioSource.PlayClipAtPoint(audio, transform.position);
                hasplayed = true;
            }
            
            
        }
    }


}
