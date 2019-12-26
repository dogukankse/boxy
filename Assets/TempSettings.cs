using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TempSettings : MonoBehaviour
{
    private string path;
    public List<InputField> inputField;
    public Button saveButton;
    private string saveData;


    private void Start()
    {
        path = $"{Application.persistentDataPath}/save.json";

        inputField[0].text = GameData.Instance().highScore.ToString();
        inputField[1].text = GameData.Instance().lastScore.ToString();
        inputField[2].text = GameData.Instance().magnetBoosterCount.ToString();
        inputField[3].text = GameData.Instance().slowBoosterCount.ToString();
        inputField[4].text = GameData.Instance().bombBoosterCount.ToString();
        inputField[5].text = GameData.Instance().bornDelay.ToString();
        inputField[6].text = GameData.Instance().objectSpeed.ToString();
        inputField[7].text = GameData.Instance().pointBornRatio.ToString();
        inputField[8].text = GameData.Instance().playerSpeed.ToString();
    }

    public void save()
    {
        GameData.Instance().highScore = Convert.ToInt32(inputField[0].text);
        GameData.Instance().lastScore = Convert.ToInt32(inputField[1].text);
        GameData.Instance().magnetBoosterCount = Convert.ToInt32(inputField[2].text);
        GameData.Instance().slowBoosterCount = Convert.ToInt32(inputField[3].text);
        GameData.Instance().bombBoosterCount = Convert.ToInt32(inputField[4].text);
        GameData.Instance().bornDelay = Convert.ToSingle(inputField[5].text);
        GameData.Instance().objectSpeed = Convert.ToSingle(inputField[6].text);
        GameData.Instance().pointBornRatio = Convert.ToSingle(inputField[7].text);
        GameData.Instance().playerSpeed = Convert.ToSingle(inputField[8].text);


        using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
        {
            fs.SetLength(0);
            string json = JsonUtility.ToJson(GameData.Instance());
            byte[] info = new UTF8Encoding(true).GetBytes(json);
            fs.Write(info, 0, info.Length);
            Debug.Log("Saved");
        }
    }
}