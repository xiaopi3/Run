using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    public float speed;
    public int flag = 0;
    public bool onGround,isAlive,isStrike;

    Rigidbody rig;
    Renderer render;
    Animator animator; 
    Material PinkGrid, YellowGrid;
    HUD hud;
    AudioSource aus;
    Camera camera;

    public AudioClip jp, cr,ld;
    public ParticleSystem dieParticle, comParticle,runParticle;
    
    private void Awake()
    {
        PinkGrid = GameObject.Find("build1").GetComponent<Renderer>().material;
        YellowGrid = GameObject.Find("build4").GetComponent<Renderer>().material;

        hud = FindObjectOfType<HUD>();
        aus = GameObject.Find("Camera").GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        camera = GetComponentInChildren<Camera>();
        animator = GetComponent<Animator>();
        dieParticle.Stop();
        comParticle.Stop();
        
    }
    void Start()
    {
        speed = 8f;
        isAlive = true;
        isStrike = false;
        render.material = PinkGrid;
        

    }
    public void Run()
    {
        if (!isAlive) return;
        if (isStrike)
        {
            //print("进入卡住检测");
            Vector3 vel3 = rig.velocity;
            vel3.z = 0;
            rig.velocity = vel3; ;
            return;
        }
        Vector3 vel2 = rig.velocity;
        vel2.z = speed;
        rig.velocity = vel2;
    }
    public void Jump(float force)
    {
        AudioSource.PlayClipAtPoint(ld, camera.transform.position);
        if (!isAlive) return;
        Vector3 vel = rig.velocity;
        vel.y = 0;
        rig.velocity = vel;
        //print(rig.velocity);
        rig.AddForce(Vector3.up * force, ForceMode.Impulse);
        
    }
    public void ChangeColor()
    {
        if (!isAlive) return;
        if (flag == 0)
        {
            render.material = YellowGrid;
            flag = 1;
        }
        else
        {
            render.material = PinkGrid;
            flag = 0;
        }
        animator.SetTrigger("ChangeColor");
    }
    bool GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.4f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                animator.SetBool("isTwoJump", false);
                return true;
            }
                
        }
        return false;
    }
    void GroundUpCheck()
    {
        if (transform.position.y < -15)
        {
            Die();
        }
    }
    public void Die()
    {
        aus.enabled = false;
        runParticle.Stop();
        isAlive = false;
        render.enabled = false;
        rig.velocity = Vector3.zero;
        dieParticle.Play();
        Invoke("Restart", 3);
    }
    public void complete()
    {
        //aus.enabled = false;
        runParticle.Stop();
        isAlive = false;
        render.enabled = false;
        rig.velocity = Vector3.zero;
        comParticle.Play();
        hud.showGameOverPanel();
    }
    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Run");
    }
    void FixedUpdate()
    {
        if (!isAlive) return;
        GroundUpCheck();
        onGround = GroundCheck();
        animator.SetBool("onGround", onGround);
        
    }
}
