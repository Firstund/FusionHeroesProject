using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSpawnScript : MonoBehaviour
{
    StageManager stageManager = null;
    GameManager gameManager = null;
    [SerializeField]
    private int startSpawnStage = 0;
    private bool canSpawnIt = true;
    [SerializeField]
    private GameObject iconImage = null;
    [SerializeField]
    private GameObject thisUpgradePannel = null;
    [SerializeField]
    private GameObject uCannotSpawnItYetText = null;

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
        get { return _spawnPosition; }
        set { _spawnPosition = value; }
    }
    [SerializeField]
    private Image respawnImage = null;
    [SerializeField]
    private int _spawnMoney = 5;
    public int spawnMoney
    {
        get { return _spawnMoney; }
        set { _spawnMoney = value; }
    }
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
        checkStage();
        checkTime();
        spawnCostText.text = $"{spawnMoney}";
    }

    private void checkStage()
    {
        if (iconImage != null && thisUpgradePannel != null)
        {
            if (gameManager.GetSaveData().currentStage < startSpawnStage)
            {
                canSpawnIt = false;
                iconImage.SetActive(false);
                thisUpgradePannel.SetActive(false);
            }
            else
            {
                canSpawnIt = true;
                iconImage.SetActive(true);
                thisUpgradePannel.SetActive(true);
            }
        }
        else if(iconImage == null)
        {
            Debug.LogError($"{this}.iconImage is null");
        }
        else if(thisUpgradePannel == null)
        {
            Debug.LogError($"{this}.thisUpgradePannel is null");
        }
    }

    public void SpawnUnit()
    {
        if (canSpawnIt)
        {
            if (gameManager.GetMoney() >= spawnMoney && canSpawnAgain)
            {
                nextTimeToSpawn = Time.time + (1 / spawnSpeed);
                int money = gameManager.GetMoney() - spawnMoney;
                gameManager.SetMoney(money);
                Vector2 a = spawnPosition.position;
                a.x -= 0.1f;
                Instantiate(spawnThis, a, Quaternion.identity);
                respawnMaxTime = nextTimeToSpawn - Time.time;
                respawnCurTime = 0f;
                canSpawnAgain = false;
            }
            else if (!canSpawnAgain)
            {
                Instantiate(haveToWaitMoreTimeText, stageManager.textSpawnPosition);
            }
            else
            {
                Instantiate(stageManager.notEnoughMoneyText, stageManager.textSpawnPosition);
            }
        }
        else
        {
            uCannotSpawnItYetText.transform.GetChild(0).GetComponent<Text>().text = startSpawnStage + "스테이지 이후부터 소환할 수 있습니다.";
            Instantiate(uCannotSpawnItYetText, stageManager.textSpawnPosition);
        }
    }
    private void checkTime()
    {
        if (!canSpawnAgain)
        {
            respawnCurTime += Time.deltaTime;

            respawnImage.fillAmount = respawnCurTime / respawnMaxTime;

            if (respawnImage.fillAmount >= 1f)
            {
                respawnImage.fillAmount = 1f;
                canSpawnAgain = true;
            }
        }
    }
}
