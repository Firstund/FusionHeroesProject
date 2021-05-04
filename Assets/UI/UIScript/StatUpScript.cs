using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpScript : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private FusionManager fusionManager = null;
    [SerializeField]
    private Text[] texts;
    // 0: CurrentStatLev
    // 1: StatUp
    // 2: LevUpCost
    // 3: CurrentStat
    [SerializeField]
    private int unitId = 0; // 스탯을 볼 유닛의 ID를 지정
                            // 플레이어 건물은 0, 적 건물은 1로 지정
    private int unitStatIndex;

    [SerializeField]
    private string scriptName = ""; // 실행시킬 this내의 함수의 이름
    [SerializeField]
    private string statName = ""; // 업그레이드시킬 스탯의 이름
    private SaveData saveData; // 스탯 레벨을 저장하는 용도의 saveData
    [SerializeField]
    private int upgradeCost = 100; // 해당 스탯의 레벨을 올릴 때 사용해야하는 gold의 양
    private int firstUpgradeCost = 0;
    [SerializeField]
    private float _upgradeStat = 100f; // 해당 스탯의 레벨을 올렸을 때, 올라갈 스탯의 양
    public float upgradeStat
    {
        get { return _upgradeStat; }
        set { _upgradeStat = value; }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        saveData = gameManager.GetSaveData();

        unitStatIndex = (unitId / 50 + unitId % 100);

        firstUpgradeCost = upgradeCost;
    }
    private void Update()
    {
        if (saveData != gameManager.GetSaveData())
        {
            saveData = gameManager.GetSaveData();
        }

        switch (scriptName)
        {
            case "BuildingUpgrade":
                switch (statName)
                {
                    case "he":
                        upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[0];
                        texts[0].text = "현재 레벨: " + saveData.buildingStatLev[0];
                        texts[1].text = "필요한 골드: " + upgradeCost;
                        texts[2].text = "현재 능력치: " + saveData.heart[unitStatIndex];
                        break;
                    case "dp":
                        upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[1];
                        texts[0].text = "현재 레벨: " + saveData.buildingStatLev[1];
                        texts[1].text = "필요한 골드: " + upgradeCost;
                        texts[2].text = "현재 능력치: " + saveData.dp[unitStatIndex]; 
                        break;
                }
                break;
        }


    }
    public void OnClick()
    {
        this.SendMessage(scriptName, SendMessageOptions.DontRequireReceiver);
    }
    private void BuildingUpgrade() // 이 함수 내에서 BuildingUpgrade를 수행
    {
        if (fusionManager == null)
        {
            fusionManager = FindObjectOfType<FusionManager>();
        }
        BuildingScript buildingScript = fusionManager.buildingScript;
        switch (statName)
        {
            case "he":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.buildingStatLev[0]++;
                    buildingScript.setStat();
                }
                break;
            case "dp":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.buildingStatLev[1]++;
                    buildingScript.setStat();
                }
                break;
            default:
                Debug.LogError($"{statName} is Disappeared");
                break;
        }
        gameManager.SetSaveData(saveData);
    }
}
