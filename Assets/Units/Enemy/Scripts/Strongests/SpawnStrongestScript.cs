using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStrongestScript : MonoBehaviour
{
    [SerializeField]
    private int spawnGlingmanStage = 1;
    [SerializeField]
    private int spawnUmikoStage = 10;
    [SerializeField]
    private int spawnImpStage = 15;
    [SerializeField]
    private int spawnGolemStage = 20;
    
    [SerializeField]
    private StageManager stageManager = null;
    private EnemyBuildingScript enemyBuildingScript = null;
    private EnemySpawnScript enemySpawnScript = null;
    private Transform spawnPosition = null;
    void Start()
    {
        enemyBuildingScript = gameObject.GetComponent<EnemyBuildingScript>();
        enemySpawnScript = gameObject.GetComponent<EnemySpawnScript>();
        spawnPosition = enemySpawnScript.GetSpawnPosition();
    }
    void Update()
    {
        if (!GameManager.Instance.tutoIsPlaying)
        {
            umiko();
            glingman();
            imp();
            golem();
        }
    }
    #region strongests
    private void glingman()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(0) && stageManager.GetCurrentStage() == spawnGlingmanStage && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 5)
        {
            enemyBuildingScript.SetstrongestSpawned(0, true);
            Instantiate(enemyBuildingScript.GetStrongest(0), enemySpawnScript.GetSpawnPosition());
        }
    }
    private void umiko()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(1) && stageManager.GetCurrentStage() == spawnUmikoStage && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 2)
        {
            enemyBuildingScript.SetstrongestSpawned(1, true);
            Instantiate(enemyBuildingScript.GetStrongest(1), enemySpawnScript.GetSpawnPosition());
        }
    }
    private void imp()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(2) && stageManager.GetCurrentStage() == spawnImpStage && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe())
        {
            enemyBuildingScript.SetstrongestSpawned(2, true);
            Instantiate(enemyBuildingScript.GetStrongest(2), enemySpawnScript.GetSpawnPosition());
        }
    }
    private void golem()
    {
        if(!enemyBuildingScript.GetStrongestSpawned(3) && stageManager.GetCurrentStage() == spawnGolemStage && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe())
        {
            enemyBuildingScript.SetstrongestSpawned(3, true);
            Instantiate(enemyBuildingScript.GetStrongest(3), enemySpawnScript.GetSpawnPosition());
        }
    }

    #endregion
}
