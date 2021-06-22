using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteScript : MonoBehaviour
{
    private Image image = null;
    [SerializeField]
    private Sprite changeToIt = null;
    private Sprite firstSprite = null;
    void Start()
    {
        image = GetComponent<Image>();
        firstSprite = image.sprite;
    }
    public void ChangeImage()
    {
        image.sprite = changeToIt;
    }
    public void ChangeToFirst()
    {
        image.sprite = firstSprite;
    }
}
