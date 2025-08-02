using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SceneLoader
{
    public int MenuSceneIdx = 0;
    public int LevelSelectionSceneIdx = 1; 

    public void LoadMainMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MenuSceneIdx);
    }


    public void LoadLevelSelectionScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelSelectionSceneIdx);

    }

    public void LoadLevelPlayScene(int level)
    {
        int sceneIdx = level + LevelSelectionSceneIdx;
        if (sceneIdx <= LevelSelectionSceneIdx)
            throw new System.Exception("WrongLevelNumber");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIdx);

    }





}






public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public SceneLoader SLoader;


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

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
