using UnityEngine;

public class NextButtonScript : MonoBehaviour
{
    private StageManager stageManager = null;
    private DataManager dataManager = null;
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        dataManager = FindObjectOfType<DataManager>();
    }

    public void OnClick()
    {
        dataManager.SaveGameData();
        stageManager.StageReset();
    }
}
