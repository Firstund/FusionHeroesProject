using System.Collections;
using UnityEngine;

public class M_ButtonScript : MonoBehaviour
{
    private GameManager gameManager = null;
    [SerializeField]
    private Transform buttonPosition = null;
    private Vector2 menuButtonPositon = Vector2.zero;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float firstSpeed = 0f;

    [SerializeField]
    private bool haveToBack = false;
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get{return _isMoving;}
        set{_isMoving = value;}
    }
    [SerializeField]
    private float comeBackVector = -2f;
    [SerializeField]
    private Vector2 currentPosition = Vector2.zero;
    [SerializeField]
    private MenuButtonScript menuButtonScript;
    [SerializeField]
    private GameObject spawnPopUp = null;
    void Awake()
    {
        firstSpeed = speed;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        currentPosition = transform.localPosition;

        if (!haveToBack && (currentPosition - (Vector2)buttonPosition.localPosition).sqrMagnitude >= 0.01f)
        {   
            isMoving = true;      
            currentPosition = Vector2.Lerp(currentPosition, buttonPosition.localPosition, Time.deltaTime * speed); // 이동
        }
        else
        {
            if(haveToBack && (currentPosition - menuButtonPositon).sqrMagnitude >= 0.01f)
            {
                isMoving = true;
                currentPosition = Vector2.Lerp(currentPosition, menuButtonPositon, Time.deltaTime * speed); // 되돌아오기
            }
            else
            {
                isMoving = false;
            }
        }

        if (currentPosition.x > comeBackVector && currentPosition.y > comeBackVector)
        {
            transform.localPosition = currentPosition;
            menuButtonScript.IsComeBack();
            
            Reset();
            speed = 0f;
            Return();
        }

        transform.localPosition = currentPosition;
    }
    public void Return()
    {
        haveToBack = !haveToBack;
    }
    public void Reset()
    {
        haveToBack = false;
        isMoving = false;
        currentPosition = new Vector2(-3f, -3f);
        transform.localPosition = currentPosition;
    }
    public void OnDisable()
    {
        speed = firstSpeed;
    }
    public void SpawnPopUp()
    {
        gameManager.popUpIsSpawned = true;
        spawnPopUp.SetActive(true);
    }
}
