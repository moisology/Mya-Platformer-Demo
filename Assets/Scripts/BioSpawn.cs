using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioSpawn : MonoBehaviour
{
    bool isFacingLeft, hit, kicked, standing, attacking;
    public GameObject bs, explosive, psnEffect;
    public Transform player;
    public Rigidbody rb;
    public float moveSpeed, jumpForce, health;
    float standTimer, hitTimer;
    int neg;
    string[] anims = { "metarig|stand", "metarig|hop", "metarig|pounce", "metarig|hit"};
    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = standing = true;
        rb = GetComponent<Rigidbody>();
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (standTimer <= 0f) { Hop(); standTimer = 50f; }
        else { Stand(); }
        TimerProgress();
        if (this.gameObject.tag == "Dead") { Instantiate(psnEffect, rb.transform.position, rb.transform.rotation); }
        if (health <= 0) { this.gameObject.tag = "Dead"; }
    }
    void FlipCheck()
    {
        if (isFacingLeft && player.position.x > transform.position.x) { Flip(); }
        if (!isFacingLeft && player.position.x < transform.position.x) { Flip(); }
    }
    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0, 180.0f, 0);
    }
    void Stand()
    {
        if(Random.Range(1,300) == 4 && !attacking){ Attack(); }
        else{ attacking = false; }
        if (rb.velocity.y == 0f && !hit)
        {
            FlipCheck();
            standing = true;
            rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
            PlayAnim(0);
        }
        else { standing = false; }
    }
    void Attack()
    {
        attacking = true;
        if (isFacingLeft) { neg = -1; }
        else { neg = 1; }
        PlayAnim(3);
        GameObject tempProjHandle;
        tempProjHandle = Instantiate(explosive, new Vector3(rb.transform.position.x + (0f * neg), rb.position.y + 0.25f, rb.position.z), rb.transform.rotation);
        Rigidbody tempProjRb = tempProjHandle.GetComponent<Rigidbody>();
        tempProjRb.AddForce(transform.right * -200f);
        Destroy(tempProjHandle, 2f);
    }
    void Hop()
    {
        attacking = false;
        PlayAnim(1);
        if (isFacingLeft)
        {
            rb.velocity = new Vector3(-moveSpeed, jumpForce, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveSpeed, jumpForce, rb.velocity.z);
        }
    }
    void Hit(float damage)
    {
        PlayAnim(3);
        health -= damage;
        if (isFacingLeft)
        {
            rb.velocity = new Vector3(4f, jumpForce, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(-4f, jumpForce, rb.velocity.z);
        }
    }
    void TimerProgress()
    {
        if (hit || kicked) { hitTimer -= 1; }
        else if (standing) { standTimer -= 1;  }
    }
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Kick": Hit(5f); break;
            case "Bullet": Hit(10f); break;
            case "Laser": Hit(15f); break;
            case "Plasma": Hit(20f); break;
            case "Blaze": Hit(25f); break;
            case "Stomp": Hit(15f); break;
            case "Stab": Hit(5f); break;
        }
    }
    void PlayAnim(int i)
    {
        bs.GetComponent<Animation>().Play(anims[i]);
    }
}
