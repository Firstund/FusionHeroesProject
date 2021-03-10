using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool canTimeStop = true;
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
    private int money = 10;
    private int plusMoney = 1;
    private float plusMoenyTime = 1f;

    private bool canMoneyPlus = true;

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
    public void SetCSt(bool a)
    {
        canTimeStop = a;
    }
    private void TimeSet()
    {
        if (!canTimeStop)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
