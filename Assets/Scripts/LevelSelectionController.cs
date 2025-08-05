using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Clusters
{
    Button[] buttons;

    public int ClusterCoeff = 0;
    public int MaxLevel = 100;
    public int NextLevel = 1;

    public Clusters(Button[] buttons , int MaxLevel , int NextLevel) { 
    
        this.buttons = buttons;
        this.MaxLevel = MaxLevel;
        this.NextLevel = NextLevel;
        this.ClusterCoeff = (NextLevel / buttons.Length);
    
    }

    public void NextCluster(){
        int maxCoeff = MaxLevel/buttons.Length;
        if (maxCoeff > ClusterCoeff)
            ClusterCoeff++;
    }

    public void PrevCluster() {
        
        if(ClusterCoeff > 0)
            ClusterCoeff--;
    }

    public void InitializeButtons()
    {
        int bLength = buttons.Length;
        for (int i = 0; i < bLength; i++)
        {
            int level = getLevelIdx(i);
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => { onButtonClick(level); });
            TextMeshProUGUI textObj = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            textObj.text = level.ToString();
        }
    }

    public void SwitchInteractionOfButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int level = getLevelIdx(i);
            buttons[i].interactable = (level <= NextLevel);

        }
    }


    void onButtonClick(int level)
    {
  
        if (level > MaxLevel)
        {
            Debug.Log("level idx is bigger than max level");
            return;
        }
        GameManager.Instance.SLoader.LoadLevelPlayScene(level);
    }


    int getLevelIdx(int iterationIdx) => ClusterCoeff * buttons.Length + (iterationIdx + 1);




}




public class LevelSelectionController : MonoBehaviour
{

    public int NextLevel = 1;
    public int MaxLevel = 100;

    private Clusters clusts;
    private Button nextCluster;
    private Button prevCluster;

    public void Start()
    {
        NextLevel = GameManager.Instance.saveData.nextPlayableLevel;
        initializeLevelSystem();


    }


    void initializeLevelSystem()
    {
        GameObject grid = GameObject.Find("LevelSelectionGrid");
        Button[] buttons = grid.GetComponentsInChildren<Button>();
        clusts = new Clusters(buttons, MaxLevel, NextLevel);

        nextCluster = GameObject.Find("NextLevelClusterButton").GetComponent<Button>();
        prevCluster = GameObject.Find("PreviousLevelClusterButton").GetComponent<Button>();

        nextCluster.onClick.AddListener(() => { onNextClusterClicked(); });
        prevCluster.onClick.AddListener(() => { onPrevClusterClicked(); });

        updateLevelButtons();
    }

    void updateLevelButtons()
    {
        clusts.SwitchInteractionOfButtons();
        clusts.InitializeButtons();
    }

    void onNextClusterClicked()
    {
        clusts.NextCluster();
        updateLevelButtons();
    }


    void onPrevClusterClicked()
    {
        clusts.PrevCluster();
        updateLevelButtons() ;
    }


    public void GoBackToMenu()
    {
        GameManager.Instance.SLoader.LoadMainMenuScene();
    }

    public void GoToNextLevelButton()
    {
        GameManager.Instance.SLoader.LoadLevelPlayScene(NextLevel);
    }


}
