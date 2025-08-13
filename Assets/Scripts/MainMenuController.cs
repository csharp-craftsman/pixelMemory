using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class Settings
{

    Slider sfxVolumeSlider;
    Slider bgMusicSlider;


    public Settings()
    {
        sfxVolumeSlider = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>();
        bgMusicSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();

        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        bgMusicSlider.onValueChanged.RemoveAllListeners();

        sfxVolumeSlider.onValueChanged.AddListener((float v) => { GameManager.Instance.Audio.SetSFXVolume(v); Debug.Log($"sfx:{v}"); });
        bgMusicSlider.onValueChanged.AddListener((float v) => { GameManager.Instance.Audio.SetMusicVolume(v); Debug.Log($"music:{v}"); });


    }



    






}


public class HearthBar
{
    public int HeartCount;

    public int HeartDuration;


    TextMeshProUGUI heartCountTxt;
    TextMeshProUGUI heartClockTxt;

    public HearthBar()
    {
        heartCountTxt = GameObject.Find("HeartCountText").GetComponent<TextMeshProUGUI>();
        heartClockTxt = GameObject.Find("HeartClockText").GetComponent<TextMeshProUGUI>();

        
    }



    public void Update()
    {
        HeartSystem sys = GameManager.Instance.HController;
        int hCount = sys.heart.value;
        int hDur = sys.GetRegenDuration();

        if(hCount != HeartCount || hDur != HeartDuration)
        {
            HeartDuration = hDur;
            HeartCount = hCount;
            updateTexts();
        }

    }

    void updateTexts()
    {
        StringBuilder sb = new StringBuilder();

        int h = HeartDuration / 3600;
        int m = HeartDuration / 60;

        sb.Append(h.ToString("00"));
        sb.Append(":");
        sb.Append(m.ToString("00"));

        heartClockTxt.text = sb.ToString();
        heartCountTxt.text = HeartCount.ToString();

    }


}




public class MainMenuController : MonoBehaviour
{

    Canvas watchAdCanvas;
    Settings set;
    HearthBar bar;


    // Start is called before the first frame update
    void Start()
    {
        //test();
        GameManager.Instance.LoadGame();
        GameManager.Instance.Audio.PlayBGMusic("kennedy");
        watchAdCanvas = GameObject.Find("WatchAdCanvas").GetComponent<Canvas>();

        set = new Settings();
        bar = new HearthBar();
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


    public void ValueChanged(float value)
    {
        Debug.Log($"ValueChangedddd to {value}");
    }


    // Update is called once per frame
    void Update()
    {
        bar.Update();
    }
}
