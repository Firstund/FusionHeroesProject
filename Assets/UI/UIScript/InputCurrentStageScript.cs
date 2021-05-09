using UnityEngine;
using UnityEngine.UI;

public class InputCurrentStageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject stageSetFailedText = null;
    private StageManager stageManager = null;
    private InputField inputField = null;

    public void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        inputField = GetComponent<InputField>();
    }
    public void OnEndEdit()
    {
        if(int.Parse(inputField.text) > stageManager.GetCurrentStage())
        {
            Instantiate(stageSetFailedText, transform);
            return;
        }

        stageManager.StageReset();
        stageManager.SetCurrentStage(int.Parse(inputField.text));
        inputField.text = "";
    }
}
