using System;
using Coffee.UIExtensions;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Background")] [SerializeField]
        private Sprite gameBg;

        [SerializeField] private Sprite bg;
        [SerializeField] private Image background;

        [Space(10)] [Header("MainMenu UI")] [SerializeField]
        private GameObject canvas;

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsButton;
        [SerializeField] private GameObject shopButton;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject rankingButton;
        [SerializeField] private GameObject frame;
        [SerializeField] private Text lastScoreText;
        [SerializeField] private Text rankingText;
        [SerializeField] private Text lastScore;
        [SerializeField] private Text highScore;
        [SerializeField] private Text magnetCount;
        [SerializeField] private Text slowCount;
        [SerializeField] private Text bombCount;


        [Space(10)] [Header("Shop UI")] [SerializeField]
        private GameObject shopMenu;

        [Space(10)] [Header("Settings UI")] [SerializeField]
        private GameObject settingsMenu;

        [SerializeField] private Image colorPreview;

        [Space(10)] [Header("Pause UI")] [SerializeField]
        private GameObject pauseMenu;

        [Space(10)] [Header("Temp UI")] public GameObject tempSettings;
        public GameObject tempPanel;

        [Space(10)] [SerializeField] private Text highscoreText;

        [SerializeField] private Text scoreText;
        public ParticleSystem explotion;


        public GameObject game;
        private GameObject currScreen;

        public UnityAction<GameObject> CreateGameWindow;
        public UnityAction DestroyGame;
        public UnityAction Magnet;

        private void Start()
        {
            UpdateUI();
            RectTransform rt = frame.GetComponent<RectTransform>();
            GameData.Instance().mainCanvasHeight = canvas.GetComponent<RectTransform>().rect.size.y;
            rt.offsetMin = new Vector2(rt.offsetMin.x, GetBannerHeight());
            colorPreview.color = GameData.Instance().color;

            currScreen = mainMenu;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (GameData.Instance().gameState == State.PLAYING)
                {
                    DestroyGame();
                    currScreen = mainMenu;
                    mainMenu.SetActive(true);
                    GameData.Instance().gameState = State.MAIN_MENU;
                }
                else if (currScreen == tempPanel)
                {
                    mainMenu.SetActive(true);
                    tempPanel.SetActive(false);
                    currScreen = mainMenu;
                    UpdateUI();
                }
                else
                {
                    currScreen.SetActive(false);
                    currScreen = mainMenu;
                    mainMenu.SetActive(true);
                    GameData.Instance().gameState = State.MAIN_MENU;
                }
            }
        }

        public void PlayButtonListener()
        {
            mainMenu.SetActive(false);
            settingsButton.SetActive(false);
            shopButton.SetActive(false);
            pauseButton.SetActive(true);
            try
            {
                CreateGameWindow(frame);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Create Game Windows probabliy null");
            }

            if (game == null) throw new Exception("uiManager game is null");
            game.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            background.sprite = gameBg;
            ParticleSystem.MainModule main = explotion.main;
            main.startColor = GameData.Instance().color;
        }
        
        public void OnSettingsButtonClick()
        {
            currScreen.SetActive(false);
            currScreen = settingsMenu;
            settingsMenu.SetActive(true);
        }

        public void OnPauseButtonClick()
        {
            if (GameData.Instance().gameState == State.PLAYING)
            {
                GameData.Instance().gameState = State.PAUSE;
                pauseMenu.SetActive(true);
                DOTween.TogglePauseAll();

                game.SetActive(false);
            }
            else if (GameData.Instance().gameState == State.PAUSE)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.PrependInterval(3).OnComplete(() =>
                {
                    DOTween.TogglePauseAll();
                    GameData.Instance().gameState = State.PLAYING;
                    game.SetActive(true);
                    pauseMenu.SetActive(false);
                    sequence.Kill();
                });
            }
        }

        public void OnShopButtonClick()
        {
            currScreen.SetActive(false);
            currScreen = shopMenu;
            shopMenu.SetActive(true);
        }

        public void UpdateUI()
        {
            print("updateUI");

            highScore.text = "" + GameData.Instance().highScore;
            lastScore.text = "" + GameData.Instance().lastScore;
            magnetCount.text = "" + GameData.Instance().magnetBoosterCount;
            slowCount.text = "" + GameData.Instance().slowBoosterCount;
            bombCount.text = "" + GameData.Instance().bombBoosterCount;
        }
        
        private float GetBannerHeight()
        {
            float height = Mathf.RoundToInt(50 * Screen.dpi / 160);
            float margin = height / 4;
            return height + margin;
        }

        public void SetTexts()
        {
            LanguageManager manager = LanguageManager.Instance();
            highscoreText.text = manager.getString(StringData.highscore);
            lastScoreText.text = manager.getString(StringData.lastScore);
            rankingText.text = manager.getString(StringData.ranking);
        }

        public void SetScoreText(Text scoreText)
        {
            this.scoreText = scoreText;
        }

        public void ChangeColor()
        {
            ColorData.NextColor();
            colorPreview.color = GameData.Instance().color;
        }

        public void SetScore(int score)
        {
            scoreText.text = "" + score;
        }

        public void GameOver()
        {
            Image handle = game.GetComponentInChildren<PlayerCollider>().gameObject.GetComponent<Image>();
            Sequence sequence = DOTween.Sequence();
            sequence.PrependCallback(() =>
                {
                    DOTween.PauseAll();
                    GameData.Instance().gameState = State.EXIT_PLAYING;
                    sequence.TogglePause();
                    handle.CrossFadeAlpha(0f, 0.01f, true);
                })
                .AppendCallback(() => explotion.Play()).AppendInterval(2f).OnComplete(() =>
                {
                    Destroy(game);
                    currScreen = mainMenu;
                    GameData.Instance().gameState = State.MAIN_MENU;
                    mainMenu.SetActive(true);
                    settingsButton.SetActive(true);
                    shopButton.SetActive(true);
                    pauseButton.SetActive(false);
                    sequence.Kill();
                    background.sprite = bg;
                });
        }

        //temp
        public void temp()
        {
            settingsMenu.SetActive(false);
            tempPanel.SetActive(true);
            currScreen = tempPanel;
        }

        public void MagnetPower()
        {
            GameData.Instance().magnetBoosterCount--;
            Magnet();
            UpdateUI();
        }

        public void SlowPower()
        {
            
        }

        public void BombPower()
        {
            
        }
    }
}