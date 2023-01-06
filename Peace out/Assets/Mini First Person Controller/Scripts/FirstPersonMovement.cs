using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed;
    public float maxStamina = 7;
    public float maxExhaustionTimer = 7;
    public bool isExhausted = false;
    public GameObject exhaustedText;

    [Header("Running")] public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 4.7f;
    public KeyCode runningKey = KeyCode.LeftShift;


    private new Rigidbody rigidbody;
    public float stamina = 7;
    public float exhaustionTimer = 5;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        IsRunning = canRun && Input.GetKey(runningKey);
        if (GetComponentInParent<ScrollCounter>().scrollCount >= GetComponentInParent<ScrollCounter>().scrollToWin) stamina = 100;

        // Get targetMovingSpeed.
        float targetMovingSpeed;
        if (IsRunning && (!isExhausted)) {
            targetMovingSpeed = runSpeed;
            if(!Input.GetKey(KeyCode.LeftControl)) stamina -= Time.fixedDeltaTime;
        }
        else {
            targetMovingSpeed = speed;
            if (!isExhausted && stamina < maxStamina) {
                stamina += Time.fixedDeltaTime;
            }
        }

        if (stamina <= 0 && exhaustionTimer > 0 ) {
            isExhausted = true;
            exhaustionTimer -= Time.fixedDeltaTime;
        }
        else if (exhaustionTimer <= 0) {
            isExhausted = false;
            exhaustionTimer = maxExhaustionTimer;
            stamina = maxStamina;
        }

        if (isExhausted) {
            targetMovingSpeed = speed - .7f;
            exhaustedText.SetActive(true);
        }
        else {
            exhaustedText.SetActive(false);
        }

        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);

        if(!Input.anyKey && rigidbody.velocity.x < 0.5 && rigidbody.velocity.z < 0.5){
            rigidbody.velocity =new Vector3(0, rigidbody.velocity.y, 0);
        }
    }
}