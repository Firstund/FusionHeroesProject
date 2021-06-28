using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStrongestScript : MonoBehaviour
{

    [SerializeField]
    private int strongestSpawnNum = 0;
    
    [SerializeField]
    private StageManager stageManager = null;
    private EnemyBuildingScript enemyBuildingScript = null;
    private EnemySpawnScript enemySpawnScript = null;
    private Transform spawnPosition = null;
    void Start()
    {
        enemyBuildingScript = GetComponent<EnemyBuildingScript>();
        enemySpawnScript = GetComponent<EnemySpawnScript>();
        spawnPosition = enemySpawnScript.GetSpawnPosition();
    }
    void Update()
    {
        strongestSpawnNum = enemyBuildingScript.spawnStrongestNum;
        if (!GameManager.Instance.tutoIsPlaying)
        {
            SpawnStrongest();
        }
    }
    private void SpawnStrongest()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(strongestSpawnNum) && stageManager.GetCurrentStage()  != 0 && stageManager.GetCurrentStage() % 5 == 0 && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() * enemyBuildingScript.strongestSpawnPerHp[strongestSpawnNum])
        {
            enemyBuildingScript.SetstrongestSpawned(strongestSpawnNum, true);
            Instantiate(enemyBuildingScript.GetStrongest(strongestSpawnNum), enemySpawnScript.GetSpawnPosition());
        }
    }
}
