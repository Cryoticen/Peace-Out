using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Return() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
}
