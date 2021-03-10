using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevNameScript : TextScript
{
    UnitScript thisObject = null;
    Vector2 currentPosition = Vector2.zero;
    void Start()
    {
        gameManager = GameManager.Instance;
        text = GetComponent<Text>();
        thisObject = transform.parent.transform.parent.GetComponent<UnitScript>();
    }
    void Update()
    {
        currentPosition = transform.position;
        currentPosition.x = thisObject.GetCurrentPosition().x;
        currentPosition.y = thisObject.GetCurrentPosition().y;
        transform.position = currentPosition;

        SetText();
    }
    void SetText()
    {
        text.text = string.Format("Lev: {0}", thisObject.GetUnitLev());
    }
}
