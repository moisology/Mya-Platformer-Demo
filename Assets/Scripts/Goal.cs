using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    bool open;
    public GameObject boss, player, closedMsg, openMsg;
    // Start is called before the first frame update
    void Start()
    {
        openMsg.gameObject.active = false;
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss.gameObject.tag == "Dead") 
        {
            open = openMsg.gameObject.active = true;
            closedMsg.gameObject.active = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == player && open)
        {
            SceneManager.LoadScene("DemoComplete");
        }
    }
}
