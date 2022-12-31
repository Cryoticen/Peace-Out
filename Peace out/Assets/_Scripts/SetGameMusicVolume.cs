using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetGameMusicVolume : MonoBehaviour
{
   public AudioMixer mixer;

   public void setLevel(float sliderValue){
    mixer.SetFloat("GameMusicVol",Mathf.Log10(sliderValue) * 20);
   }
}
