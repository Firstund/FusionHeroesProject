using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOnMiniMapScript : MonoBehaviour
{
    private MapSliderScript mapSliderScript = null;
    private RectTransform thisRectTransform = null;
    [SerializeField]
    private Transform _targetUnitTrm = null;
    public Transform targetUnitTrm
    {
        get { return _targetUnitTrm; }
        set { _targetUnitTrm = value; }
    }
    void Start()
    {
        thisRectTransform = GetComponent<RectTransform>();
        mapSliderScript = FindObjectOfType<MapSliderScript>();
    }

    void Update()
    {
        try{
            
        Vector2 a = targetUnitTrm.localPosition;  

        a.x = (a.x) * (1000f / mapSliderScript.mousePositionPerValue) - 7.5f;
        Debug.Log(1000f / mapSliderScript.mousePositionPerValue);

        thisRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, a.x, thisRectTransform.rect.width);
        }
        catch(MissingReferenceException)
        {
            Destroy(gameObject);
        }
    }
}
