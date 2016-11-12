﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Animator myAnim;
    public float moveSpeed;
    public float jumpSpeed;
    private Rigidbody2D myRigidbody;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public Vector3 respawnPosition;
    public LevelManager theLevelManager;
    public GameObject stopmBox;
    public float knokbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    public float invinicibilityLength;
    private float invinicibilityCounter;

	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
    }
	
	void Update () {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (knockbackCounter <= 0)
        {

            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
            }
            theLevelManager.invincible = false;
        }
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            if (transform.localScale.x > 0)
            {
                myRigidbody.velocity = new Vector3(-knokbackForce, knokbackForce, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(knokbackForce, knokbackForce, 0f);
            }
        }
        if (invinicibilityCounter > 0) {
            invinicibilityCounter -= Time.deltaTime;
        }
        if (invinicibilityCounter <= 0) {
            theLevelManager.invincible = false;
        }
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
        myAnim.SetBool("Grounded",isGrounded);
        if (myRigidbody.velocity.y < 0)
        {
            stopmBox.SetActive(true);
        }
        else {
            stopmBox.SetActive(false);
        }

	}

    public void Knockback() {
        knockbackCounter = knockbackLength;
        invinicibilityCounter = invinicibilityLength;
        theLevelManager.invincible = true;
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "KillPlane") {
            theLevelManager.Respawn();
        }
        if (other.tag == "Checkpoint") {
            respawnPosition = other.transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "MovingPlatform") {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "MovingPlatform") {
            transform.parent = null;
        }
    }

}