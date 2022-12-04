using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController123 : MonoBehaviour
{
    bool isFacingLeft, standing, running, jumping, crouching, attacking, hit, dead;
    public bool bladeCannonCtrl, padsCtrl, bulletCtrl, laserCtrl, plasmaCtrl, blazeCrtl;
    public Rigidbody rb;
    public float moveSpeed, jumpForce, health;
    public GameObject player, kick, bladeCannon, pad1, pad2, bullet, laser, plasma, blaze;
    public HealthBar healthBar;
    float t = 0f, neg;
    string[] anims = { "metarig|stand", "metarig|crouch", "metarig|jump","metarig|run", "metarig|hit", "metarig|shoot",  "metarig|stab", "metarig|stomp", "metarig|kick" };
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMax(100f);
        isFacingLeft = standing = true;
        running = jumping = attacking = crouching = hit = dead = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        if(rb.transform.position.z != 0f) { rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, 0f); }
        if (!hit)
        {
            DefaultChecks();
            if (Input.GetKeyDown("up")) { JumpCheck(); }
            if (Input.GetKey("left") || Input.GetKey("right")) { Move(); }
            if (Input.GetKeyUp("left") || Input.GetKeyUp("right")) { rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z); }
            if (standing && Input.GetKey("down")) { rb.velocity = new Vector3(0f, 0f, rb.velocity.z); CrouchCheck(); }
            if (Input.GetKeyUp("down")) { crouching = false; }
            if (Input.GetKeyDown("space")) { AttackCheck(); }
        }
        ActionProgress(); }
    void FixedUpdate()
    {
        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));
        rb.MovePosition(transform.position + m_Input * Time.deltaTime * moveSpeed);
        if (!hit)
        {
            DefaultChecks();
            if (Input.GetKeyDown("up")) { JumpCheck(); }
            if (Input.GetKey("left") || Input.GetKey("right")) { Move(); }
            if (Input.GetKeyUp("left") || Input.GetKeyUp("right")) { rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z); }
            if (standing && Input.GetKey("down")) { rb.velocity = new Vector3(0f, 0f, rb.velocity.z); CrouchCheck(); }
            if (Input.GetKeyUp("down")) { crouching = false; }
            if(Input.GetKeyDown("space")) { AttackCheck(); }
        }
    }
    void DefaultChecks()
    {
        StateCheck();
        StandingOrInAir();
        ItemsCheck();
        HealthCheck();
    }
    void HealthCheck()
    {
        healthBar.SetHealth(health);
        if (health > 100f) { health = 100f; }
        else if (health <= 0) { health = 0; dead = true; }
        else if (!dead) { health += 0.0025f; }
        if (dead) { SceneManager.LoadScene("GameOver"); }
    }
    void ItemsCheck()
    {
        if (bladeCannonCtrl) { bladeCannon.active = true; }
        else { bladeCannon.active = false; }
        if (padsCtrl) { pad1.active = true; pad2.active = true; }
        else { pad1.active = false; pad2.active = false; }
        if (bulletCtrl) { bullet.active = true; laser.active = false; plasma.active = false; blaze.active = false; }
        if (laserCtrl) { laser.active = true; bullet.active = false; plasma.active = false; blaze.active = false; }
        if (plasmaCtrl) { plasma.active = true; bullet.active = false; laser.active = false; blaze.active = false; }
        if (blazeCrtl) { blaze.active = true; plasma.active = false; bullet.active = false; laser.active = false; }
    }
    void StateCheck()
    {
        if(rb.velocity.x == 0f && rb.velocity.y == 0f && !(Input.GetKey("down"))) { standing = true; running = jumping = crouching = false; }
        if(rb.velocity.x == 0f && rb.velocity.y == 0f && Input.GetKey("down")) { crouching = standing = true; running = jumping = false; }
        if(rb.velocity.x != 0 && rb.velocity.y == 0f) { running = true; standing = crouching = jumping = false; }
        if(rb.velocity.y != 0) { jumping = true; standing = crouching = false; }
    }
    void StandingOrInAir(){ StandCheck(); InAirCheck(); }
    void StandCheck()
    {
        if (standing && !crouching && !attacking) { PlayAnim(0); }
    }
    void CrouchCheck()
    {
        if (crouching && !attacking){ PlayAnim(1); rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z); }
    }
    void Move()
    {
        FlipCheck();
        if (t <= 0 && !jumping && !attacking && running)
        {
            PlayAnim(3);
            t = 4f;
        }
        if (Input.GetKey("right") && !crouching)
        {
                rb.velocity = new Vector3(moveSpeed, rb.velocity.y, rb.velocity.z);
        }
        if (Input.GetKey("left") && !crouching)
        {
                rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, rb.velocity.z);
        }
    }
    void FlipCheck(){
        if (!isFacingLeft && Input.GetKey("left")) { Flip(); }
        if (isFacingLeft && Input.GetKey("right")) { Flip(); }
    }
    void Flip(){
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0, 180.0f, 0);
    }
    
    void JumpCheck(){
        if (!jumping){
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
    void InAirCheck(){
        if(jumping && !attacking && !hit){ PlayAnim(2); }
    }
    void AttackCheck(){
        if (standing && !attacking && bladeCannonCtrl && !crouching) { Attack(1); }
        if (running && !attacking && !jumping && bladeCannonCtrl) { Attack(2); }
        if (jumping && !attacking && padsCtrl) { Attack(3); }
        if (crouching && !attacking) { Attack(4); }
    }
    GameObject chooseBeam()
    {
        if (laserCtrl) { return laser; }
        else if (plasmaCtrl) { return plasma; }
        else if (blazeCrtl) { return blaze; }
        else { return bullet; }
    }
    void Attack(int i){
        attacking = true;
        if (isFacingLeft) { neg = -1; }
        else { neg = 1; }
        switch (i)
        {
            case 1:
                t = 10f;
                GameObject tempProjHandle;
                tempProjHandle = Instantiate(chooseBeam(), new Vector3(rb.transform.position.x + (0.05f * neg), rb.position.y + 0.1f, rb.position.z), rb.transform.rotation);
                rb.velocity = new Vector3(rb.velocity.x, 0.5f, rb.velocity.z);
                PlayAnim(5);
                Rigidbody tempProjRb = tempProjHandle.GetComponent<Rigidbody>();
                tempProjRb.AddForce(transform.right * -1000f);
                Destroy(tempProjHandle, 1f);
                break;
            case 2:
                t = 25f;
                gameObject.tag = "Stab";
                PlayAnim(6);
                break;
            case 3:
                t = 25f;
                gameObject.tag = "Stomp";
                rb.velocity = new Vector3(rb.velocity.x, -jumpForce, rb.velocity.z);
                PlayAnim(7);
                break;
            case 4:
                t = 50f;
                GameObject tempKickHandle;
                tempKickHandle = Instantiate(kick, new Vector3(rb.transform.position.x + (1f * neg), rb.position.y + 0.1f, rb.position.z), rb.transform.rotation) as GameObject;
                rb.velocity = new Vector3(rb.velocity.x, 1f, rb.velocity.z);
                PlayAnim(8);
                standing = true;
                Destroy(tempKickHandle, 0.25f);
                break;
        }
    }
    void CancelActions(){
        gameObject.tag = "Player";
        attacking = hit = false;
    }
    void ActionProgress(){
        if (t <= 0){ CancelActions(); }
        t -= 1.25f;
    }
    void Hit(float damage)
    {
        if (!hit) {
            hit = true;
            PlayAnim(4);
            t = 20f;
        }
        health -= damage;
    }
    void OnCollisionEnter(Collision other)
    {
        if(!hit)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy": if (gameObject.tag == "Player") { Hit(5); } break;
                case "BioSpawnExplosive": if (gameObject.tag == "Player") { Hit(15); } break;
                case "Enemy Plasma": if(gameObject.tag == "Player") { Hit(5); } break;
            }
        }
    }
    void PlayAnim(int i)
    {
        player.GetComponent<Animation>().Play(anims[i]);
    }
}
