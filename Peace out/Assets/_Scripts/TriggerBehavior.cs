using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScrollCounter : MonoBehaviour
{
    public int scrollCount = 0;
    public int scrollToWin = 20;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI myScore;
    public TextMeshProUGUI peaceOut;
    public AudioClip collectSound;

    private float timer = 0;
    private bool pressedE;
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
        if (Input.GetKeyDown(KeyCode.E) && timer <= 0) {
            pressedE = true;
            timer = .08f;
        }
        else if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else if(timer <= 0) {
            pressedE = false;
        }
    }

    void OnTriggerStay(Collider other){
        if (other.tag == "Scrolls" && pressedE) {
            scrollCount++;
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(other.gameObject);
            pressedE = false;
        }
        if(other.tag == "Finish" && scrollCount >= scrollToWin && pressedE) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
