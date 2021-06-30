using UnityEngine;

public class NextButtonScript : MonoBehaviour
{
    private StageManager stageManager = null;
    private DataManager dataManager = null;
    private GameManager gameManager = null;
    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = FindObjectOfType<StageManager>();
        dataManager = FindObjectOfType<DataManager>();
    }

    public void OnClick()
    {
        gameManager.GetSaveData().maxStatLev = 5 + 5 * (gameManager.GetSaveData().maxReachedStage / 10);
        stageManager.StageReset();
        dataManager.SaveGameData();
    }
}
