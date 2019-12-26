using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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


        public Button magnet;
        public Button slow;
        public Button bomb;

        public UnityAction UpdeteUI;

        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float startPosX;


        private int callID;
        private int cubeID;
        private List<GameObject> currentCubes;


        private float gameWidth;
        private float gameHeight;


        private void Awake()
        {
            print($"width: {Screen.width} height: {Screen.height}");
            print(
                $"rect.size width: {canvas.GetComponent<RectTransform>().rect.size.x} rect.size height: {canvas.GetComponent<RectTransform>().rect.size.y}");
            print($"{Mathf.Abs(player.anchoredPosition.y * 100) / canvas.GetComponent<RectTransform>().rect.size.y}");
            print($"{player.anchoredPosition.y} {canvas.GetComponent<RectTransform>().rect.size.y}");

            currentCubes = new List<GameObject>();
            gameWidth = canvas.GetComponent<RectTransform>().rect.size.x;
            gameHeight = canvas.GetComponent<RectTransform>().rect.size.y;
            //gameWidth = Screen.width;
            //gameHeight = Screen.height;


            bornLeftX = (gameWidth / -2f);
            bornRightX = (gameWidth / 2f);
            bornY = gameHeight / 2f + (gameHeight / 10f);

            /*GameObject go = new GameObject("endLeft");
            go.transform.SetParent(canvas.transform);
            RectTransform r = go.AddComponent<RectTransform>();
            r.anchoredPosition = new Vector2(endLeftX, endY);
            r.sizeDelta = new Vector2(1, 1);

            go = new GameObject("endright");
            go.transform.SetParent(canvas.transform);
            r = go.AddComponent<RectTransform>();
            r.anchoredPosition = new Vector2(endRightX, endY);
            r.sizeDelta = new Vector2(1, 1);

            go = new GameObject("bornright");
            go.transform.SetParent(canvas.transform);
            r = go.AddComponent<RectTransform>();
            r.anchoredPosition = new Vector2(bornRightX, bornY);
            r.sizeDelta = new Vector2(1, 1);

            go = new GameObject("bornleft");
            go.transform.SetParent(canvas.transform);
            r = go.AddComponent<RectTransform>();
            r.anchoredPosition = new Vector2(bornLeftX, bornY);
            r.sizeDelta = new Vector2(1, 1);

            go = new GameObject("die");
            go.transform.SetParent(canvas.transform);
            r = go.AddComponent<RectTransform>();
            r.anchoredPosition = new Vector2(0, dieY);
            r.sizeDelta = new Vector2(1, 1);*/
        }


        private void Start()
        {
            Sequence tweenSequence = DOTween.Sequence();
            callID = 1;
            tweenSequence.PrependCallback(CreateCube).PrependInterval(GameData.Instance().bornDelay)
                .SetLoops(-1).SetId(callID);
        }


        void CreateCube()
        {
            GameObject cube;
            startPosX = Random.Range(bornLeftX, bornRightX);

            if (GameData.Instance().pointBornRatio <= Random.Range(0f, 1f))
            {
                cube = Instantiate(obstacle, canvas.transform, false);
                cubeID = 10;
            }
            else
            {
                cube = Instantiate(point, canvas.transform, false);
                cube.GetComponent<Image>().color = GameData.Instance().color;
                cubeID = 11;
                currentCubes.Add(cube);
            }

            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startPosX, bornY);
            //float size = 100f * (Screen.width / Screen.height);
            //rectTransform.

            /*rectTransform.DORotate(new Vector3(0, 0, 360), GameData.Instance().objectSpeed,
                RotateMode.LocalAxisAdd).SetLoops(-1).SetRelative();
            rectTransform.DOAnchorPos(new Vector2(startPosX, dieY), GameData.Instance().objectSpeed)
                .SetEase(Ease.InSine).OnComplete(() => Destroy(cube));*/

            /* float myFloat = 0;
             DOTween.To(() => myFloat, (x) =>
             {
                 myFloat = x;
                 rectTransform.anchoredPosition = Interpolate(new Vector2(startPosX, bornY),
                     new Vector3(Screen.width / 2, Screen.height), endPos,
                     new Vector3(endPosX, -100), x);
             }, 1, GameData.Instance().objectSpeed).OnComplete(() => Destroy(cube)).SetAutoKill(false);
             
 */

            float curve = Random.Range((Screen.width * 13.5f) / 100f, Screen.width - (Screen.width * 13.5f) / 100f);

            rectTransform.DOPath(
                    new Vector3[]
                    {
                        new Vector3(Screen.width / 2f, Screen.height),
                        new Vector2(curve, (Screen.height * 23f) / 100f),
                        new Vector3(curve, -100)
                    },
                    GameData.Instance().objectSpeed, PathType.CatmullRom,
                    PathMode.Ignore, 5, null)
                .OnStart(() =>
                {
                    rectTransform.DORotate(new Vector3(0, 0, 360), GameData.Instance().objectSpeed,
                        RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetRelative(true);
                })
                .OnComplete(() =>
                {
                    Destroy(cube);
                    RemoveNulls();
                }).SetId(cubeID);
        }

        private void RemoveNulls()
        {
            currentCubes.RemoveAll(item => item == null);
        }


        private void OnDestroy()
        {
            print("gameManager on destroy");
            DOTween.Kill(callID);
            GameData.Instance().lastScore = playerCollider.Score;
            if (GameData.Instance().lastScore > GameData.Instance().highScore)
                GameData.Instance().highScore = GameData.Instance().lastScore;
            UpdeteUI();
        }

        public Text GetScoreText()
        {
            return scoreText;
        }

        Vector3 Interpolate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u)
        {
            return (
                0.5f *
                (
                    (-a + 3f * b - 3f * c + d) *
                    (u * u * u) +
                    (2f * a - 5f * b + 4f * c - d) *
                    (u * u) +
                    (-a + c) *
                    u + 2f * b
                )
            );
        }

        public void Magnet()
        {
            DOTween.Kill(11);
            foreach (GameObject cube in currentCubes)
            {
                if (cube != null)
                    DOTween.To(() => cube.GetComponent<RectTransform>().anchoredPosition3D,
                        (x) => { cube.GetComponent<RectTransform>().DOMove(playerCollider.transform.position, 1f); },
                        player.transform.position, 1f);
                /*cube.GetComponent<RectTransform>().DOMove(
                    playerCollider.transform.position, 1f
                ).OnComplete(RemoveNulls);*/
            }

            RemoveNulls();
        }
    }
}