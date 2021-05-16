using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButtonScript : MonoBehaviour
{   
    private DataManager dataManager = null;
    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    public void OnClick()
    {
        dataManager.SaveGameData();
    }
}
