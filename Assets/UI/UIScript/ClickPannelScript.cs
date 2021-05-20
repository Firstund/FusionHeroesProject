using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPannelScript : MonoBehaviour
{
    private GameManager gameManager = null;
    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            gameManager.mousePosition = mousePosition;
        }
    }
}
