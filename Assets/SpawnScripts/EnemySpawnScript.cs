using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private EnemyPooling poolManager = null;
    private Transform spawnPosition = null;

    [SerializeField]
    private EnemyScript spawnThis = null;

    // 이 스크립트를 유닛들이 우려먹을 수 있도록 바꾸기
    private float objAtLeastDelay = 0f;
    [SerializeField]
    private float spawnDelay = 1f;
    private float firstObjDelay = 1f;
    [SerializeField]
    private float minusObjDelayPerCurrentStage = 0.02f;
    // 이 유닛을 어느 스테이지때부터 스폰하는가? 어느 스테이지때부터 스폰을 안하는가?
    [SerializeField]
    private int spawnStartStage = 0;
    [SerializeField]
    private int endSpawnStage = 100;

    private bool objSpawned = false;
    void Awake()
    {
        firstObjDelay = spawnDelay;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        poolManager = FindObjectOfType<EnemyPooling>();

        spawnPosition = gameManager.GetEnemyUnitSpawnPosition();

        objAtLeastDelay = minusObjDelayPerCurrentStage * 0f;
    }

    void Update()
    {
        if (!gameManager.tutoIsPlaying)
        {
            InitSpawnDelay();
            Spawn();
        }
    }
    private void InitSpawnDelay()
    {
        spawnDelay = firstObjDelay - minusObjDelayPerCurrentStage * gameManager.GetSaveData().currentStage;
        spawnDelay = Mathf.Clamp(spawnDelay, objAtLeastDelay, firstObjDelay) + 1f;
    }
    void Spawn()
    {
        StartCoroutine(spawn());
    }
    private IEnumerator spawn()
    {
        if (gameManager.GetSaveData().currentStage >= spawnStartStage && gameManager.GetSaveData().currentStage <= endSpawnStage)
        {
            if (!objSpawned)
            {
                Vector2 a = spawnPosition.position;
                a.x += 0.1f;
                objSpawned = true;
                yield return new WaitForSeconds(spawnDelay);

                if (gameManager.GetSaveData().currentStage >= spawnStartStage && gameManager.GetSaveData().currentStage <= endSpawnStage)
                {
                    if (!poolManager.Go(spawnThis.GetUnitID()))
                    {
                        Instantiate(spawnThis.gameObject, a, Quaternion.identity);
                    }
                }

                objSpawned = false;
            }
        }
    }
    public Transform GetSpawnPosition()
    {
        return spawnPosition;
    }
}
