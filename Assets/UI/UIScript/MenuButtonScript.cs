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

            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                mBtnScript = transform.GetChild(0).GetChild(i).GetComponent<M_ButtonScript>();
                mBtnScript.Reset();
            }

            MenuSet(false);
        }
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            ShowMenu();
        }
    }
    public void ShowMenu()
    {
        OnClick();
    }
    public void OnClick()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            mBtnScript = transform.GetChild(0).GetChild(i).GetComponent<M_ButtonScript>();
            if(mBtnScript.isMoving)
            {
                 Debug.Log(mBtnScript.isMoving);
                return;
            }
            
        }

        if (!menuOpen)
        {
            MenuSet(true);
        }
        else
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
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

