using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpScript : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private FusionManager fusionManager = null;

    [SerializeField]
    private string scriptName = ""; // 실행시킬 this내의 함수의 이름
    [SerializeField]
    private string statName = ""; // 업그레이드시킬 스탯의 이름
    private SaveData saveData; // 스탯 레벨을 저장하는 용도의 saveData
    [SerializeField]
    private int upgradeCost = 100; // 해당 스탯의 레벨을 올릴 때 사용해야하는 gold의 양
    private int firstUpgradeCost = 0;
    [SerializeField]
    private float upgradeStat = 100f; // 해당 스탯의 레벨을 올렸을 때, 올라갈 스탯의 양

    private void Start()
    {
        gameManager = GameManager.Instance;
        saveData = gameManager.GetSaveData();

        firstUpgradeCost = upgradeCost;
    }
    private void Update()
    {
        if (saveData != gameManager.GetSaveData())
        {
            saveData = gameManager.GetSaveData();
        }
    }
    public void OnClick()
    {
        this.SendMessage(scriptName, SendMessageOptions.DontRequireReceiver);
    }
    private void BuildingUpgrade() // 이 함수 내에서 BuildingUpgrade를 수행
    {
        if(fusionManager == null)
        {
            fusionManager = FindObjectOfType<FusionManager>();
        }
        BuildingScript buildingScript = fusionManager.buildingScript;
        switch (statName)
        {
            case "he":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[0];
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.buildingStatLev[0]++;
                    buildingScript.setStat();

                    upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[0];
                }
                break;
            case "dp":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[1];
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.buildingStatLev[1]++;
                    buildingScript.setStat();

                    upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.buildingStatLev[1];
                }
                break;
            default:
                Debug.LogError($"{statName} is Disappeared");
                break;
        }
        gameManager.SetSaveData(saveData);
    }
}
