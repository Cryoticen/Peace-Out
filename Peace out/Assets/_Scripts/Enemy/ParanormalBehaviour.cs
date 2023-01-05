using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParanormalBehaviour : MonoBehaviour
{
    public AudioClip blowOutCandle;
    public AudioClip igniteTorch;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
            if(Random.Range(0,3) == 0) AudioSource.PlayClipAtPoint(blowOutCandle, other.transform.position);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 9) {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(igniteTorch, other.transform.position);
        }
    }
}
