using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //fields
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject gameScreen;
    
    
    public void PlayButtonListener()
    {
        mainMenu.SetActive(false);
        gameScreen.SetActive(true);
    }
}
