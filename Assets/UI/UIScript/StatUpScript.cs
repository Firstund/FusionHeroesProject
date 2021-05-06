using UnityEngine;
using UnityEngine.UI;

public class StatUpScript : MonoBehaviour
{
    private GameManager gameManager = null;

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
    private string statName = ""; // 업그레이드시킬 스탯의 이름
    private SaveData saveData; // 스탯 레벨을 저장하는 용도의 saveData
    [SerializeField]
    private int upgradeCost = 100; // 해당 스탯의 레벨을 올릴 때 사용해야하는 gold의 양
    private int firstUpgradeCost = 0;

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

        switch (statName)
        {
            case "he":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.unitHeartLev[unitStatIndex];
                texts[0].text = "현재 레벨: " + saveData.unitHeartLev[unitStatIndex];
                texts[1].text = "필요한 골드: " + upgradeCost;
                texts[2].text = "현재 체력: " + saveData.heart[unitStatIndex];
                break;
            case "ap":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.unitApLev[unitStatIndex];
                texts[0].text = "현재 레벨: " + saveData.unitApLev[unitStatIndex];
                texts[1].text = "필요한 골드: " + upgradeCost;
                texts[2].text = "현재 공격력: " + saveData.ap[unitStatIndex];
                break;
            case "dp":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.unitDpLev[unitStatIndex];
                texts[0].text = "현재 레벨: " + saveData.unitDpLev[unitStatIndex];
                texts[1].text = "필요한 골드: " + upgradeCost;
                texts[2].text = "현재 방어력: " + saveData.dp[unitStatIndex];
                break;
            case "plusMoney":
                upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * saveData.plusMoneySpeedLev;
                texts[0].text = "현재 레벨: " + saveData.plusMoneySpeedLev;
                texts[1].text = "필요한 골드: " + upgradeCost;
                texts[2].text = "현재 속도: " + (gameManager.plusMoneyTime - gameManager.minusPluseMoneyTimePerLev * saveData.plusMoneySpeedLev);
                break;
            default:
                Debug.LogError($"{statName} is Disappeared");
                enabled = false;
                break;
        }

    }
    public void OnClick()
    {
        Upgrade();
    }
    private void Upgrade() // 이 함수 내에서 BuildingUpgrade를 수행
    {
        switch (statName)
        {
            case "he":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.unitHeartLev[unitStatIndex]++;
                }
                break;
            case "ap":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.unitApLev[unitStatIndex]++;
                }
                break;
            case "dp":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.unitDpLev[unitStatIndex]++;
                }
                break;
            case "plusMoney":
                if (saveData.gold >= upgradeCost)
                {
                    saveData.gold -= upgradeCost;
                    saveData.plusMoneySpeedLev++;
                }
                break;
            default:
                Debug.LogError($"{statName} is Disappeared");
                break;
        }
        gameManager.SetSaveData(saveData);
    }
}
