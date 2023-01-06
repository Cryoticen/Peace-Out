using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntingSounds : MonoBehaviour
{
    public float huntingTimer = 0;
    public float hearingTimer = 0;

    public AudioClip huntingSound1;
    public AudioClip huntingSound2;
    public AudioClip huntingSound3;
    public AudioClip huntingSound4;
    public AudioClip hunting_hearingSound;
    public AudioClip hearingSound;

    AudioClip[] huntingAudio;
    AudioClip[] hearingAudio;

    private void Start() {
        huntingAudio = new AudioClip[5];
        huntingAudio[0] = huntingSound1;
        huntingAudio[1] = huntingSound2;
        huntingAudio[2] = huntingSound3;
        huntingAudio[3] = huntingSound4;
        huntingAudio[4] = hunting_hearingSound;

        hearingAudio = new AudioClip[2];
        hearingAudio[0] = hunting_hearingSound;
        hearingAudio[1] = hearingSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<NavMeshAgent>().speed >=4 ) {
            if(huntingTimer <= 0) {
                AudioSource.PlayClipAtPoint(huntingAudio[Random.Range(0, 5)], GetComponentInParent<Transform>().position);
                huntingTimer = 4;
            }
            huntingTimer -= Time.deltaTime;
        }

        if (GetComponentInParent<GhoulBehaviour>().canHearPlayer) {
            if (hearingTimer <= 0) {
                AudioSource.PlayClipAtPoint(hearingAudio[Random.Range(0, 2)], GetComponentInParent<Transform>().position);
                hearingTimer = 30;
            }
        }
        hearingTimer -= Time.deltaTime;
    }
}
