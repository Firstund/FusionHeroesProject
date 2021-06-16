using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    private float _maxCameraPosition = 40f;
    public float maxCameraPosition
    {
        get { return _maxCameraPosition; }
        set { _maxCameraPosition = value; }
    }
    [SerializeField]
    private float minDragRange = 1f;
    [SerializeField]
    private float moveSliderXSpeed = 1f;
    private Vector3 cameraPosition = Vector3.zero;
    [SerializeField]
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 mouseFirstPosition = Vector2.zero;
    private bool mouseButtonDowned = false;
    private bool mouseDrag = false;


    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        mapSlider.maxValue = maxCameraPosition;
    }
    void Update()
    {
        if (!gameManager.GetUiClicked())
        {
            if (gameManager.GetCST())
            {
                if (Input.GetMouseButtonDown(0) && !mouseButtonDowned)
                {
                    mouseFirstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseButtonDowned = true;
                }
                if (Input.GetMouseButtonUp(0) && mouseButtonDowned)
                {
                    mouseDrag = false;
                    mouseButtonDowned = false;
                }
            }
        }

        if(mouseButtonDowned)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            float a = Vector2.Distance(mouseFirstPosition, targetPosition);

            if(a > minDragRange)
            {
                mouseDrag = true;
            }
        }

        if (!gameManager.tutoIsPlaying && mouseDrag)
        {
            // if (gameManager.mousePosition.x >= gameManager.halfViewportSizeX)
            //     mapSlider.value += moveSliderXSpeed * Time.deltaTime;
            // else if (gameManager.mousePosition.x < gameManager.halfViewportSizeX)
            //     mapSlider.value -= moveSliderXSpeed * Time.deltaTime;

            targetPosition.x += targetPosition.x - mouseFirstPosition.x;
            targetPosition.x = Mathf.Clamp(targetPosition.x, 0f, maxCameraPosition);
            
            mapSlider.DOValue(targetPosition.x, moveSliderXSpeed);

        
        }
        cameraPosition = mainCamera.transform.position;
        cameraPosition.x = mapSlider.value;
        mainCamera.transform.position = cameraPosition;
    }
    void SetMouseDrag()
    {
        mouseDrag = false;
    }
}
