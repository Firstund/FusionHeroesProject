using UnityEngine;

public class M_ButtonScript : MonoBehaviour
{
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
    void Update()
    {
        currentPosition = transform.localPosition;

        if (!haveToBack)
            currentPosition = Vector2.Lerp(currentPosition, buttonPosition.localPosition, Time.deltaTime * speed); // 이동
        else
        {
            currentPosition = Vector2.Lerp(currentPosition, menuButtonPositon, Time.deltaTime * speed); // 되돌아오기
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
        currentPosition = new Vector2(-3f, -3f);
        transform.localPosition = currentPosition;
    }
    public void OnDisable()
    {
        speed = firstSpeed;
    }
    public void SpawnPopUp()
    {
        spawnPopUp.SetActive(true);
    }
}
