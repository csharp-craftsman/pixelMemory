using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{

    public int nextLevel = 1;

    public void GoToNextLevelButton()
    {
        GameManager.Instance.SLoader.LoadLevelPlayScene(nextLevel);
    }


}
