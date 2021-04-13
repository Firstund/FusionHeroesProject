using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private FusionManager fusionManager = null;
    private GameManager gameManager = null;
    [SerializeField]
    private GameObject stageClearPopUp = null;

    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {

    }
    public void StageClear(bool a)
    {
        Debug.Log("Clear?");
        if (a)
        {
            // 스테이지 클리어
            stageClearPopUp.SetActive(true);
        }
        else
        {
            // 스테이지 클리어 실패
            stageClearPopUp.SetActive(true);
        }
    }
    public void StageReset()
    {
        stageClearPopUp.SetActive(false);
        gameManager.SetCSt(true);

        gameManager.SetMoney(0);

        fusionManager.enemyBuildingScript.Reset();
        fusionManager.buildingScript.Reset();

        for (int i = 0; i < fusionManager.unitScript.Length; i++)
        {
            Debug.Log(fusionManager.unitScript.Length);
            Destroy(fusionManager.unitScript[i].gameObject);
        }
        fusionManager.SetUnitNum(1);
        for (int i = 0; i < fusionManager.enemyScript.Length; i++)
        {
            Destroy(fusionManager.enemyScript[i].gameObject);
        }
        fusionManager.SetEnemyUnitNum(1);


    }
}
