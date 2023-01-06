using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParanormalBehaviour : MonoBehaviour
{
    public AudioClip igniteTorch;
    public Transform player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
            if (Vector3.Distance(GetComponentInParent<Transform>().position, player.position) < 11) {
                AudioSource.PlayClipAtPoint(igniteTorch, other.transform.position);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 9) {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
