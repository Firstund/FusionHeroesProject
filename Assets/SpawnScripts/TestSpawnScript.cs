using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSpawnScript : MonoBehaviour
{
    StageManager stageManager = null;
    GameManager gameManager = null;

    [SerializeField]
    private GameObject spawnThis = null;
    [SerializeField]
    private GameObject haveToWaitMoreTimeText = null;
    [SerializeField]
    private Text spawnCostText = null;
    [SerializeField]
    private Transform _spawnPosition = null;
    public Transform spawnPosition
    {
        get{return _spawnPosition;}
        set{_spawnPosition = value;}
    }
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
        stageManager = FindObjectOfType<StageManager>();
        spawnPosition = gameManager.GetUnitSpawnPosition();
        spawnCostText = transform.GetChild(1).GetComponent<Text>();
    }
    void Update()
    {
        checkTime();
        spawnCostText.text = $"{spawnMoney}";
    }
   public void SpawnUnit()
    {
        if (gameManager.GetMoney() >= spawnMoney && canSpawnAgain)
        {
            nextTimeToSpawn = Time.time + (1/spawnSpeed);
            int money = gameManager.GetMoney() - spawnMoney;
            gameManager.SetMoney(money);
            Vector2 a = spawnPosition.position;
            a.x -= 0.1f;
            Instantiate(spawnThis, a, Quaternion.identity);
            respawnMaxTime = nextTimeToSpawn - Time.time;
            respawnCurTime = 0f;
            canSpawnAgain = false;
        }
        else if(!canSpawnAgain)
        {
            Instantiate(haveToWaitMoreTimeText, stageManager.textSpawnPosition);
        }
        else
        {
            Instantiate(stageManager.notEnoughMoneyText, stageManager.textSpawnPosition);
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
