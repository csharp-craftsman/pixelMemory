using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{

    Canvas winCanvas;
    Canvas loseCanvas;
    bool isPlayerWon;

    int LosingDurationTextValue;
    int PreviewDurationTextValue;
    public TextMeshProUGUI losingDurText;
    public TextMeshProUGUI previewDurText;

    Button retryButton;

    // Start is called before the first frame update
    void Start()
    {
        winCanvas = GameObject.Find("WinCanvas").GetComponent<Canvas>();
        loseCanvas = GameObject.Find("LoseCanvas").GetComponent <Canvas>();
        retryButton = GameObject.Find("RetryButton").GetComponent<Button>();

        // if heart 1 and player lost then player cant retry play the same level.
        Heart h = GameManager.Instance.HController.heart;
        if (h.value == 1)
            retryButton.interactable = false;

        losingDurText.enabled = false;
    }


    public void PlayerWon()
    {
        isPlayerWon = !loseCanvas.enabled;
        if (isPlayerWon)
            winCanvas.enabled = true;

    }

    public void PlayerLost()
    {
        loseCanvas.enabled = true;
    }


    public void OnRetryClicked()
    {
        GameManager.Instance.SaveIfLost();
        GameManager.Instance.SLoader.ReloadCurrentScene();
        
        

    }


    public void OnNextLevelClicked()
    {
        GameManager.Instance.SaveIfNextLevelWon();
        GameManager.Instance.SLoader.LoadLevelPlayScene(GameManager.Instance.saveData.nextPlayableLevel);
    }


    public void GoToMainMenuScene()
    {   
        GameManager.Instance.SLoader.LoadMainMenuScene();
    }

    public void UpdateLosingDuration(float remainingSecs)
    {
        if (losingDurText.enabled)
        {
            LosingDurationTextValue = Mathf.FloorToInt(remainingSecs + 0.5f);
            losingDurText.text = LosingDurationTextValue.ToString();
        }
    }

    public void EnableLosingDurationText(){
        losingDurText.enabled = true;
    }


    public void UpdatePreviewDuration(float remainingSecs) { 
        PreviewDurationTextValue = Mathf.FloorToInt(remainingSecs + 0.5f);
        previewDurText.text= PreviewDurationTextValue.ToString();
    }

    public void DisablePreviewDurationText(){ 
        previewDurText.enabled = false; 
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
