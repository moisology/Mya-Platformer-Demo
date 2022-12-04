using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherSpawn : MonoBehaviour
{
    bool dead, attacking;
    public Rigidbody rb;
    public Transform player;
    public GameObject ms, explosive;
    public float health;
    float distance, attackTimer;
    string[] anims = { "Armature|idle","Armature|attack","Armature|die" };
    // Start is called before the first frame update
    void Start()
    {
        dead = attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance <= 20f && !dead)
        {
            if (!attacking) { PlayAnim(0); }
            if (AttackChance()) { Attack();  }
            TimerProgress();
        }
        if(health <= 0f && !dead) { Die(); }
    }
    void Die()
    {
        PlayAnim(2);
        ms.gameObject.tag = "Dead";
        dead = true;
    }
    void TimerProgress()
    {
        if (attackTimer <= 0) { attacking = false; }
        attackTimer -= 1;
    }
    bool AttackChance() { return Random.Range(1, 200) == 4 && !attacking; }
    void Attack()
    {
        attackTimer = 40f;
        attacking = true;
        PlayAnim(1);
        GameObject tempProjHandle;
        tempProjHandle = Instantiate(explosive, new Vector3(rb.transform.position.x -4f, rb.position.y + 0.25f, rb.position.z), rb.transform.rotation);
        Rigidbody tempProjRb = tempProjHandle.GetComponent<Rigidbody>();
        tempProjRb.AddForce(transform.right * -1000f);
    }
    void Hit(float damage)
    {
        health -= damage;
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
        ms.GetComponent<Animation>().Play(anims[i]);
    }
}
