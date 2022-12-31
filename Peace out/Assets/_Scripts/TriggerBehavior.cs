using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScrollCounter : MonoBehaviour
{
    int scrollCount = 0;
    int scrollToWin = 1;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI myScore;
    public TextMeshProUGUI peaceOut;
    bool keypressed = false;

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
        if(Input.GetKeyDown(KeyCode.E)){
            keypressed = true;
            
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag == "Scrolls" && keypressed){
                scrollCount++;
                Debug.Log(scrollCount);
                Destroy(other.gameObject);
        }
        if(other.tag == "Finish" && scrollCount >= scrollToWin & keypressed){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        keypressed = false;
    }
}
