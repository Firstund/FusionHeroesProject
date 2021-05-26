using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private Transform spawnPosition = null;

    [SerializeField]
    private float skeletonDelay = 1f;
    private float firstSkeletonDelay = 1f;
    [SerializeField]
    private float minusSkeletonDelayPerCurrentStage = 0.02f;
    //
    [SerializeField]
    private float archerDelay = 1f;
    private float firstArcherDelay = 1f;
    [SerializeField]
    private float minusArcherDelayPerCurrentStage = 0.02f;
    [SerializeField]
    private int archerSpawnStartStage = 2;
    // 
    [SerializeField]
    private float madBlindDelay = 1f;
    private float firstMadBlindDelay = 1f;
    [SerializeField]
    private float minusMadBlindDelayPerCurrentStage = 0.02f;
    [SerializeField]
    private int madBlindSpawnStartStage = 6;
    //

    [SerializeField]
    private GameObject oSkeleton = null;
    [SerializeField]
    private GameObject oArcher = null;
    [SerializeField]
    private GameObject oMadBlind = null;
    private bool skeSpawned = false;
    private bool arcSpawned = false;
    private bool madSpawned = false;
    void Awake()
    {
        firstSkeletonDelay = skeletonDelay;
        firstArcherDelay = archerDelay;
        firstMadBlindDelay = madBlindDelay;
    }
    void Start()
    {   
        gameManager = GameManager.Instance;
        spawnPosition = gameManager.GetEnemyUnitSpawnPosition();
    }

    void Update()
    {
        InitSpawnDelay();
        Spawn();
    }
    private void InitSpawnDelay()
    {
        skeletonDelay = firstSkeletonDelay - minusSkeletonDelayPerCurrentStage * gameManager.GetSaveData().currentStage;
        archerDelay = firstArcherDelay - minusArcherDelayPerCurrentStage * gameManager.GetSaveData().currentStage;
        madBlindDelay = firstMadBlindDelay - minusMadBlindDelayPerCurrentStage * gameManager.GetSaveData().currentStage;
    }
    void Spawn()
    {
        StartCoroutine(skeleton());
        StartCoroutine(archer());
        StartCoroutine(madBlind());
    }
    private IEnumerator madBlind()
    {
        if(!madSpawned && gameManager.GetSaveData().currentStage >= madBlindSpawnStartStage)
        {
            Vector2 a = spawnPosition.position;
            a.x += 0.1f;
            madSpawned = true;
            yield return new WaitForSeconds(madBlindDelay);
            Instantiate(oMadBlind, a, Quaternion.identity);
            madSpawned = false;
        }
    }
    private IEnumerator skeleton()
    {
        if (!skeSpawned)
        {
            Vector2 a = spawnPosition.position;
            a.x += 0.1f;
            skeSpawned = true;
            yield return new WaitForSeconds(skeletonDelay);
            Instantiate(oSkeleton, a, Quaternion.identity);
            skeSpawned = false;
        }
    }
    private IEnumerator archer()
    {
        if (!arcSpawned && gameManager.GetSaveData().currentStage >= archerSpawnStartStage)
        {
            Vector2 a = spawnPosition.position;
            a.x += 0.1f;
            arcSpawned = true;
            yield return new WaitForSeconds(archerDelay);
            Instantiate(oArcher, a, Quaternion.identity);
            arcSpawned = false;
        }
    }
     public Transform GetSpawnPosition()
    {
        return spawnPosition;
    }
}
