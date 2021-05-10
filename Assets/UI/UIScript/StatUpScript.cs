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
    private UnitScript currentUnit;

    [SerializeField]
    private string statName = ""; // 업그레이드시킬 스탯의 이름
    private SaveData saveData; // 스탯 레벨을 저장하는 용도의 saveData
    [SerializeField]
    private int upgradeCost = 100; // 해당 스탯의 레벨을 올릴 때 사용해야하는 gold의 양
    private int firstUpgradeCost = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;

        foreach (UnitScript item in gameManager.playerUnitPrefabs)
        {
            if (item.GetUnitID() == unitId)
                currentUnit = item;
        }

        saveData = gameManager.GetSaveData();

        if (currentUnit != null)
        {
            unitStatIndex = currentUnit.unitStatIndex;
        }
        else
        {
            unitStatIndex = 0;
        }

        MaxCheck();
        SetStat();

        firstUpgradeCost = upgradeCost;
    }

    private void SetStat()
    {
        if (currentUnit != null)
        {
            saveData.heart[unitStatIndex] = currentUnit.heart + saveData.unitHeartLev[unitStatIndex] * currentUnit.heartUpPerLev;
            saveData.ap[unitStatIndex] = currentUnit.ap + saveData.unitApLev[unitStatIndex] * currentUnit.apUpPerLev;
            saveData.dp[unitStatIndex] = currentUnit.dp + saveData.unitDpLev[unitStatIndex] * currentUnit.dpUpPerLev;
        }
    }

    private void MaxCheck()
    {
        if (saveData.unitHeartLev[unitStatIndex] > saveData.maxStatLev)
        {
            saveData.unitHeartLev[unitStatIndex] = saveData.maxStatLev;
        }
        if (saveData.unitApLev[unitStatIndex] > saveData.maxStatLev)
        {
            saveData.unitApLev[unitStatIndex] = saveData.maxStatLev;
        }
        if (saveData.unitDpLev[unitStatIndex] > saveData.maxStatLev)
        {
            saveData.unitDpLev[unitStatIndex] = saveData.maxStatLev;
        }
        if (saveData.plusMoneySpeedLev > saveData.maxPlusMoneySpeedLev)
        {
            saveData.plusMoneySpeedLev = saveData.maxPlusMoneySpeedLev;
        }
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
                SetTexts(saveData.unitHeartLev[unitStatIndex], saveData.heart[unitStatIndex], true, "현재 체력:");
                break;
            case "ap":
                SetTexts(saveData.unitApLev[unitStatIndex], saveData.ap[unitStatIndex], true, "현재 공격력:");
                break;
            case "dp":
                SetTexts(saveData.unitDpLev[unitStatIndex], saveData.dp[unitStatIndex], true, "현재 방어력:");
                break;
            case "fusionLev":
                SetTexts(saveData.maxFusionLev, 0, false, "");
                break;
            case "plusMoney":
                SetTexts(saveData.plusMoneySpeedLev, gameManager.minusPluseMoneyTimePerLev, true, "현재 속도:");
                break;

            default:
                Debug.LogError($"{statName} is Disappeared");
                enabled = false;
                break;
        }

    }

    private void SetTexts(int unitStatLev, float unitStat, bool hasThirdText, string thirdText)
    {
        upgradeCost = firstUpgradeCost + (firstUpgradeCost / 2) * unitStatLev;
        texts[0].text = "현재 레벨: " + unitStatLev;
        texts[1].text = "필요한 골드: " + upgradeCost;

        if (hasThirdText)
            texts[2].text = thirdText + " " + unitStat;
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
                if (currentUnit != null)
                    saveData.unitHeartLev[unitStatIndex] = UpgradeStat(saveData.unitHeartLev[unitStatIndex], ref saveData.heart[unitStatIndex], currentUnit.heart, currentUnit.heartUpPerLev);
                else
                    saveData.unitHeartLev[unitStatIndex] = UpgradeStat(saveData.unitHeartLev[unitStatIndex], ref saveData.heart[unitStatIndex]);
                break;
            case "ap":
                if (currentUnit != null)
                    saveData.unitApLev[unitStatIndex] = UpgradeStat(saveData.unitApLev[unitStatIndex], ref saveData.ap[unitStatIndex], currentUnit.ap, currentUnit.apUpPerLev);
                else
                    saveData.unitApLev[unitStatIndex] = UpgradeStat(saveData.unitApLev[unitStatIndex], ref saveData.ap[unitStatIndex]);
                break;
            case "dp":
                if (currentUnit != null)
                    saveData.unitDpLev[unitStatIndex] = UpgradeStat(saveData.unitDpLev[unitStatIndex], ref saveData.dp[unitStatIndex], currentUnit.dp, currentUnit.dpUpPerLev);
                else
                    saveData.unitDpLev[unitStatIndex] = UpgradeStat(saveData.unitDpLev[unitStatIndex], ref saveData.dp[unitStatIndex]);
                break;
            case "fusionLev":
                saveData.maxFusionLev = UpgradeStat(saveData.maxFusionLev);
                break;
            case "plusMoney":
                saveData.plusMoneySpeedLev = UpgradeStat(saveData.plusMoneySpeedLev);
                break;
            default:
                Debug.LogError($"{statName} is Disappeared");
                break;
        }

        gameManager.SetSaveData(saveData);
    }

    private int UpgradeStat(int unitStatLev, ref float unitStat, float currentUnitStat, float currentUnitStatUpPerLev)
    {
        if (saveData.gold >= upgradeCost && unitStatLev < saveData.maxStatLev)
        {
            saveData.gold -= upgradeCost;
            unitStatLev++;
            if (currentUnit != null)
                unitStat = currentUnitStat + unitStatLev * currentUnitStatUpPerLev;
        }
        else if (unitStatLev > saveData.maxStatLev)
        {
            unitStatLev = saveData.maxStatLev;
        }

        return unitStatLev;
    }
    private int UpgradeStat(int unitStatLev, ref float unitStat)
    {
        if (saveData.gold >= upgradeCost && unitStatLev < saveData.maxStatLev)
        {
            saveData.gold -= upgradeCost;
            unitStatLev++;
        }
        else if (unitStatLev > saveData.maxStatLev)
        {
            unitStatLev = saveData.maxStatLev;
        }

        return unitStatLev;
    }
    private int UpgradeStat(int unitStatLev)
    {
        if (saveData.gold >= upgradeCost && unitStatLev < saveData.maxStatLev)
        {
            saveData.gold -= upgradeCost;
            unitStatLev++;
        }
        else if (unitStatLev > saveData.maxStatLev)
        {
            unitStatLev = saveData.maxStatLev;
        }

        return unitStatLev;
    }
}
