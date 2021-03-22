﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnScript : MonoBehaviour
{
    GameManager gameManager = null;

    [SerializeField]
    private GameObject spawnThis = null;
    private Transform spawnPosition = null;
    [SerializeField]
    private int spawnMoney = 5;
    void Start()
    {
        gameManager = GameManager.Instance;
        spawnPosition = gameManager.GetUnitSpawnPosition();
    }
   public void SpawnUnit()
    {
        if (gameManager.GetMoney() >= spawnMoney)
        {
            int money = gameManager.GetMoney() - spawnMoney;
            gameManager.SetMoney(money);

            Instantiate(spawnThis, spawnPosition);
        }
    }
}