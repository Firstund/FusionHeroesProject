using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOnMiniMapScript : MonoBehaviour
{
    private MapSliderScript mapSliderScript = null;
    private RectTransform thisRectTransform = null;
    private GameObject _targetObject = null;
    public GameObject targetObject
    {
        set{_targetObject = value;}
    }
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
        transform.SetParent(GameObject.Find("UnitOnMiniMaps").transform);
    }

    void Update()
    {
        
        if(targetUnitTrm != null){
            
        Vector2 a = targetUnitTrm.localPosition;  

        a.x = (a.x) * (1000f / mapSliderScript.maxCameraPosition) - 7.5f;

        thisRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, a.x, thisRectTransform.rect.width);

        gameObject.SetActive(_targetObject.active);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
