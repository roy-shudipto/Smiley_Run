using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour 
{

    public GameObject startUI;
    public GameObject endUI;
    public GameObject player;
   
	void Update () 
    {
        if(PlayerController.instance.dead)
        {
            startUI.SetActive(true);
        }
	}

    public void StartPlaying()
    {
        PlayerController.instance.Reset();
        startUI.SetActive(false);
    }
}
