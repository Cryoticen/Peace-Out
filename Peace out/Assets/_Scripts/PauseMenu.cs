using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject inGameUi;
    public FirstPersonLook firstPersonLook;
    // Update is called once per frame
    void Start(){
        this.firstPersonLook.mouseLookEnabled = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(gameIsPaused){
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        inGameUi.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        this.firstPersonLook.mouseLookEnabled = true;
         Cursor.visible = false;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        inGameUi.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        this.firstPersonLook.mouseLookEnabled = false;
         Cursor.visible = true;
    }

    public void Quit(){
        Debug.Log("quit");
        Application.Quit();
    }
}
