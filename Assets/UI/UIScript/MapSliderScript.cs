using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSliderScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private Slider _mapSlider = null;
    public Slider mapSlider{
        get{return _mapSlider;}
        set{_mapSlider = value;}
    }
    private Vector3 cameraPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        cameraPosition = mainCamera.transform.position;
        cameraPosition.x = mapSlider.value * 40f;
        mainCamera.transform.position = cameraPosition;
    }
}
