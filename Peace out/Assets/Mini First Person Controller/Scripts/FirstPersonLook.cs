using UnityEngine;
using UnityEngine.AI;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    [SerializeField] Component referencedScript;

    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public bool mouseLookEnabled = false;
    public float maxDistanceFromHead = .03f;
    public float leaningSpeed = .3f;
    public SphereCollider headCollider;
    public bool headIsColliding = false;

    Vector2 velocity;
    Vector2 frameVelocity;

    private float max_fov;
    private float min_fov;
    private float heightFromParent;
    private bool headIsBlocked = false;
    private float yDistanceFromHead;

    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
        headCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        heightFromParent = transform.position.y - transform.parent.transform.position.y;
    }

    void Update(){
        lean();

        if (this.mouseLookEnabled) {
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
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (GetComponentInParent<FirstPersonMovement>().IsRunning  && !GetComponentInParent<FirstPersonMovement>().isExhausted && !Input.GetKey(KeyCode.LeftControl)) {
            if (Camera.main.fieldOfView <= 67) Camera.main.fieldOfView += Time.deltaTime * 35;
        }
        else {
            if (Camera.main.fieldOfView >= 60) Camera.main.fieldOfView -= Time.deltaTime * 50;
        }

        if (headIsColliding) {
            Camera.main.nearClipPlane = 0.001f;
        }
        else {
            Camera.main.nearClipPlane = 0.1f;
        }
    }

    private void lean() {
        Vector3 headPosition = transform.parent.transform.position + new Vector3(0, heightFromParent, 0);
        yDistanceFromHead = headPosition.y - transform.position.y;

        if (Input.GetKey(KeyCode.Q) && getAbleToLean()) {
            transform.RotateAround(transform.parent.transform.position, transform.forward, leaningSpeed);
            GetComponentInParent<FirstPersonMovement>().isLeaning = true;
        }
        else if (Input.GetKey(KeyCode.E) && getAbleToLean()) {
            transform.RotateAround(transform.parent.transform.position, -transform.forward, leaningSpeed);
            GetComponentInParent<FirstPersonMovement>().isLeaning = true;
        }
        else if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)) {
            transform.position = transform.parent.transform.position + new Vector3(0, this.heightFromParent, 0);
            GetComponentInParent<FirstPersonMovement>().isLeaning =  false;
        }
    }

    private void OnTriggerEnter(Collider other){
        this.headIsBlocked = true;
        headIsColliding = true;
    }
    private void OnTriggerExit(Collider other){
        this.headIsBlocked = false;
        headIsColliding = false;
    }

    private bool getAbleToLean(){
        return yDistanceFromHead <= maxDistanceFromHead && !headIsBlocked;
    }
}
