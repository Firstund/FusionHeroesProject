using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxLevelTextScript : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    private int maxReachedStage = 0;
    private int nextUpgradeStage = 0;

    void Update()
    {
        maxReachedStage = GameManager.Instance.GetSaveData().maxReachedStage;
        nextUpgradeStage = (maxReachedStage) / 10 * 10 + 10;

        text.text = "이미 최대레벨입니다! " + nextUpgradeStage +"스테이지부터 새로운 레벨이 해금됩니다.";

    }
}
