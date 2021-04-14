using UnityEngine;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour
{
    FusionManager fusionManager = null;
    StageManager stageManager = null;
    GameManager gameManager = null;
    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private AudioClip destroy1 = null;

    private Animator anim = null;

    [SerializeField]
    private float heart = 10000f;
    private float firstHeart = 0f;
    [SerializeField]
    private float heartUp = 1000f;
    private float dp = 10f;
    private float dpUp = 1f;

    [SerializeField]
    private bool destroy1Played = false;
    [SerializeField]

    private bool destroy2Played = false;

    public Vector2 currentPosition = Vector2.zero;

    // Start is called before the first frame update
    void Awake()
    {
        firstHeart = heart;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        stageManager = FindObjectOfType<StageManager>();

        audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        int unitNum = fusionManager.GetUnitNum() + 1;
        fusionManager.SetUnitNum(unitNum);
 
        SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;
        audi.volume = gameManager.GetSoundValue();

        HealthBar();
        breaking();
    }
    private void SetMaxHealth()
    {
        slider.maxValue = heart;
        slider.value = heart;
        slider.minValue = 0;

    }
    private void HealthBar()
    {
        slider.value = heart;
    }
    public float getHe()
    {
        return heart;
    }
    public float getD()
    {
        return dp;
    }
    public void SetHP(float he)
    {
        heart = he;
    }
     public void Reset()
    {
        heart = firstHeart;
        destroy1Played = false;
        destroy2Played = false;
    }
    private void setStat() // 나중에 건물 업그레이드 기능을 넣었을 때 제대로 작동시킬것
    {
        heart = firstHeart + heartUp;
    }
    void breaking()
    {
        if (heart <= 0)
        {
            anim.Play("destory2Idle");
            if (!destroy2Played)
            {
                stageManager.StageClear(false);
                destroy2Played = true;
                audi.clip = destroy1;
                audi.Play();       
            }
        }
        else if (heart <= (firstHeart / 2))
        {
            anim.Play("destroy1Idle");
            if (!destroy1Played)
            {
                destroy1Played = true;
                audi.clip = destroy1;
                audi.Play();
            }
        }
    }
}
