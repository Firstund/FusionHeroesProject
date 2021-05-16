using UnityEngine;
using UnityEngine.UI;

public class InputCurrentStageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject stageSetFailedText = null;
    [SerializeField]
    private Transform spawnPosition = null;
    private StageManager stageManager = null;
    private InputField inputField = null;

    public void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        inputField = GetComponent<InputField>();
    }
    public void OnEndEdit()
    {
        int inItStageNum = int.Parse(inputField.text);

        if(inItStageNum <= 0)
            inItStageNum = 1;

        if(inItStageNum > GameManager.Instance.GetSaveData().maxReachedStage)
        {
            Instantiate(stageSetFailedText, spawnPosition);
            return;
        }

        stageManager.StageReset();
        stageManager.SetCurrentStage(inItStageNum);
    }
}
