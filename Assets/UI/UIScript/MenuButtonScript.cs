using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] buttons = null;
    [SerializeField]
    private GameObject buttonsParent = null;
    private bool menuOpen = false;
    [SerializeField]
    private int comeBackNum = 0;

    M_ButtonScript mBtnScript = null;
    private void Update()
    {
        if (comeBackNum >= 3)
        {
            comeBackNum = 0;

            foreach(var item in buttons)
            {
                mBtnScript = item.GetComponent<M_ButtonScript>();
                mBtnScript.Reset();
            }

            MenuSet(false);////////
        }
        // if(Input.GetKeyUp(KeyCode.Escape))
        // {
        // }
    }
    public void ShowMenu()
    {
        OnClick();
    }
    public void OnClick()
    {
        foreach(var item in buttons)
        {
            mBtnScript = item.GetComponent<M_ButtonScript>();
            if(mBtnScript.isMoving)
            {
                return;
            }
            
        }

        if (!menuOpen)
        {
            MenuSet(true);
        }
        else
        {
            foreach(var item in buttons)
            {
                mBtnScript = item.GetComponent<M_ButtonScript>();
                mBtnScript.Return();
            }
        }
        menuOpen = !menuOpen;
    }
    private void MenuSet(bool a)
    {
        buttonsParent.gameObject.SetActive(a);////
    }
    public void IsComeBack()
    {
        comeBackNum++;
    }
}

