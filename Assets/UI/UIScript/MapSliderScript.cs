using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSliderScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private Slider mapSlider = null;
    private Vector3 cameraPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        cameraPosition = mainCamera.transform.position;
        cameraPosition.x = mapSlider.value * 40f;
        mainCamera.transform.position = cameraPosition;

    }
}
