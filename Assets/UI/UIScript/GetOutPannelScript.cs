using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutPannelScript : PopUpScaleScript
{
    void Start()
    {
        PlusStart();
    }

    void Update()
    {
        PlusUpdate();

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickNo();
        }
    }
    public void OnClickNo()
    {
        OnClickDisable();
    }
    public void OnClickYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
