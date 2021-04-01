using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    private bool menuOpen = false;
    [SerializeField]
    private int comeBackNum = 0;

    M_ButtonScript mBtnScript = null;
    private void Update()
    {
        if (comeBackNum >= 3)
        {
            comeBackNum = 0;

            MenuSet(false);

            for (int i = 0; i < transform.GetChild(0).GetChildCount(); i++)
            {
                mBtnScript = transform.GetChild(0).GetChild(i).GetComponent<M_ButtonScript>();
                mBtnScript.Reset();
            }

        }
    }
    public void OnClick()
    {
        if (!menuOpen)
        {    
            MenuSet(true);
        }
        else
        {
            for (int i = 0; i < transform.GetChild(0).GetChildCount(); i++)
            {
                mBtnScript = transform.GetChild(0).GetChild(i).GetComponent<M_ButtonScript>();
                mBtnScript.Return();
            }
        }
        menuOpen = !menuOpen;
    }
    private void MenuSet(bool a)
    {
        transform.GetChild(0).gameObject.SetActive(a);
    }
    public void IsComeBack()
    {
        comeBackNum++;
    }
}

