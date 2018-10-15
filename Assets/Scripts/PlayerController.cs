using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    int jumpCount;


    Renderer render;
    PlayerCharacter character;
    Collision collisionR;
    Animator animator;
    HUD hud;
    Rigidbody rig;
    Camera camera;
    void Awake()
    {
        character = GetComponent<PlayerCharacter>();
        render = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        hud = FindObjectOfType<HUD>();
        rig = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
    }
	// Use this for initialization
	void Start () {
        jumpCount = 0;

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!character.isAlive) return;
        character.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.ChangeColor();
        }
	}
    void FixedUpdate()
    {
        //print("固定检测");
        if (!character.isAlive) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (jumpCount == 0)
            {
                character.Jump(8);
                jumpCount++;
                animator.SetBool("isTwoJump", false);
            }
            else if (jumpCount == 1)
            {
                character.Jump(8);
                jumpCount++;
                animator.SetBool("isTwoJump", true);
            }
        }
        if (collisionR != null)
        {
            
            if (collisionR.gameObject.CompareTag("red"))
            {
                if (character.flag == 1)
                    character.Die();
            }
            else if (collisionR.gameObject.CompareTag("yellow"))
            {
                if (character.flag == 0)
                    character.Die();
            }
            else
            {
                character.Die();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //print("进入碰撞");
        jumpCount=0;
        if (rig.velocity.z == 0 && character.isAlive)
        {
            AudioSource.PlayClipAtPoint(character.cr, camera.transform.position);
            //rig.AddForce(Vector3.down * 20, ForceMode.Acceleration);

            animator.SetBool("isTwoJump", true);
        }
        else
            AudioSource.PlayClipAtPoint(character.ld, camera.transform.position);
        collisionR = collision;       
    }
    private void OnCollisionStay(Collision collision)
    {
        if (rig.velocity.z < 1 && character.isAlive)
        {
            character.isStrike = true;
            //print("卡住了");
        }
        //print(rig.velocity.z+"  "+character.isAlive);
        collisionR = collision;
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionR = null;
        character.isStrike = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Finish") 
        {
            character.complete();
        }
    }
}
