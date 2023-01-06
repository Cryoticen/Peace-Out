using UnityEngine;
using UnityEngine.AI;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public bool mouseLookEnabled = false;


    Vector2 velocity;
    Vector2 frameVelocity;

    private float max_fov;
    private float min_fov;
    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(this.mouseLookEnabled){
             // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        Cursor.lockState = CursorLockMode.Locked;
        }else{
            Cursor.lockState = CursorLockMode.None;
        }

        if (GetComponentInParent<FirstPersonMovement>().IsRunning && !GetComponentInParent<FirstPersonMovement>().isExhausted && !Input.GetKey(KeyCode.LeftControl)) {
            if (Camera.main.fieldOfView <= 67) Camera.main.fieldOfView += Time.deltaTime * 35;
        }
        else {
            if (Camera.main.fieldOfView >= 60) Camera.main.fieldOfView -= Time.deltaTime * 50;
        }
    }
}
