using _Scripts;
using _Scripts.Data;
using _Scripts.Managers;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [Space(10)] [Header("Rankings")] [SerializeField]
        private GameObject rankingsScreen;

        [Space(10)] [Header("Boosters")] [SerializeField]
        private Button magnetBooster;

        [SerializeField] private Button slowBooster;
        [SerializeField] private Button bombBooster;

        #region UI

        [Header("Background")] [SerializeField]
        private Sprite gameBg;

        [SerializeField] private Sprite bg;
        [SerializeField] private Image background;

        [Space(10)] [Header("MainMenu UI")] [SerializeField]
        private GameObject canvas;

        // [SerializeField] private Text username;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsButton;
        // [SerializeField] private GameObject shopButton;
        [SerializeField] private GameObject pauseButton;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject frame;
        // [SerializeField] private Text lastScoreText;
        // [SerializeField] private Text rankingText;
        // [SerializeField] private Text lastScore;
        // [SerializeField] private Text highScore;
        // [SerializeField] private Text highscoreText;
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

        [SerializeField] private GameObject countdown;

        [Space(10)] [Header("Temp UI")] public GameObject tempSettings;
        public GameObject tempPanel;

        [Space(10)] [Header("Game UI")] public Button magnet;
        public Button slow;
        public Button bomb;
        [SerializeField] private Text scoreText;
        [HideInInspector] public GameObject game;

        #endregion


        [HideInInspector] public ParticleSystem explosion;

        #region UnityActions

        public UnityAction<GameObject> CreateGameWindow;
        public UnityAction DestroyGame;
        public UnityAction OnMagnetUse;
        public UnityAction OnSlowUse;
        public UnityAction OnBombUse;

        #endregion


        #region UnityLifeCycle

        private void Start()
        {
            UpdateUI();
            RectTransform rt = frame.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(rt.offsetMin.x, GetBannerHeight());
            colorPreview.color = GameData.Instance().Color;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                StateChange();
        }

        #endregion

        #region ButtonEvents

        public void OnPlayButtonClick()
        {
            CreateGame();
        }

        public void OnShareButtonClick()
        {
            GameData.Instance().gameState = State.RANKINGS;
            rankingsScreen.SetActive(true);
            backButton.SetActive(false);
            settingsMenu.SetActive(false);
        }

        public void OnRankingButtonClick()
        {
            // if (GameServices.IsInitialized())
            //     GameServices.ShowLeaderboardUI();
        }

        public void OnSettingsButtonClick()
        {
            GameData.Instance().gameState = State.SETTINGS;
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            settingsButton.SetActive(false);
            // shopButton.SetActive(false);
            backButton.SetActive(true);
        }

        public void OnPauseButtonClick()
        {
            if (GameData.Instance().gameState == State.PLAYING)
            {
                GameData.Instance().gameState = State.PAUSE;
                pauseMenu.SetActive(true);
                DOTween.TogglePauseAll();
                pauseButton.SetActive(false);
                game.SetActive(false);
            }
        }

        public void OnResumeButtonClick()
        {
            ResumeGame();
        }

        public void OnShopButtonClick()
        {
            GameData.Instance().gameState = State.SHOP;
            mainMenu.SetActive(false);
            shopMenu.SetActive(true);
            settingsButton.SetActive(false);
            // shopButton.SetActive(false);
            backButton.SetActive(true);
            // AdsManager.Instance().HideBanner();
        }

        public void OnBackButtonClick()
        {
            StateChange();
        }

        public void OnExitButtonClick()
        {
            ExitGame();
        }

        public void OnRestartButtonClick()
        {
            GameData.Instance().gameState = State.RESTART;

            pauseButton.SetActive(true);
            pauseMenu.SetActive(false);

            CreateGame();
            foreach (Transform child in frame.transform)
            {
                if (child.GetComponent<GameObject>() != null && !child.gameObject.activeInHierarchy)
                {
                    Destroy(child);
                }
            }
        }


        public void OnBombCountButtonClick()
        {
            if (GameData.Instance().bombBoosterCount != 0) return;
            // AdsManager.Instance().ShowRewardedVideo(1);
        }

        public void OnSlowCountButtonClick()
        {
            if (GameData.Instance().slowBoosterCount != 0) return;
            // AdsManager.Instance().ShowRewardedVideo(2);
        }


        public void OnMagnetCountButtonClick()
        {
            if (GameData.Instance().magnetBoosterCount != 0) return;
            // AdsManager.Instance().ShowRewardedVideo(3);
        }

        #endregion

        #region Events

        private void StateChange()
        {
            switch (GameData.Instance().gameState)
            {
                case State.PAUSE:
                    ResumeGame();
                    break;
                case State.SETTINGS:
                    settingsMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    backButton.SetActive(false);
                    // shopButton.SetActive(true);
                    settingsButton.SetActive(true);
                    GameData.Instance().gameState = State.MAIN_MENU;
                    break;
                case State.SHOP:
                    shopMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    backButton.SetActive(false);
                    // shopButton.SetActive(true);
                    settingsButton.SetActive(true);
                    GameData.Instance().gameState = State.MAIN_MENU;
                    // StartCoroutine(AdsManager.Instance().ShowBannerWhenReady());
                    break;
                case State.PLAYING:
                    ExitGame();
                    break;
                case State.RANKINGS:
                    rankingsScreen.SetActive(false);
                    mainMenu.SetActive(true);
                    // shopButton.SetActive(true);
                    settingsButton.SetActive(true);
                    backButton.SetActive(false);
                    GameData.Instance().gameState = State.MAIN_MENU;
                    break;
                case State.EXIT_PLAYING:
                    break;
                case State.RESTART:
                    break;
                default:
                case State.MAIN_MENU:
                    break;
            }
        }


        public void UpdateUI()
        {
            // print("updateUI");
            // if (GameServices.IsInitialized())
            // {
            //     username.text = GameServices.LocalUser.userName;
            //     GameData.Instance().username = GameServices.LocalUser.userName;
            //     GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_Rakings,
            //         (string leaderboardName, IScore score) =>
            //         {
            //             if (score != null)
            //             {
            //                 if (GameData.Instance().highScore > score.value)
            //                 {
            //                     highScore.text = GameData.Instance().highScore + "";
            //                 }
            //                 else
            //                 {
            //                     highScore.text = score.value + "";
            //                     GameData.Instance().highScore = (int)score.value;
            //                 }
            //
            //                 Debug.Log("Score: " + score.value + "leaderboard: " + leaderboardName);
            //             }
            //             else
            //                 highScore.text = "0";
            //         });
            // }
            // else
            // {
            //     highScore.text = "" + GameData.Instance().highScore;
            //     username.text = GameData.Instance().username ?? "Boxy";
            // }
            //
            // lastScore.text = "" + GameData.Instance().lastScore;
            // magnetCount.text = (GameData.Instance().magnetBoosterCount > 0)
            //     ? "" + GameData.Instance().magnetBoosterCount
            //     : "+";
            // slowCount.text = (GameData.Instance().slowBoosterCount > 0)
            //     ? "" + GameData.Instance().slowBoosterCount
            //     : "+";
            // bombCount.text = (GameData.Instance().bombBoosterCount > 0)
            //     ? "" + GameData.Instance().bombBoosterCount
            //     : "+";
        }

        public void UpdateGameUI()
        {
            slow.GetComponentInChildren<Text>().text = GameData.Instance().slowBoosterCount + "";
            magnet.GetComponentInChildren<Text>().text = GameData.Instance().magnetBoosterCount + "";
            bomb.GetComponentInChildren<Text>().text = GameData.Instance().bombBoosterCount + "";
        }

        #endregion


        private float GetBannerHeight()
        {
            float height = Mathf.RoundToInt(50 * Screen.dpi / 160);
            float margin = height / 4;
            return height + margin;
        }

        public void SetTexts()
        {
            LanguageManager manager = LanguageManager.Instance();
            // highscoreText.text = manager.getString(StringData.highscore);
            // lastScoreText.text = manager.getString(StringData.lastScore);
            // rankingText.text = manager.getString(StringData.ranking);
        }

        public void SetScoreText(Text scoreText)
        {
            this.scoreText = scoreText;
        }

        public void ChangeColor()
        {
            ColorData.NextColor();
            colorPreview.color = GameData.Instance().Color;
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
                .AppendCallback(() => explosion.Play()).AppendInterval(2f).OnComplete(() =>
                {
                    Destroy(game);
                    GameData.Instance().gameState = State.MAIN_MENU;
                    mainMenu.SetActive(true);
                    settingsButton.SetActive(true);
                    // shopButton.SetActive(true);
                    pauseButton.SetActive(false);
                    sequence.Kill();
                    background.sprite = bg;
                });
        }

        void CreateGame()
        {
            GameData.Instance().gameState = State.PLAYING;
            mainMenu.SetActive(false);
            settingsButton.SetActive(false);
            // shopButton.SetActive(false);
            pauseButton.SetActive(true);

            CreateGameWindow(frame);

            game.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            background.sprite = gameBg;
            ParticleSystem.MainModule main = explosion.main;
            main.startColor = GameData.Instance().Color;
        }

        void ResumeGame()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.PrependInterval(3.25f);
            sequence.PrependCallback(() =>
            {
                pauseMenu.SetActive(false);
                Instantiate(countdown, frame.transform);
            });
            sequence.OnComplete(() =>
            {
                DOTween.TogglePauseAll();
                GameData.Instance().gameState = State.PLAYING;
                game.SetActive(true);
                pauseButton.SetActive(true);
                sequence.Kill();
            });
        }

        private void ExitGame()
        {
            pauseMenu.SetActive(false);
            pauseButton.SetActive(false);
            mainMenu.SetActive(true);
            background.sprite = bg;
            settingsButton.SetActive(true);
            // shopButton.SetActive(true);
            DestroyGame();
            GameData.Instance().gameState = State.MAIN_MENU;
        }

        //temp
        public void temp()
        {
            settingsMenu.SetActive(false);
            tempPanel.SetActive(true);
        }

        #region Powers

        public void MagnetPower()
        {
            if (GameData.Instance().magnetBoosterCount == 0) return;
            GameData.Instance().magnetBoosterCount--;
            OnMagnetUse();
            UpdateUI();
        }

        public void SlowPower()
        {
            if (GameData.Instance().slowBoosterCount == 0) return;
            GameData.Instance().slowBoosterCount--;
            OnSlowUse();
            UpdateUI();
        }

        public void BombPower()
        {
            if (GameData.Instance().bombBoosterCount == 0) return;
            GameData.Instance().bombBoosterCount--;
            OnBombUse();
            UpdateUI();
        }

        #endregion
    }
}