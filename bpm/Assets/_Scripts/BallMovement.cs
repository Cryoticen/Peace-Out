using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public float maxJumpStrength = 2000;
    public float minJumpStrength = 1000;
    public float step = 1000;
    public float speed = 15f;
    public float airDrag = 0.12f;
    public float maxVelocity = 7f;
    public bool isGrounded;

    public Vector3 velocity;

    private float jumpStrength;
    private Rigidbody rigid;
    private SphereCollider groundCheck;



    private void Start() {
        rigid = gameObject.GetComponent<Rigidbody>();
        groundCheck = gameObject.GetComponent<SphereCollider>();
        jumpStrength = minJumpStrength;
        rigid.drag = 0.2f;
        rigid.mass = 3f;
        isGrounded = false;
        velocity = Vector3.zero;

        //Physics.gravity = new Vector3(0,-10.0f,0);
    }

    private void Update() {
        velocity = rigid.velocity;
        print(rigid.velocity.magnitude);

        //Movement on ground
        Vector3 horizontalForce = Vector3.zero;
        Vector3 verticalForce = Vector3.zero;

        if (Input.GetKey(KeyCode.D)) {
            horizontalForce += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            horizontalForce += Vector3.left;
        }

        if (Input.GetKey(KeyCode.W)) {
            verticalForce += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            verticalForce += Vector3.back;
        }

        if (Mathf.Abs(rigid.velocity.x) > maxVelocity) horizontalForce = Vector3.zero;
        if (Mathf.Abs(rigid.velocity.z) > maxVelocity) verticalForce = Vector3.zero;

        if (isGrounded) {
            rigid.AddForce(Vector3.Normalize(horizontalForce + verticalForce) * speed);

            if (!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D)) {
                rigid.velocity = new Vector3(rigid.velocity.x * .99f, rigid.velocity.y, rigid.velocity.z);
                if (Mathf.Abs(rigid.velocity.x) < 0.01f) rigid.velocity = new Vector3(0, rigid.velocity.y, rigid.velocity.z);
            }
            if (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S)) {
                rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, rigid.velocity.z * .99f);
                if (Mathf.Abs(rigid.velocity.z) < 0.01f) rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, 0);
            }
        }
        else {
            rigid.AddForce(Vector3.Normalize(horizontalForce + verticalForce) * speed * airDrag);
            rigid.AddForce(Vector3.down * 2.5f);
        }


        if (Input.GetKey(KeyCode.Space)) {
            jumpStrength = Mathf.Min(jumpStrength + Time.deltaTime * step, maxJumpStrength);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded){
            rigid.AddForce(Vector3.up * jumpStrength);
            jumpStrength = minJumpStrength;
        }

    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
        if (velocity.y < -8.5)
            rigid.AddForce(Vector3.up * velocity.magnitude * 60);
    }

    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
    }
}
