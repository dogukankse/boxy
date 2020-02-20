using System;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Data;
using Data;
using DG.Tweening;
using EasyMobile;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private RectTransform player;
        [SerializeField] private GameObject point;
        [SerializeField] private GameObject obstacle;
        [SerializeField] public PlayerCollider playerCollider;
        [SerializeField] private Text scoreText;

        [SerializeField] private GameObject magnetBooster;
        [SerializeField] private GameObject slowBooster;
        [SerializeField] private GameObject bombBooster;

        private bool isBoosterValid = true;
        private bool isSpeedValid = true;
        private bool isRatioValid = true;

        private float localBornDelay;
        private float localObjectSpeed;
        private float localPointBornRatio;

        public Button magnet;
        public Button slow;
        public Button bomb;

        public UnityAction UpdateUI;
        public UnityAction UpdateGameUI;


        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float startPosX;

        private int callID;
        private int cubeID;
        public List<Cube> currentCubes;

        private float gameWidth;
        private float gameHeight;
        private float cubeSize;

        private Sequence tweenSequence;

        [Header("Right")] public GameObject R_P1;
        public GameObject R_P2;

        [Header("Left")] public GameObject L_P1;
        public GameObject L_P2;

        Vector3 R;
        Vector3 L;

        #region UnityLifeCycle

        private void Awake()
        {
            localBornDelay = GameData.Instance().bornDelay;
            localObjectSpeed = GameData.Instance().objectSpeed;
            localPointBornRatio = GameData.Instance().pointBornRatio;
            player.gameObject.GetComponent<PlayerController>().localPlayerSpeed = GameData.Instance().playerSpeed;

            playerCollider.DestroyOther = DestroyOther;


            R = (R_P2.transform.position - R_P1.transform.position).normalized;
            L = (L_P2.transform.position - L_P1.transform.position).normalized;


            print($"width: {Screen.width} height: {Screen.height}");
            print(
                $"rect.size width: {canvas.GetComponent<RectTransform>().rect.size.x} rect.size height: {canvas.GetComponent<RectTransform>().rect.size.y}");
            print(
                $"cube sizes: {point.GetComponent<RectTransform>().lossyScale} {point.GetComponent<RectTransform>().sizeDelta} {point.transform.lossyScale}");

            currentCubes = new List<Cube>();
            //gameWidth = canvas.GetComponent<RectTransform>().rect.size.x;
            //gameHeight = canvas.GetComponent<RectTransform>().rect.size.y;
            gameWidth = canvas.GetComponent<RectTransform>().rect.size.x; //Screen.width;
            gameHeight = canvas.GetComponent<RectTransform>().rect.size.y; //Screen.height;

            cubeSize = point.GetComponent<RectTransform>().sizeDelta.x;
            bornLeftX = (gameWidth / -2f);
            bornRightX = (gameWidth / 2f);
            bornY = (gameHeight / 2f) + (cubeSize * 2f); // + (gameHeight / 10f);
        }

        private float timeLeft;

        private void Start()
        {
            UpdateGameUI();
            /* tweenSequence = DOTween.Sequence();
             callID = 1;
             tweenSequence.PrependCallback(CreateCube)
                 .PrependInterval(localBornDelay)
                 .SetLoops(-1).SetId(callID);*/
            CreateCube();
        }

        private void Update()
        {
            if (playerCollider.Score == 0) return;

            if (playerCollider.Score % 2 == 0 && isSpeedValid && playerCollider.Score < 25)
            {
                localObjectSpeed /= 1.1f;
                player.gameObject.GetComponent<PlayerController>().localPlayerSpeed *= 1.05f;
                tweenSequence.Restart();
                isSpeedValid = false;
            }
            else if (playerCollider.Score % 2 != 0)
                isSpeedValid = true;

            if (playerCollider.Score % 5 == 0 && isRatioValid)
            {
                localBornDelay /= 1.3f;
                print("update");
                isRatioValid = false;
            }
            else if (playerCollider.Score % 5 != 0)
                isRatioValid = true;
        }


        private void OnDestroy()
        {
            print("gameManager on destroy");
            DOTween.Kill(callID);
            tweenSequence.Kill();
            GameData.Instance().lastScore = playerCollider.Score;
            if (GameServices.IsInitialized())
                GameServices.ReportScore(GameData.Instance().lastScore, EM_GameServicesConstants.Leaderboard_Rakings);
            if (GameData.Instance().lastScore > GameData.Instance().highScore)
                GameData.Instance().highScore = GameData.Instance().lastScore;
            UpdateUI();
        }

        #endregion

        #region Powerups

        public void Magnet()
        {
            foreach (Cube cube in currentCubes)
            {
                if (cube.cubeType == CubeType.OBSTACLE || cube.gameObject == null) continue;

                DOTween.To(() => cube.gameObject.GetComponent<RectTransform>().anchoredPosition3D,
                    (x) =>
                    {
                        cube.gameObject.GetComponent<RectTransform>().DOMove(playerCollider.transform.position, 1f)
                            .SetLink(cube.gameObject, LinkBehaviour.KillOnDestroy);
                    },
                    player.transform.position, 1f).SetLink(cube.gameObject, LinkBehaviour.KillOnDestroy);
            }

            UpdateGameUI();
        }

        public void Slow()
        {
            DOTween.timeScale /= 2f;
            float start = 0;
            slow.interactable = false;
            DOTween.To(() => start, (x) => start = x, 3f, 3f)
                .OnComplete(() =>
                {
                    DOTween.timeScale *= 2f;
                    slow.interactable = true;
                });

            UpdateGameUI();
        }

        public void Bomb()
        {
            bomb.interactable = false;
            foreach (Cube cube in currentCubes)
            {
                if (cube.gameObject == null) continue;
                cube.gameObject.GetComponent<Image>().CrossFadeAlpha(0f, .01f, true);
                cube.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                DOTween.Kill(cube.DOTweenCallID);
                ParticleSystem ps = cube.gameObject.GetComponentInChildren<ParticleSystem>();
                var main = ps.main;
                main.startColor = cube.gameObject.name.Contains("Obstacle")
                    ? Color.white
                    : GameData.Instance().Color;
                ps.Play();


                float start = 0;

                DOTween.To(() => start, (x) => start = x, 1f, 1f)
                    .SetLink(cube.gameObject, LinkBehaviour.KillOnDestroy)
                    .OnComplete(() =>
                    {
                        Destroy(cube.gameObject);
                        RemoveNulls();
                        bomb.interactable = true;
                    });
            }

            UpdateGameUI();
        }

        #endregion

        private void CreateCube()
        {
            GameObject cube;
            if (Random.Range(0f, 1f) > .5f)
            {
                startPosX = Random.Range(cubeSize * 2f, bornRightX + (cubeSize * 2));
                //Random.Range(bornRightX + (cubeSize * 2), bornRightX - (cubeSize * 2));
            }
            else
            {
                startPosX = Random.Range(-(cubeSize * 2f), bornLeftX - (cubeSize * 2f));
                //Random.Range(bornLeftX - (cubeSize * 2), bornLeftX + (cubeSize * 2));
            }

            float r = Random.Range(0f, 1f);

            if (playerCollider.Score % 30 == 0 && playerCollider.Score != 0 && isBoosterValid)
            {
                print("create booster");
                if (r < 0.33f)
                {
                    //magnetbooster
                    cube = Instantiate(magnetBooster, canvas.transform, false);
                }
                else if (r >= 0.33f && r < 0.66f)
                {
                    //slowbooster
                    cube = Instantiate(slowBooster, canvas.transform, false);
                }
                else
                {
                    //bombbooster
                    cube = Instantiate(bombBooster, canvas.transform, false);
                }

                cube.GetComponent<Image>().color = GameData.Instance().Color;
                cubeID = 11;
                currentCubes.Add(new Cube(cube, CubeType.BOOSTER, cubeID));

                isBoosterValid = false;
            }
            else
            {
                if (localPointBornRatio <= r)
                {
                    cube = Instantiate(obstacle, canvas.transform, false);
                    cubeID = 10;
                    currentCubes.Add(new Cube(cube, CubeType.OBSTACLE, cubeID));
                }
                else
                {
                    cube = Instantiate(point, canvas.transform, false);
                    cube.GetComponent<Image>().color = GameData.Instance().Color;
                    cubeID = 11;
                    currentCubes.Add(new Cube(cube, CubeType.POINT, cubeID));
                }
            }

            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startPosX, bornY);


            float curve = Random.Range((gameWidth * 14f) / 100f, gameWidth - (gameWidth * 14f) / 100f);
            rectTransform.DOPath(
                    new Vector3[]
                    {
                        new Vector2(curve, gameHeight * 23f / 100f),
                        new Vector2(curve, -gameHeight / 10f), //-100f),
                    },
                    localObjectSpeed, PathType.CatmullRom,
                    PathMode.Ignore, 5, Color.red)
                .SetLink(cube, LinkBehaviour.KillOnDestroy)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    rectTransform.DORotate(new Vector3(0, 0, 360), localObjectSpeed,
                            RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetRelative(true)
                        .SetLink(cube, LinkBehaviour.KillOnDestroy);
                })
                .OnComplete(() =>
                {
                    Destroy(cube);
                    RemoveNulls();
                }).SetId(cubeID);
            DOVirtual.DelayedCall(localBornDelay, CreateCube);
        }

        private void RemoveNulls()
        {
            currentCubes.RemoveAll(item => item.gameObject == null);
        }

        public Text GetScoreText()
        {
            return scoreText;
        }

        private void DestroyOther(GameObject other)
        {
            Destroy(other);
            RemoveNulls();
        }

        private Vector2 CalculateLinePoint()
        {
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                var p = R_P1.transform.position + (Random.Range(0f, 1f) * R);
                print(p);
                return p;
            }

            var p2 = L_P1.transform.position + (Random.Range(0f, 1f) * L);
            print(p2);
            return p2;
        }
    }
}