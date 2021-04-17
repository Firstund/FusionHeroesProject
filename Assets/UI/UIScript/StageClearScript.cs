using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearScript : PopUpScaleScript
{
    [SerializeField]
    private StageManager stageManager;
    [SerializeField]
    private Text clearText = null;
    [SerializeField]
    private Text plusGoldText = null;
    private SaveData saveData;
    private bool gameClear = false;
    private int plusGold = 1000;
 

    void Start()
    {
        PlusStart();
        saveData = gameManager.GetSaveData();
        SetText();
    }
    void Update()
    {
        SetScale();
    }
    private void SetText()
    {
        if(gameClear)
        {
            clearText.text = "Stage" + stageManager.GetCurrentStage() + " Clear!";
            plusGoldText.text = "+" + plusGold * (stageManager.GetCurrentStage() / 10 + 1);

            
            
            
        }
        else
        {
            clearText.text = "GameOver";
        }
    }
    public void SetGameClear(bool a)
    {
        gameClear = a;
    }
    public void OnNextStage()
    {
        saveData.gold += plusGold * (stageManager.GetCurrentStage() / 10 + 1); 

        int a = stageManager.GetCurrentStage();
        stageManager.SetCurrentStage(a + 1);
    }
    
}
