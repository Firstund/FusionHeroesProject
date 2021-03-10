using UnityEngine.UI;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    protected Text text = null;
    protected GameManager gameManager = null;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {      
        SetText();
    }
    void SetText()
    {
        text.text = string.Format("Current Money: {0}", gameManager.GetMoney());
    }
}
