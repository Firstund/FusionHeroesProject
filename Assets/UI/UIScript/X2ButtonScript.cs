using UnityEngine;
using UnityEngine.UI;

public class X2ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Sprite[] startPauseSprite = new Sprite[2];
    [SerializeField]
    private GameManager gameManager = null;
    
    
    private void Start()
    {
        gameManager = GameManager.Instance;
       
    }
    private void Update()
    {
        
    }

    public void OnClick()
    {
        if(gameManager.GetCDT())
        {
            gameObject.GetComponent<Image>().sprite = startPauseSprite[1];
            gameManager.SetCDT(false);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = startPauseSprite[0];
            gameManager.SetCDT(true);
        }
    }
}
