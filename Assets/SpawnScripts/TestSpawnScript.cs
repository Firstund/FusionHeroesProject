using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSpawnScript : MonoBehaviour
{
    GameManager gameManager = null;

    [SerializeField]
    private GameObject spawnThis = null;
    private Transform spawnPosition = null;
    [SerializeField]
    private Image respawnImage = null;
    [SerializeField]
    private int spawnMoney = 5;
    [SerializeField]
    private float spawnSpeed = 3f;
    private float nextTimeToSpawn = 0f;
    private float respawnCurTime = 0f;
    private float respawnMaxTime = 0f;
    [SerializeField]
    private bool canSpawnAgain = true;
    void Start()
    {
        gameManager = GameManager.Instance;
        spawnPosition = gameManager.GetUnitSpawnPosition();
    }
    void Update()
    {
        checkTime();
    }
   public void SpawnUnit()
    {
        if (gameManager.GetMoney() >= spawnMoney && canSpawnAgain)
        {
            nextTimeToSpawn = Time.time + (1/spawnSpeed);
            int money = gameManager.GetMoney() - spawnMoney;
            gameManager.SetMoney(money);

            Instantiate(spawnThis, spawnPosition);
            respawnMaxTime = nextTimeToSpawn - Time.time;
            respawnCurTime = 0f;
            canSpawnAgain = false;
        }
    }
    private void checkTime()
    {
        if(!canSpawnAgain)
        {
            respawnCurTime += Time.deltaTime;

            respawnImage.fillAmount = respawnCurTime / respawnMaxTime;

            if(respawnImage.fillAmount  >= 1f)
            {
                respawnImage.fillAmount = 1f;
                canSpawnAgain = true;
            }
        }
    }
}
