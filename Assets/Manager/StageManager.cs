using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private FusionManager fusionManager = null;
    private GameManager gameManager = null;
    private DataManager dataManager = null;
    private AudioSource audi = null;
    private SaveData saveData;
    [SerializeField]
    private GameObject stageClearPopUp = null;
    [SerializeField]
    private StageClearScript stageClearScript = null;
    
    [SerializeField]
    private Text stageText = null;
    [SerializeField]
    private int currentStage = 1;

    void Start()
    {
        gameManager = GameManager.Instance;
        dataManager = DataManager.Instance;
        audi = GetComponent<AudioSource>();
        saveData = gameManager.GetSaveData();
        
    }
    void Update()
    {
        if(saveData != gameManager.GetSaveData())
        {
            saveData = gameManager.GetSaveData();
        }

        currentStage = saveData.currentStage;
        stageText.text = "CurrentStage: " + currentStage;
        audi.volume = gameManager.GetSoundValue();
    }
    public void StageClear(bool a)
    {
        stageClearScript.SetGameClear(a);
        stageClearPopUp.SetActive(true);
    }
    public void StageReset()
    {
        stageClearScript.OnNextStage();
        stageClearPopUp.SetActive(false);
        gameManager.SetCSt(true);

        gameManager.SetMoney(0);

        fusionManager.enemyBuildingScript.Reset();
        fusionManager.buildingScript.Reset();

        saveData.maxStatLev = 10 + 10 * (currentStage / 10);

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

        dataManager.SaveGameData();

    }
    public void SetCurrentStage(int a)
    {
        saveData.currentStage = a;
    }
    public int GetCurrentStage()
    {
        return saveData.currentStage;
    }

}
