using UnityEngine;

public class NextButtonScript : MonoBehaviour
{
    private StageManager stageManager = null;
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    public void OnClick()
    {
        stageManager.StageReset();
    }
}
