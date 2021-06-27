using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradePannelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPanel = null;
    [SerializeField]
    private Sprite endUpgradePannelSprite = null;
    [SerializeField]
    private Sprite middleUpgradePannelSprite = null;

    private Image myImage = null;
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    void Update()
    {
        if (nextPanel != null)
        {
            if (nextPanel.active)
            {
                myImage.sprite = middleUpgradePannelSprite;
            }
            else
            {
                myImage.sprite = endUpgradePannelSprite;
            }
        }
        else
        {
            myImage.sprite = endUpgradePannelSprite;
        }
    }
}
