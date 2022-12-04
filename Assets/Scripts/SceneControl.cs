using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public int index;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            switch (index)
            {
                case 0:
                    SceneManager.LoadScene("TitleScreen");
                    break;
                case 1:
                    SceneManager.LoadScene("Game");
                    break;
            }
        }
    }
}
