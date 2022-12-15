using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public float maxJumpStrength = 2000;
    public float minJumpStrength = 1000;
    public float speed = 3f;
    public bool isGrounded;

    private float jumpStrength;
    private Rigidbody rigid;
    private SphereCollider groundCheck;



    private void Start() {
        rigid = gameObject.GetComponent<Rigidbody>();
        groundCheck = gameObject.GetComponent<SphereCollider>();
        jumpStrength = minJumpStrength;
    }

    private void Update() {
        //Movement on ground
        if (Input.GetAxis("Horizontal") > 0) {
            rigid.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0) {
            rigid.AddForce(-Vector3.right * speed);
        }
        else {

        }

        if (Input.GetAxis("Vertical") > 0) {
            rigid.AddForce(Vector3.forward * speed);
        }
        else if (Input.GetAxis("Vertical") < 0) {
            rigid.AddForce(-Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.Space)) {
            jumpStrength = Mathf.Min(jumpStrength + Time.deltaTime * 2000, maxJumpStrength);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded) {
            rigid.AddForce(Vector3.up * jumpStrength);
            jumpStrength = minJumpStrength;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
    }
}
