using Data;
using DG.Tweening;
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

        public UnityAction UpdeteUI;


        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float endLeftX;
        private float endRightX;
        private float endY;
        private float dieY;

        private float startPosX;
        private Vector3 endPos;

        private int callID;


        private void Awake()
        {
            bornLeftX = (Screen.width / -2f);
            bornRightX = (Screen.width / 2f);
            bornY = (Screen.height / 2f) + 100;

            endLeftX = (Screen.width / -3f);
            endRightX = (Screen.width / 3f);
            endY = (Screen.height / -2f) * .6f;

            dieY = (Screen.height / -2f) - 100;
            float w = (Screen.width * 292f) / 375f;
            float h = w / 13.27f;
            player.sizeDelta = new Vector2(w, h);
            player.anchoredPosition = new Vector2(0, endY);
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
            float endPosX = Random.Range(endLeftX, endRightX);
            endPos = new Vector3(endPosX, endY, 0);
            if (Random.Range(0f, 1f) <= GameData.Instance().pointRatio)
                cube = Instantiate(obstacle, canvas.transform, true);
            else
            {
                cube = Instantiate(point, canvas.transform, true);
                cube.GetComponent<Image>().color = ColorData.orange;
            }


            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startPosX, bornY);

            rectTransform.DOAnchorPos(new Vector2(startPosX, dieY), GameData.Instance().speed).SetEase(Ease.InSine)
                .OnComplete(() => Destroy(cube));
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
    }
}