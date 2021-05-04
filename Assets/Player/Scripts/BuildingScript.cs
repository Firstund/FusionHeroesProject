using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingScript : MonoBehaviour
{
    FusionManager fusionManager = null;
    StageManager stageManager = null;
    GameManager gameManager = null;

    private SaveData saveData = null;
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
    private float firstDp = 0f;
    private float dpUp = 1f;

    [SerializeField]
    private bool destroy1Played = false;
    [SerializeField]

    private bool destroy2Played = false;

    public Vector2 currentPosition = Vector2.zero;

    void Awake()
    {
        firstHeart = heart;
        firstDp = dp;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        stageManager = FindObjectOfType<StageManager>();

        saveData = gameManager.GetSaveData();

        audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        int unitNum = fusionManager.GetUnitNum() + 1;
        fusionManager.SetUnitNum(unitNum);

        setStat();
        SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;
        audi.volume = gameManager.GetSoundValue();

        if(saveData != gameManager.GetSaveData())
        {
            saveData = gameManager.GetSaveData();
        }

        HealthBar();
        breaking();
    }
    public void SetMaxHealth()
    {
        slider.maxValue = heart;
        slider.value = heart;
        slider.minValue = 0;

    }
    private void HealthBar()
    {
        slider.DOValue(heart, gameManager.dovalueTime);
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
    public void setStat()
    {
        heart = firstHeart + heartUp * saveData.unitHeartLev[0];
        saveData.heart[0] = firstHeart = heart;

        dp = firstDp + dpUp * saveData.unitDpLev[0];
        saveData.dp[0] = firstDp = dp;
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
        else
        {
            anim.Play("Idle");
        }
    }
}
