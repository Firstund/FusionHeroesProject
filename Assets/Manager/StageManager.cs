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
    private Transform _textSpawnPosition = null;
    public Transform textSpawnPosition
    {
        get{return _textSpawnPosition;}
    }
    [SerializeField]
    private GameObject _notEnoughMoneyText = null;
    public GameObject notEnoughMoneyText
    {
        get{return _notEnoughMoneyText;}
    }
    [SerializeField]
    private GameObject _saveDoneText = null;
    public GameObject saveDoneText
    {
        get{return _saveDoneText;}
    }
    [SerializeField]
    private StageClearScript stageClearScript = null;
    
    [SerializeField]
    private Text stageText = null;
    [SerializeField]
    private int currentStage = 1;
    [SerializeField]
    private int _useMoneyNum = 0;
    public int useMoneyNum{
        get{return _useMoneyNum;}
        set{_useMoneyNum = value;}
    }
    [SerializeField]
    private int _deathPlayerUnitNum = 0;
    public int deathPlayerUnitNum
    {
        get{return _deathPlayerUnitNum;}
        set{_deathPlayerUnitNum = value;}
    }
    [SerializeField]
    private int _killedEnemyUnitNum = 0;
    public int killedEnemyUnitNum
    {
        get{return _killedEnemyUnitNum;}
        set{_killedEnemyUnitNum = value;}
    }
    [SerializeField]
    private double _t = 0; // 여기에 Time.deltaTime을 곱하면 지난 시간이 나온다.
    public double t 
    {
        get{return  _t;}
        set{_t = value;}
    }
    
    void Start()
    {
        gameManager = GameManager.Instance;
        dataManager = DataManager.Instance;
        audi = GetComponent<AudioSource>();
        saveData = gameManager.GetSaveData();

        saveData.maxStatLev = 5 + 5 * (saveData.maxReachedStage / 10);

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

        if(gameManager.GetCST())
            t += Time.deltaTime;
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

        useMoneyNum = 0;
        deathPlayerUnitNum = 0;
        killedEnemyUnitNum = 0;

        saveData.maxStatLev = 5 + 5 * (saveData.maxReachedStage / 10);

        for (int i = 0; i < fusionManager.unitScript.Length; i++)
        {
            Destroy(fusionManager.unitScript[i].gameObject);
        }

        fusionManager.unitScript = new UnitScript[0];
        fusionManager.SetUnitNum(1);
        for (int i = 0; i < fusionManager.enemyScript.Length; i++)
        {
            Destroy(fusionManager.enemyScript[i].gameObject);
        }
        fusionManager.enemyScript = new EnemyScript[0];
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
