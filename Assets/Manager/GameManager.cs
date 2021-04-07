using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int roundNum = 1;
    public int money = 20;
    public int plusMoney = 1;
    public float plusMoenyTime = 1f; // 저장데이터, 후에 적용
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

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    GameObject temp = new GameObject("GameManager");
                    instance = temp.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    private int money = 200;
    private int plusMoney = 1;
    private float plusMoenyTime = 1f;
    private float soundValue = 0f;

    private bool canMoneyPlus = true;
    private bool mapSliderMoving = false;

    void Update()
    {
        if(canMoneyPlus)
        StartCoroutine(PlusMoney());
        TimeSet();
    }
    private IEnumerator PlusMoney()
    {
        canMoneyPlus = false;

        yield return new WaitForSeconds(plusMoenyTime);
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
        else if(!canTimeDouble)
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
