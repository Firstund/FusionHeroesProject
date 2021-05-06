using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class SaveData
{
    public int currentStage = 1;
    public int gold = 100;
    // 업그레이드를 하지 않은 상태에선, 스탯 레벨은 0이다.
    public int[] unitHeartLev = new int[100];
    public int[] unitApLev = new int[100];
    public int[] unitDpLev = new int[100];
    public float[] heart = new float[100];
    public float[] ap = new float[100];
    public float[] dp = new float[100]; // index는 유닛 ID로, 건물의 경우 아군의 건물은 0, 적의 건물은 1로한다.
    public int plusMoneySpeedLev = 0;
}
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform unitSpawnPosition = null;
    [SerializeField]
    private Transform enemyUnitSpawnPosition = null;
    [SerializeField]
    private bool canTimeStop = true;
    [SerializeField]
    private bool canTimeDouble = true;
    private static GameManager instance;
    [SerializeField]
    private UnitScript[] _playerUnitPrefabs;
    public UnitScript[] playerUnitPrefabs
    {
        get{return _playerUnitPrefabs;}
        set{_playerUnitPrefabs = value;}
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
    private int plusMoney = 1;
    [SerializeField]
    private float _plusMoneyTime = 0.1f;
    public float plusMoneyTime
    {
        get{return _plusMoneyTime;}
        set{
            if(value >= 0f)
                _plusMoneyTime = value;
            }
    }
    [SerializeField]
    private float _minusPluseMoneyTimePerLev = 0.005f;
    public float minusPluseMoneyTimePerLev{
        get{return _minusPluseMoneyTimePerLev;}
        set{
            _minusPluseMoneyTimePerLev = value;
        }

    }
    private float soundValue = 1f;

    private bool canMoneyPlus = true;
    private bool mapSliderMoving = false;

    void Update()
    {
        if (canMoneyPlus)
            StartCoroutine(PlusMoney());
        TimeSet();

        goldText.text = saveData.gold + "";
    }
    private IEnumerator PlusMoney()
    {
        canMoneyPlus = false;

        yield return new WaitForSeconds(plusMoneyTime - (saveData.plusMoneySpeedLev * minusPluseMoneyTimePerLev));
        money += plusMoney;

        canMoneyPlus = true;
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
        if (!canTimeStop)
            Time.timeScale = 0;
        else if (!canTimeDouble) // canTimeDouble이 false일 때만 Time.timeScale = 0;이 작동한다. 왜지?
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
    public void SetMapSliderMoving(bool a)
    {
        mapSliderMoving = a;
    }
}
