using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioGrenadeController : MonoBehaviour
{
    public GameObject spawn, player;
    float distance;
    Collider c;
    //Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        c = this.GetComponent<Collider>();
        //rb = this.GetComponent<Rigidbody>();
        spawn.active = false;
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance <= 3f) { spawn.active = true; c.enabled = false; GetComponent<Rigidbody>().isKinematic = true; }
        if (spawn.gameObject.tag == "Dead") { Destroy(gameObject); }
    }
}
