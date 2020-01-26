﻿using System.Collections;
using System.IO;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class ShareTemp : MonoBehaviour
{
    [SerializeField] private Text username;
    [SerializeField] private Text score;


    void Start()
    {
        SetUsername();
        SetScore();

        StartCoroutine(SaveScreenshot());
    }

    private void ShareScreenshot()
    {
        string imgPath = Path.Combine(Application.persistentDataPath, "hs.png");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            gameObject.SetActive(false);
    }


    private void SetUsername()
    {
        if (GameServices.IsInitialized())
            username.text = GameServices.LocalUser.userName;
        else
            username.text = GameData.Instance().username ?? "Boxy";
    }

    private void SetScore()
    {
        score.text = GameData.Instance().highScore.ToString();
    }

    IEnumerator SaveScreenshot()
    {
        yield return new WaitForEndOfFrame();


        string path = Sharing.SaveScreenshot("hs");
        Debug.Log(path);
        Sharing.ShareImage(path, "Can you beat me?");
    }
}