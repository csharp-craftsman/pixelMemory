using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //test();
        GameManager.Instance.LoadGame();
        GameManager.Instance.Audio.PlayBGMusic("kennedy");
    }


    public void PlayGameButton()
    {
        GameManager.Instance.SLoader.LoadLevelSelectionScene();
    }

    void test()
    {
        GameData data = new GameData();
        data.lastHearthCount = 2;
        DateTime before30 = DateTime.Now.AddMinutes(-30);
        Debug.Log(before30);
        data.hearthChangeTime = before30;
        GameManager.Instance.HController.Initialize(data);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
