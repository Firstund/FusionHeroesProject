using UnityEngine;
using UnityEngine.UI;

public class InputCurrentStageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject stageSetFailedText = null;
    private StageManager stageManager = null;
    private DataManager dataManager = null;
    private InputField inputField = null;

    public void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        dataManager = FindObjectOfType<DataManager>();
        inputField = GetComponent<InputField>();
    }
    public void OnEndEdit()
    {
        int inItStageNum = int.Parse(inputField.text);

        if(inItStageNum <= 0)
            inItStageNum = 1;

        if(inItStageNum > GameManager.Instance.GetSaveData().maxReachedStage)
        {
            Instantiate(stageSetFailedText, stageManager.textSpawnPosition);
            return;
        }

        dataManager.SaveGameData();
        stageManager.StageReset();
        stageManager.SetCurrentStage(inItStageNum);
    }
}
