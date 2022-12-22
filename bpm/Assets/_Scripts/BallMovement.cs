using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public float maxJumpStrength = 1000;
    public float minJumpStrength = 500;
    public float step = 350;
    public float speed = 3f;
    public float maxVelocity = 10f;
    public bool isGrounded;

    public Vector3 velocity;

    private float jumpStrength;
    private Rigidbody rigid;
    private SphereCollider groundCheck;



    private void Start() {
        rigid = gameObject.GetComponent<Rigidbody>();
        groundCheck = gameObject.GetComponent<SphereCollider>();
        jumpStrength = minJumpStrength;
        //rigid.drag = 1f;

        velocity = Vector3.zero;

        //Physics.gravity = new Vector3(0,-10.0f,0);
    }

    private void Update() {
        //Movement on ground
        Vector3 horizontalForce = Vector3.zero;
        Vector3 verticalForce = Vector3.zero;
        if (Input.GetAxis("Horizontal") > 0) {
            //rigid.AddForce(Vector3.right * speed);
            horizontalForce = Vector3.right;
        }
        else if (Input.GetAxis("Horizontal") < 0) {
            //rigid.AddForce(-Vector3.right * speed);
            horizontalForce = -Vector3.right;
        }

        if (Input.GetAxis("Vertical") > 0) {
            //rigid.AddForce(Vector3.forward * speed);
            verticalForce = Vector3.forward;
        }
        else if (Input.GetAxis("Vertical") < 0) {
            //rigid.AddForce(-Vector3.forward * speed);
            verticalForce = -Vector3.forward;
        }

        if (Mathf.Abs(rigid.velocity.x) > maxVelocity) horizontalForce = Vector3.zero;
        if (Mathf.Abs(rigid.velocity.z) > maxVelocity) verticalForce = Vector3.zero;

        rigid.AddForce(Vector3.Normalize(horizontalForce + verticalForce) * speed);


        if (Input.GetKey(KeyCode.Space)) {
            jumpStrength = Mathf.Min(jumpStrength + Time.deltaTime * step, maxJumpStrength);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded){
            rigid.AddForce(Vector3.up * jumpStrength);
            jumpStrength = minJumpStrength;
        }

        if (!isGrounded) rigid.AddForce(Vector3.down * 2);

        if (!Input.anyKey) {
            rigid.velocity = new Vector3(rigid.velocity.x * .99f, rigid.velocity.y , rigid.velocity.z * .99f);
            if (Mathf.Abs(rigid.velocity.x) < 0.01f) rigid.velocity = new Vector3(0, rigid.velocity.y, rigid.velocity.z);
            if (Mathf.Abs(rigid.velocity.z) < 0.01f) rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, 0);
        }
        velocity = rigid.velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
    }
}
