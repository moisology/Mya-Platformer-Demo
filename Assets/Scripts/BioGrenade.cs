using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioGrenade : MonoBehaviour
{
    public GameObject psnEffect, player;
    float distance;
    void Burst()
    {
        Instantiate(psnEffect, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance <= 3f) { Burst(); }
    }

}
