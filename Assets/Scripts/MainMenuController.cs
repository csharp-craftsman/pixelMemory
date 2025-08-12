using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    Canvas watchAdCanvas;


    // Start is called before the first frame update
    void Start()
    {
        //test();
        GameManager.Instance.LoadGame();
        GameManager.Instance.Audio.PlayBGMusic("kennedy");
        watchAdCanvas = GameObject.Find("WatchAdCanvas").GetComponent<Canvas>();
    }


    public void PlayGameButton()
    {
        if (GameManager.Instance.HController.IsHearthOver())
        {
            watchAdCanvas.enabled = true;
            return;
        }

        GameManager.Instance.SLoader.LoadLevelSelectionScene();
    }


    public void CloseAdCanvas()
    {
        watchAdCanvas.enabled=false;
    }

    void test()
    {
        GameData data = new GameData();
        data.lastHearthCount = 2;
        DateTime before30 = DateTime.Now.AddMinutes(-30);
        Debug.Log(before30);
        data.hearthChangeTime = before30;
        GameManager.Instance.HController.LoadHeart(data);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
