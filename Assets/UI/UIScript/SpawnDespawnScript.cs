using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDespawnScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject spawnIt = null;
    [SerializeField]
    protected GameObject deSpawnIt = null;
    [SerializeField]
    protected bool isPasteButton;
    void Update()
    {
        if (isPasteButton)
        {

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnClick();
            }
        }
    }
    public void OnClick()
    {
        deSpawnIt.SetActive(false);
        spawnIt.SetActive(true);
    }
}
