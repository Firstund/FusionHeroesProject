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
    private StageManager stageManager = null;
    private EnemyBuildingScript enemyBuildingScript = null;
    private EnemySpawnScript enemySpawnScript = null;
    void Start()
    {
        enemyBuildingScript = gameObject.GetComponent<EnemyBuildingScript>();
        enemySpawnScript = gameObject.GetComponent<EnemySpawnScript>();
    }
    void Update()
    {
        umiko();
        glingman();
    }
    #region strongests
    private void glingman()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(0) && stageManager.GetCurrentStage() == spawnGlingmanStage  && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 5)
        {
            enemyBuildingScript.SetstrongestSpawned(0, true);
            Instantiate(enemyBuildingScript.GetStrongest(0), enemySpawnScript.GetSpawnPosition());
        }
    }
    private void umiko()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(1) && stageManager.GetCurrentStage() == spawnUmikoStage  && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 2)
        {
            enemyBuildingScript.SetstrongestSpawned(1, true);
            Instantiate(enemyBuildingScript.GetStrongest(1), enemySpawnScript.GetSpawnPosition());
        }
    }
    
    #endregion
}
