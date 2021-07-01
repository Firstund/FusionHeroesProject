using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class SaveData
{
    [SerializeField]
    private int _curretnStage = 0;
    public int currentStage
    {
        get { return _curretnStage; }
        set { _curretnStage = value; }
    }
    [SerializeField]
    private int _maxReachedStage = 0;
    public int maxReachedStage
    {
        get { return _maxReachedStage; }
        set { _maxReachedStage = value; }
    }
    [SerializeField]
    private int _gold = 100;
    public int gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    // 업그레이드를 하지 않은 상태에선, 스탯 레벨은 0이다.
    [SerializeField]
    private int[] _unitHeartLev = new int[100];
    public int[] unitHeartLev
    {
        get { return _unitHeartLev; }
        set { _unitHeartLev = value; }
    }
    [SerializeField]
    private int[] _unitApLev = new int[100];
    public int[] unitApLev
    {
        get { return _unitApLev; }
        set { _unitApLev = value; }
    }
    [SerializeField]
    private int[] _unitDpLev = new int[100];
    public int[] unitDpLev
    {
        get { return _unitDpLev; }
        set { _unitDpLev = value; }
    }
    [SerializeField]
    private float[] _heart = new float[100];
    public float[] heart
    {
        get { return _heart; }
        set { _heart = value; }
    }
    [SerializeField]
    private float[] _ap = new float[100];
    public float[] ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    [SerializeField]
    private float[] _dp = new float[100]; // index는 유닛 ID로, 건물의 경우 아군의 건물은 0, 적의 건물은 1로한다.
    public float[] dp
    {
        get { return _dp; }
        set { _dp = value; }
    }
    [SerializeField]
    private int _plusMoneySpeedLev = 0;
    public int plusMoneySpeedLev
    {
        get { return _plusMoneySpeedLev; }
        set { _plusMoneySpeedLev = value; }
    }
    [SerializeField]
    private int _maxMoneyLev = 0;
    public int maxMoneyLev{
        get{return _maxMoneyLev;}
        set{_maxMoneyLev = value;}
    }
    [SerializeField]
    private int _maxStatLev = 10; // 유닛의 업그레이드 최대레벨
    public int maxStatLev
    {
        get { return _maxStatLev; }
        set { _maxStatLev = value; }
    }
    [SerializeField]
    private int _maxFusionLev = 5; // 유닛의 fusion 최대레벨
    public int maxFusionLev
    {
        get { return _maxFusionLev; }
        set { _maxFusionLev = value; }
    }
}
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform unitSpawnPosition = null;
    [SerializeField]
    private Transform enemyUnitSpawnPosition = null;
    [SerializeField]
    private Vector2 _mousePosition = Vector2.zero;
    public Vector2 mousePosition
    {
        get { return _mousePosition; }
        set { _mousePosition = value; }
    }
    [SerializeField]
    private bool canTimeStop = true;
    [SerializeField]
    private bool canTimeDouble = true;
    [SerializeField]
    private bool _canGetOutPopUpSpawn = true;
    public bool canGetOutPopUpSpawn
    {
        get { return _canGetOutPopUpSpawn; }
        set { _canGetOutPopUpSpawn = value; }
    }
    private static GameManager instance;
    private StageManager stageManager = null;
    private AdsManager adsManager = null;
    [SerializeField]
    private MapSliderScript mapSliderScript = null;

    [SerializeField]
    private GameObject gameOut = null;
    [SerializeField]
    private GameObject tutorial = null;

    private bool gameOutSpawned = false;
    [SerializeField]
    private UnitScript[] _playerUnitPrefabs;
    public UnitScript[] playerUnitPrefabs
    {
        get { return _playerUnitPrefabs; }
        set { _playerUnitPrefabs = value; }
    }
    [SerializeField]
    private SaveData saveData;
    [SerializeField]
    private Text goldText = null;
    [SerializeField]
    private float _dovalueTime = 0.4f;

    public float dovalueTime
    {
        get { return _dovalueTime; }
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject temp = new GameObject("GameManager");
                    instance = temp.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    private int money = 0;

    [SerializeField]
    private int _maxMoney = 100;
    public int maxMoney{
        get{return _maxMoney;}
        set{_maxMoney = value;}
    }
    private int firstMaxMoney = 0;
    [SerializeField]
    private float maxMoneyUp = 1.5f;
    [SerializeField]
    private int maxMoneyUpPerLev = 30;
    [SerializeField]
    private int _maxPlusMoneySpeedLev = 10; // 업그레이드패널을 이용한 돈모으기 속도의 최대레벨
    public int maxPlusMoneySpeedLev
    {
        get { return _maxPlusMoneySpeedLev; }
        set { _maxPlusMoneySpeedLev = value; }
    }

    private int _hadMoney = 0; // 처음부터 끝까지 money를 하나도 안썼을 경우의 money의 양
    public int hadMoney
    {
        get { return _hadMoney; }
        set { _hadMoney = value; }
    }
    private int plusMoney = 1;
    [SerializeField]
    private float _plusMoneyTime = 0.1f;
    [SerializeField]
    private float totalPlusMoneyTime = 0f;
    public float plusMoneyTime
    {
        get { return _plusMoneyTime; }
        set
        {
            if (value >= 0f)
                _plusMoneyTime = value;
        }
    }
    [SerializeField]
    private int _getMoneyUpgradeLev = 0;
    public int getMoneyUpgradeLev
    {
        get{return _getMoneyUpgradeLev;}
        set{_getMoneyUpgradeLev = value;}
    }
    [SerializeField]
    private float _minusPlusMoneyTimePerUpgrade = 0.005f;
    public float minusPlusMoneyTimePerUpgrade{
        get{return _minusPlusMoneyTimePerUpgrade;}
        set{_minusPlusMoneyTimePerUpgrade = value;}
    }
    [SerializeField]
    private int _maxGetMoneyUpgradeLev = 8;
    public int maxGetMoneyUpgradeLev
    {
        get{return _maxGetMoneyUpgradeLev;}
    }
    [SerializeField]
    private int _getMoneyUpgradeCost = 20;
    public int getMoneyUpgradeCost
    {
        get{return _getMoneyUpgradeCost;}
        set{_getMoneyUpgradeCost = value;}
    }
    private int firstGetMoneyUpgradeCost = 0;
    [SerializeField]
    private float getMoneyUpgradeCostUp = 1.5f;
    

    [SerializeField]
    private float _minusPluseMoneyTimePerLev = 0.005f;
    public float minusPluseMoneyTimePerLev
    {
        get { return _minusPluseMoneyTimePerLev; }
        set
        {
            _minusPluseMoneyTimePerLev = value;
        }

    }
    private float soundValue = 1f;

    private bool canMoneyPlus = true;
    [SerializeField]
    private bool mapSliderMoving = false;
    private bool uiClicked = false;
    private bool _popUpIsSpawned = false;
    public bool popUpIsSpawned
    {
        get { return _popUpIsSpawned; }
        set { _popUpIsSpawned = value; }
    }
    private bool _tutoIsPlaying = false;
    public bool tutoIsPlaying
    {
        get { return _tutoIsPlaying; }
        set { _tutoIsPlaying = value; }
    }
    void Start()
    {
        adsManager = FindObjectOfType<AdsManager>();
        stageManager = FindObjectOfType<StageManager>();
        firstGetMoneyUpgradeCost = getMoneyUpgradeCost;
        firstMaxMoney = maxMoney;

        if (saveData.currentStage <= 0)
        {
            SpawnTutorial();
        }
        
    }

    void Update()
    {
        if (canMoneyPlus)
            StartCoroutine(PlusMoney());
        TimeSet();
        CheckSpawnGetOut();
        SetMaxMoney();

        goldText.text = saveData.gold + "";
    }

    private void SetMaxMoney()
    {
        int _maxMoney = 0;
        _maxMoney = firstMaxMoney + (maxMoneyUpPerLev * saveData.maxMoneyLev);

        for (int i = 0; i < getMoneyUpgradeLev; i++)
            _maxMoney = (int)(_maxMoney * maxMoneyUp);

        maxMoney = _maxMoney;
    }

    public void SpawnTutorial()
    {
        Instantiate(tutorial);
    }
    public void OnClickMinusPlusMoneyTimePerUpgradeNutton()
    {
        if(money >= getMoneyUpgradeCost && getMoneyUpgradeLev + 1 < maxGetMoneyUpgradeLev)
        {
            money -= getMoneyUpgradeCost;
            getMoneyUpgradeLev++;
            getMoneyUpgradeCost = (int)(getMoneyUpgradeCostUp * getMoneyUpgradeCost);
        }
        else if(getMoneyUpgradeLev + 1 >= maxGetMoneyUpgradeLev)
        {
            Instantiate(stageManager.maxLevelText, stageManager.textSpawnPosition);
        }
        else
        {
            Instantiate(stageManager.notEnoughMoneyText, stageManager.textSpawnPosition);
        }
        
    }
    public void Reset()
    {
        getMoneyUpgradeCost = firstGetMoneyUpgradeCost;
        maxMoney = firstMaxMoney;
        getMoneyUpgradeLev = 0;
        money = 0;
        hadMoney = 0;
        FindObjectOfType<GetMoneySpeedUpBtnScript>().SetText();
    }

    private void CheckSpawnGetOut()
    {
        if (canGetOutPopUpSpawn)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!gameOutSpawned)
                    gameOut.SetActive(true);
                else
                    gameOut.SetActive(false);

                gameOutSpawned = !gameOutSpawned;
            }
        }
    }

    private IEnumerator PlusMoney()
    {
        if (!tutoIsPlaying)
        {
            canMoneyPlus = false;

            totalPlusMoneyTime = ((plusMoneyTime - saveData.plusMoneySpeedLev * minusPluseMoneyTimePerLev) / (getMoneyUpgradeLev + 1) * minusPlusMoneyTimePerUpgrade);
            yield return new WaitForSeconds(totalPlusMoneyTime);

            money += plusMoney;
            hadMoney += plusMoney;
            money = Mathf.Clamp(money, 0, maxMoney);
            canMoneyPlus = true;
        }

    }
    public int GetMoney()
    {
        return money;
    }
    public void SetMoney(int a)
    {
        money = a;
    }
    public int GetGold()
    {
        return saveData.gold;
    }
    public void SetGold(int a)
    {
        saveData.gold = a;
    }
    public SaveData GetSaveData()
    {
        return saveData;
    }
    public void SetSaveData(SaveData a)
    {
        saveData = a;
    }
    public bool GetCST()
    {
        return canTimeStop;
    }
    public bool GetCDT()
    {
        return canTimeDouble;
    }
    public void SetCSt(bool a)
    {
        mapSliderScript.ReSet();
        canTimeStop = a;
    }
    public void SetCDT(bool a)
    {
        canTimeDouble = a;
    }
    public void SetSoundValue(float a)
    {
        soundValue = a;
    }
    public float GetSoundValue()
    {
        return soundValue;
    }
    private void TimeSet()
    {
        if (!canTimeStop || adsManager.adsIsShowing)
            Time.timeScale = 0;
        else if (!canTimeDouble)
            Time.timeScale = 2;
        else
            Time.timeScale = 1;
    }
    public Transform GetUnitSpawnPosition()
    {
        return unitSpawnPosition;
    }
    public Transform GetEnemyUnitSpawnPosition()
    {
        return enemyUnitSpawnPosition;
    }
    public bool GetMapSliderMoving()
    {
        return mapSliderMoving;
    }
    public bool GetUiClicked()
    {
        return uiClicked;
    }
    public void SetMapSliderMoving(bool a)
    {
        mapSliderMoving = a;
    }
    public void SetUiClicked(bool a)
    {
        uiClicked = a;
    }
}
