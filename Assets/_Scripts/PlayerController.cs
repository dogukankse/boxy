using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private GameObject player;
        private float dir = 1;
        public float localPlayerSpeed;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            player.GetComponent<Image>().color = GameData.Instance().Color;
          
            
        }

        private void Update()
        {
            if (slider.value == 0f || slider.value == 1f)
                dir *= -1;

            if (Input.touchCount > 0 && CheckUIObjectsInPosition(Input.mousePosition))
            {
                Touch touch = Input.GetTouch(0);
                CheckUIObjectsInPosition(touch.position);

                if (touch.phase == TouchPhase.Began)
                    dir *= -1;
            }
#if UNITY_EDITOR
            //pc controller for test
            if (Input.GetMouseButtonDown(0) && CheckUIObjectsInPosition(Input.mousePosition))
            {
                dir *= -1;
            }
#endif
            if (GameData.Instance().gameState == State.PLAYING)
                slider.value += .1f * localPlayerSpeed * dir * Time.deltaTime;
        }

        bool CheckUIObjectsInPosition(Vector2 position)
        {
            //print(position);
            UnityEngine.EventSystems.PointerEventData pointer =
                new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointer.position = position;
            List<UnityEngine.EventSystems.RaycastResult>
                raycastResults = new List<UnityEngine.EventSystems.RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach (var go in raycastResults)
                {
                    //Debug.Log($"{go.gameObject.name}, {go.gameObject}, {go.GetType()}");
                    if (go.gameObject.name.Contains("Button")) return false;
                }
            }

            return true;
        }
    }
}