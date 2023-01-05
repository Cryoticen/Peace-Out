using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScrollCounter : MonoBehaviour
{
    int scrollCount = 0;
    public int scrollToWin = 1;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI myScore;
    public TextMeshProUGUI peaceOut;
    public AudioClip collectSound;

    void Start(){ 
        ScoreText.text = "Scrolls:";
        myScore.text = scrollCount.ToString();
        peaceOut.enabled = false;
        peaceOut.text = "It's time to Peace Out of here!";
    }

    void Update(){
        myScore.text = scrollCount.ToString();
        if(scrollCount == scrollToWin){
            peaceOut.enabled = true;
            ScoreText.enabled = false;
            myScore.enabled = false;
        }
    }

    void OnTriggerStay(Collider other){
        if (other.tag == "Scrolls" && Input.GetKeyDown(KeyCode.E)) {
                scrollCount++;
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
                Destroy(other.gameObject);
        }
        if(other.tag == "Finish" && scrollCount >= scrollToWin & Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
