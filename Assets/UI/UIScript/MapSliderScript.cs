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
    private float moveSliderXChangeTime = 1f;
    [SerializeField]
    private float moveSliderSpeed = 20f;
    private Vector3 cameraPosition = Vector3.zero;
    [SerializeField]
    private Vector2 targetPosition = Vector2.zero;
    [SerializeField]
    private Vector2 mouseFirstPosition = Vector2.zero;
    [SerializeField]
    private bool mouseButtonDowned = false;
    [SerializeField]
    private bool mouseDrag = false;


    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        mapSlider.maxValue = maxCameraPosition;
    }
    void Update()
    {
        if (gameManager.GetCST())
        {
            if (!gameManager.GetUiClicked())
            {
                if (Input.GetMouseButtonDown(0) && !mouseButtonDowned)
                {
                    mouseFirstPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    mouseFirstPosition *= moveSliderSpeed;
                    mouseButtonDowned = true;
                }
                if (Input.GetMouseButtonUp(0) && mouseButtonDowned)
                {
                    mouseDrag = false;
                    mouseButtonDowned = false;
                }
            }
        }

        if (mouseButtonDowned)
        {
            targetPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            targetPosition *= moveSliderSpeed;

            float a = Vector2.Distance(mouseFirstPosition, targetPosition);

            if (a > minDragRange)
            {
                mouseDrag = true;
            }
        }

        if (!gameManager.tutoIsPlaying && mouseDrag && gameManager.GetCST())
        {
            mapSlider.DOValue(mapSlider.value + targetPosition.x - mouseFirstPosition.x, moveSliderXChangeTime);
            mapSlider.value = Mathf.Clamp(mapSlider.value, 0f, maxCameraPosition);
        }

        cameraPosition = mainCamera.transform.position;
        cameraPosition.x = mapSlider.value;
        mainCamera.transform.position = cameraPosition;
    }
    public void ReSet()
    {
        mouseDrag = false;
        mouseButtonDowned = false;
    }
}
