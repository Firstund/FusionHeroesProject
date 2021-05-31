using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMoneySpeedUpBtnScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private Text upgradeCostText = null;
    private Text upgradeCostLev = null;
    void Start()
    {
        gameManager = GameManager.Instance;

        upgradeCostText = transform.GetChild(1).GetComponent<Text>();
        upgradeCostLev = transform.GetChild(2).GetComponent<Text>();

        SetText();
    }

    public void OnClick()
    {
        gameManager.OnClickMinusPlusMoneyTimePerUpgradeNutton();

        SetText();
    }
    public void SetText()
    {
        upgradeCostText.text = gameManager.getMoneyUpgradeCost + "";
        upgradeCostLev.text = "Lv." + (gameManager.getMoneyUpgradeLev + 1) + "";
    }
}
