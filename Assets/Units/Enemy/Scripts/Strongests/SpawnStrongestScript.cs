using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStrongestScript : MonoBehaviour
{
    FusionManager fusionManager = null;

    [SerializeField]
    private int strongestSpawnNum = 0;
    [SerializeField]
    private AudioClip spawnStrongestSound = null;
    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private GameObject spawnStrongestEnergy = null;
    
    [SerializeField]
    private StageManager stageManager = null;
    private EnemyBuildingScript enemyBuildingScript = null;
    private EnemySpawnScript enemySpawnScript = null;
    private Transform spawnPosition = null;
    void Start()
    {
        fusionManager = FindObjectOfType<FusionManager>();
        enemyBuildingScript = GetComponent<EnemyBuildingScript>();
        enemySpawnScript = GetComponent<EnemySpawnScript>();
        audi = GetComponent<AudioSource>();

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
            audi.clip = spawnStrongestSound;
            audi.Play();
            foreach(var item in fusionManager.unitScript)
            {
                item.MoveBack();
            }
            spawnStrongestEnergy.SetActive(true);
        }
    }
}
