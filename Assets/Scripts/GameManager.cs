using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct AudioTuple
{
    public string key;
    public AudioClip clip;
}

public class AudioSystem
{
    List<AudioTuple> soundEffectList;
    List<AudioTuple> bgMusicList;

    AudioSource backgroundMusicSource;
    AudioSource soundEffectSource;


    public AudioSystem( List<AudioTuple> sfx , List<AudioTuple> bgm )
    {
        soundEffectList = sfx;
        bgMusicList = bgm;

        backgroundMusicSource = GameObject.Find("BGMusicSource").GetComponent<AudioSource>();
        soundEffectSource = GameObject.Find("SoundEffectSource").GetComponent<AudioSource>();


        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = 0.5f;

        soundEffectSource.loop = false;
        soundEffectSource.volume = 0.5f;


    }


    public void PlaySoundEffect(string key)
    {
        
        for (int i = 0; i < soundEffectList.Count; i++)
            if (soundEffectList[i].key == key)
            {
                playClip(soundEffectSource, soundEffectList[i].clip);
                return;
            }

        throw new Exception("Sound Effect Can Not Be Found");
               
                
    }

    public void PlayBGMusic(string key)
    {
        for(int i = 0;i < bgMusicList.Count; i++)
            if (bgMusicList[i].key == key)
            {
                playClip(backgroundMusicSource, bgMusicList[i].clip);
                return;
            }

        throw new Exception("Background Music Can Not Be Found");
                
        
    }


    void playClip(AudioSource src , AudioClip clp)
    {
        src.clip = clp;
        src.Play();
    }




}







public class HeartSystem
{
    int heart = 3;
    int maxHeart = 3;
    Timer regenTimer;
    int tickPeriod = 0;


    public HeartSystem(int tickPeriod) {


        heart = -4;
        maxHeart = 3;
        this.tickPeriod = tickPeriod;
    
    }


    public void Regenerate()
    {
        if (regenTimer == null)
            return;

        if (regenTimer.IsFinished())
        {
            heart++;
            regenTimer.Reset();
        }


    }


    public int CalculateOfflineHeart(DateTime lastHeartChangeTime) { 
    
        TimeSpan offset = DateTime.Now - lastHeartChangeTime;
        int extra = (int)(offset.TotalSeconds / tickPeriod);
        return (extra < 0) ? 0 : (extra > maxHeart) ? maxHeart : extra;
    
    }

    public int CalculateOfflineDuration(DateTime lastHeartChangeTime) {

        TimeSpan offset = DateTime.Now - lastHeartChangeTime;
        int extra = (int)(offset.TotalSeconds % tickPeriod);
        return (extra < 0) ? 0 : extra ;

    }


    public void Initialize(GameData data){
        int extraHeart = CalculateOfflineHeart(data.hearthChangeTime);
        int extraDur = CalculateOfflineDuration(data.hearthChangeTime);
        int lastCount = data.lastHearthCount;
        heart = lastCount + extraHeart;
        regenTimer = new Timer(Time.time - extraDur, tickPeriod );
        Debug.Log($"You have {heart} hearts. Next heart is going to be gotten after {regenTimer.GetRemainingSecs()} seconds");
    }




}





public struct GameData
{
    public int nextPlayableLevel;
    public DateTime hearthChangeTime;
    public int lastHearthCount;
    
}


public class SaveLoader
{
    public string FileName;
    private string FullPath;
    public SaveLoader(string filename)
    {
        FileName = filename;
        FullPath = Path.Combine(Application.dataPath, FileName);
        Debug.Log($"Application path is on : {FullPath}");

    }


    public bool IsFileExist()
    {
        return File.Exists( FullPath );
    }


    public void Save(GameData data)
    {
        
        using (FileStream stream = File.Open(FullPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"MAX_LEVEL={data.nextPlayableLevel}");
                writer.WriteLine($"LAST_HEART_CHANGE_TIME={data.hearthChangeTime}");
                writer.WriteLine($"LAST_HEART_COUNT={data.lastHearthCount}");
            }
        }

    }


    


    public GameData Load() {

        if (!IsFileExist())
            throw new Exception("There is no file to load");

        GameData data = new GameData();
        data.nextPlayableLevel = -1;
        data.hearthChangeTime = default(DateTime);
        data.lastHearthCount = -1;

        using (FileStream stream = File.Open(FullPath, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                data.nextPlayableLevel = Convert.ToInt32(
                    parseValueFrom(reader.ReadLine())
                    );
                data.hearthChangeTime = Convert.ToDateTime(
                    parseValueFrom(reader.ReadLine())
                    );
                data.lastHearthCount = Convert.ToInt32(
                    parseValueFrom(reader.ReadLine())
                    );
            }
        }

        if (data.nextPlayableLevel == -1 || data.lastHearthCount == -1)
            throw new System.Exception($"Data overwritten with a default value : nextLevel={data.nextPlayableLevel} changeTime={data.hearthChangeTime} ");


        return data;
    
    }


    string parseValueFrom(string line)
    {
        line.Trim();
        string[] token = line.Split("=");
        return token[1];
    }

    public void NewProgress()
    {
        GameData data = new GameData();
        data.nextPlayableLevel = 1;
        data.hearthChangeTime = DateTime.Now;
        data.lastHearthCount = 3;
        this.Save(data);
    }



}



public class SceneLoader
{
    public int MenuSceneIdx = 0;
    public int LevelSelectionSceneIdx = 1;
    public int SceneIdx = 0;

    public void LoadMainMenuScene()
    {
        loadScene(MenuSceneIdx);
    }


    public void LoadLevelSelectionScene()
    {
        
        loadScene(LevelSelectionSceneIdx);

    }

    public void LoadLevelPlayScene(int level)
    {
        int sceneIdx = level + LevelSelectionSceneIdx;
        if (sceneIdx <= LevelSelectionSceneIdx)
            throw new System.Exception("WrongLevelNumber");
        loadScene(sceneIdx);

    }

    public int CalculateLevelFromSceneIdx() => SceneIdx - LevelSelectionSceneIdx; 


    void loadScene(int idx)
    {
        SceneIdx = idx;
        UnityEngine.SceneManagement.SceneManager.LoadScene(idx);
    }





}






public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public SceneLoader SLoader;

    public SaveLoader Saving;

    public AudioSystem Audio;

    public GameData saveData;

    public HeartSystem HController;



    public string FileName;

    public int HeartRegenPeriod;

    public List<AudioTuple> soundEffects;
    public List<AudioTuple> BGMusics;


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else if(this.gameObject != Instance.gameObject)
            Destroy(this.gameObject);
            
    }

    void initialize()
    {
        Instance = this;
        SLoader = new SceneLoader();
        Saving = new SaveLoader(FileName);
        HController = new HeartSystem(HeartRegenPeriod);
        Audio = new AudioSystem(soundEffects , BGMusics);

    }

    public void LoadGame()
    {
        if (!Saving.IsFileExist())
        {
            Saving.NewProgress();
        }
        saveData = Saving.Load();
        HController.Initialize(saveData);

    }

    public void SaveIfNextLevelWon()
    {
        GameData data = Instance.saveData;
        int current = SLoader.CalculateLevelFromSceneIdx();
        int next = data.nextPlayableLevel;
        Debug.Log($"CurrentLevel : {current} NextLevel: {next}");
        if (current == next)
        {
            data.nextPlayableLevel += 1;
            Saving.Save(data);
            saveData = data;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(HController != null)
            HController.Regenerate();
    }
}
