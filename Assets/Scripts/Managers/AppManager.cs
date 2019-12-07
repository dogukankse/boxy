using System;
using UnityEngine;

namespace Managers
{
    public class AppManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private GameObject gameScreen;

        [SerializeField] private UIManager uiManager;

        private void Awake()
        {
            LanguageManager.Instance().SetLanguage("English");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            uiManager.SetTexts();
        }
    }
}