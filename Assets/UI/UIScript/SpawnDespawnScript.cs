using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDespawnScript : MonoBehaviour
{
    
    [SerializeField]
    protected GameObject spawnIt = null;
    [SerializeField]
    protected GameObject deSpawnIt = null;
    public void OnClick()
    {
        deSpawnIt.SetActive(false);
        spawnIt.SetActive(true);
    }
}
