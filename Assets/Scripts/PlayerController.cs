using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private GameObject player;
    private float dir = 1;
    private Sequence sequence;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        player.GetComponent<Image>().color = GameData.Instance().Color;
        sequence = DOTween.Sequence();
        sequence.SetLoops(-1);
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
            slider.value += .1f * GameData.Instance().playerSpeed * dir * Time.deltaTime;
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
                if (go.gameObject.name.Contains("Button") && !go.gameObject.name.Equals("PauseButton")) return false;
            }
        }

        return true;
    }
}