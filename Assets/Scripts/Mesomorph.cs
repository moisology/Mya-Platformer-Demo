using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesomorph : MonoBehaviour
{
    bool isFacingLeft, hit, standing, attacking;
    public GameObject mm, explosive, psnEffect;
    public Transform player;
    public Rigidbody rb;
    public float moveSpeed, jumpForce, health;
    float standTimer, hitTimer, distance;
    int neg;
    string[] anims = { "metarig|idle", "metarig|run", "metarig|jump", "metarig|shoot", "metarig|hit"};
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
        FlipCheck();
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if(distance <= 5f)
        {
            if (standing && standTimer <= 0f && AttackChance()) { Attack(); standTimer = 10f; }
            else { Stand(); }
        }
        else{ Run(); }
        if(IsLower() && !Jumping() && AttackChance() ){ Jump(); }
        if (health <= 0) {
            Instantiate(psnEffect, rb.transform.position, rb.transform.rotation);
            Destroy(gameObject);
        }
        TimerProgress();
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
        attacking = false;
        if (!Jumping() && !hit)
        {
            standing = true;
            rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
            PlayAnim(0);
        }
        else { standing = false; }
    }
    bool AttackChance() { return Random.Range(1, 25) == 4 && !attacking; }
    void Attack()
    {
        attacking = true;
        if (isFacingLeft) { neg = -1; }
        else { neg = 1; }
        PlayAnim(3);
        GameObject tempProjHandle;
        tempProjHandle = Instantiate(explosive, new Vector3(rb.transform.position.x + (0.75f * neg), rb.position.y - 0.3f, rb.position.z), rb.transform.rotation);
        Rigidbody tempProjRb = tempProjHandle.GetComponent<Rigidbody>();
        tempProjRb.AddForce(transform.right * -450f);
        Destroy(tempProjHandle, 2f);
    }
    void Jump()
    {
        attacking = false;
        PlayAnim(2);
        if (isFacingLeft)
        {
            rb.velocity = new Vector3(-moveSpeed, jumpForce, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveSpeed, jumpForce, rb.velocity.z);
        }
    }
    bool Jumping()
    {
        return rb.velocity.y != 0f;
    }
    bool IsLower() { return this.transform.position.y < player.transform.position.y; }
    void Run()
    {
        attacking = false;
        if(!attacking)
        {
            PlayAnim(1);
        }
        
        if (isFacingLeft)
        {
            rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveSpeed, rb.velocity.y, rb.velocity.z);
        }
    }
    void Hit(float damage)
    {
        PlayAnim(4);
        health -= damage;
        if (isFacingLeft)
        {
            rb.velocity = new Vector3(2f, 2f, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(-2f, 2f, rb.velocity.z);
        }
    }
    void TimerProgress()
    {
        if(standTimer <= 0) { attacking = false; }
        if (hit) { hitTimer -= 1; }
        else if (standing) { standTimer -= 1; }
    }
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Kick": Hit(5f); break;
            case "Bullet":Hit(10f); break;
            case "Laser": Hit(15f); break;
            case "Plasma": Hit(20f); break;
            case "Blaze": Hit(25f); break;
            case "Stomp": Hit(15f); break;
            case "Stab": Hit(5f); break;
        }
    }
    void PlayAnim(int i)
    {
        mm.GetComponent<Animation>().Play(anims[i]);
    }
}
