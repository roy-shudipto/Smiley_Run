using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour 
{
    private static bool skipUI = false;
    private static bool created = false;
    public static int unlockedLevel = 1;
    public GameObject startUI;
    public GameObject endUI;
    public GameObject player;
    public GameObject playButton;
    public GameObject level1Button, level2Button, level3Button,
                      level4Button, level5Button, level6Button,
                      level7Button, level8Button, level9Button;

    private void Awake()
    {
        if (!created)
        {
            
        }
    }

    void Start()
    {
        updateLevelButtons();
    }

    private void disableLevelButtons()
    {
        level1Button.SetActive(false);
        level2Button.SetActive(false);
        level3Button.SetActive(false);
        level4Button.SetActive(false);
        level5Button.SetActive(false);
        level6Button.SetActive(false);
        level7Button.SetActive(false);
        level8Button.SetActive(false);
        level9Button.SetActive(false);
    }

    public void updateLevelButtons()
    {
        disableLevelButtons();
        
        int i = 1;
        if (unlockedLevel < i++) return;
        level1Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level2Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level3Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level4Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level5Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level6Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level7Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level8Button.SetActive(true);
        if (unlockedLevel < i++) return;
        level9Button.SetActive(true);
    }

    void Update()
    {
        if (PlayerController.instance.dead)
        {
            updateLevelButtons();
            startUI.SetActive(true);
        }
        else
        {
            startUI.SetActive(false);
        }
	}

    public void StartPlaying()
    {
        PlayerController.instance.Reset();
    }

    public void loadLevel(int i)
    {
        SceneManager.LoadScene("level" + i);
        PlayerController.level = i;
        PlayerController.skipUI = true;
    }
}
