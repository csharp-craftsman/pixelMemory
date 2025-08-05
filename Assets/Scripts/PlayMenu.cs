using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{

    Canvas winCanvas;
    Canvas loseCanvas;
    bool isPlayerWon;

    int LosingDurationTextValue;
    int PreviewDurationTextValue;
    public TextMeshProUGUI losingDurText;
    public TextMeshProUGUI previewDurText;

    // Start is called before the first frame update
    void Start()
    {
        winCanvas = GameObject.Find("WinCanvas").GetComponent<Canvas>();
        loseCanvas = GameObject.Find("LoseCanvas").GetComponent <Canvas>();

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


    public void GoToLevelSelection()
    {
        if (isPlayerWon)
            GameManager.Instance.SaveIfNextLevelWon();
        
        GameManager.Instance.SLoader.LoadLevelSelectionScene();

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
