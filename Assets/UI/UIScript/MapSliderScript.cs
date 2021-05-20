using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSliderScript : MonoBehaviour
{
    private FusionManager fusionManager = null;
    private GameManager gameManager = null;
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private Slider _mapSlider = null;
    public Slider mapSlider
    {
        get { return _mapSlider; }
        set { _mapSlider = value; }
    }
    [SerializeField]
    private float mousePositionPerValue = 40f;
    [SerializeField]
    private float moveSliderXSpeed = 1f;
    private Vector3 cameraPosition = Vector3.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
    }
    void Update()
    {
        if (!gameManager.GetUiClicked())
        {

            if (gameManager.GetCST() && Input.GetMouseButton(0))
            {
                if (gameManager.mousePosition.x >= gameManager.halfScreenSizeX)
                    mapSlider.value += moveSliderXSpeed * Time.deltaTime;
                else if (gameManager.mousePosition.x < gameManager.halfScreenSizeX)
                    mapSlider.value -= moveSliderXSpeed * Time.deltaTime;
            }
        }
        cameraPosition = mainCamera.transform.position;
        cameraPosition.x = mapSlider.value * mousePositionPerValue;
        mainCamera.transform.position = cameraPosition;
    }
}
