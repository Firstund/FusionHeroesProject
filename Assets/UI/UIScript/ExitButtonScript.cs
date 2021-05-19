using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
    [SerializeField]
    private PopUpScaleScript popUpScaleScript = null;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClcik();
        }
    }
    public void OnClcik()
    {
        popUpScaleScript.OnClickDisable();
    }
}
