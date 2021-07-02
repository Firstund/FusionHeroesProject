using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearScript : PopUpScaleScript
{
    [SerializeField]
    private StageManager stageManager;
    [SerializeField]
    private GameObject doubleGoldBtn = null;
    [SerializeField]
    private Text clearText = null;
    [SerializeField]
    private Text plusGoldText = null;
    [SerializeField]
    private Text clearTimeText = null;
    [SerializeField]
    private Text useMoneyText = null;
    [SerializeField]
    private Text deathPlayerUnitText = null;
    [SerializeField]
    private Text killedEnemyUnitText = null;
    private SaveData saveData;
    private bool gameClear = false;
    [SerializeField]
    private int plusGold = 1000;

    private bool getGoldDouble = false;


    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();

        PlusStart();
        saveData = gameManager.GetSaveData();
        SetText();
    }
    void Update()
    {
        SetScale();

        if (gameManager.GetCST())
            SetText();

        if (saveData != gameManager.GetSaveData())
        {
            saveData = gameManager.GetSaveData();
        }
    }
    private void SetText()
    {
        clearTimeText.text = "걸린 시간: " + (long)stageManager.t + "초";
        useMoneyText.text = "사용한 Money: " + (gameManager.hadMoney - gameManager.GetMoney());
        killedEnemyUnitText.text = "죽인 유닛의 수: " + stageManager.killedEnemyUnitNum;
        deathPlayerUnitText.text = "죽은 유닛의 수: " + stageManager.deathPlayerUnitNum;

        if (gameClear)
        {
            if (saveData.currentStage <= 0)
            {
                clearText.text = "TutorialStage Clear!";
            }
            else
            {
                clearText.text = "Stage" + stageManager.GetCurrentStage() + " Clear!";
            }

            plusGoldText.text = "+" + (plusGold + (stageManager.GetCurrentStage() * (plusGold / 2)));
            
            doubleGoldBtn.SetActive(true);
        }
        else
        {
            clearText.text = "GameOver";
            plusGoldText.text = "+0";
            doubleGoldBtn.SetActive(false);
        }
    }
    public void SetGameClear(bool a)
    {
        gameClear = a;
    }
    public void OnNextStage()
    {
        if (gameClear)
        {
            if (!getGoldDouble)
            {
                saveData.gold += (plusGold + (stageManager.GetCurrentStage() * (plusGold / 2)));
            }
            else if (getGoldDouble)
            {
                saveData.gold += (plusGold + (stageManager.GetCurrentStage() * (plusGold / 2))) * 2;
            }


            int a = stageManager.GetCurrentStage();
            stageManager.SetCurrentStage(a + 1);

            if (stageManager.GetCurrentStage() > saveData.maxReachedStage)
            {
                saveData.maxReachedStage = stageManager.GetCurrentStage();
            }

            getGoldDouble = false;
        }
    }

    public void PlusGoldDouble()
    {
        getGoldDouble = true;
        plusGoldText.text = "+" + (plusGold + (stageManager.GetCurrentStage() * (plusGold / 2))) * 2;

    }
}
