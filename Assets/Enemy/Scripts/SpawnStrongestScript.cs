using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStrongestScript : MonoBehaviour
{
    [SerializeField]
    private StageManager stageManager = null;
    private EnemyBuildingScript enemyBuildingScript = null;
    void Start()
    {
        enemyBuildingScript = gameObject.GetComponent<EnemyBuildingScript>();
    }
    void Update()
    {
        umiko();
        glingman();
    }
    #region strongests
    private void umiko()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(0) && stageManager.GetCurrentStage() == 2  && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 2)
        {
            Debug.Log("aaa");
            enemyBuildingScript.SetstrongestSpawned(0, true);
            Instantiate(enemyBuildingScript.GetStrongest(0), enemyBuildingScript.GetSpawnPosition());
        }
    }
    private void glingman()
    {
        if (!enemyBuildingScript.GetStrongestSpawned(1) && stageManager.GetCurrentStage() == 1 && enemyBuildingScript.getHe() <= enemyBuildingScript.getFirstHe() / 5)
        {
            Debug.Log("bbb");
            enemyBuildingScript.SetstrongestSpawned(1, true);
            Instantiate(enemyBuildingScript.GetStrongest(1), enemyBuildingScript.GetSpawnPosition());
        }
    }
    #endregion
}
