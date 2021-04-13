using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBuildingScript : MonoBehaviour
{
    //enemy 자동소환기능 넣기
    [SerializeField]
    private Slider slider = null;
    FusionManager fusionManager = null;
    StageManager stageManager = null;
    GameManager gameManager = null;
    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private AudioClip destroy1 = null;

    private Animator anim = null;

    private Transform spawnPosition = null;

    [SerializeField]
    private float skeletonDelay = 1f;
    [SerializeField]
    private float archerDelay = 1f;

    [SerializeField]
    private GameObject oSkeleton = null;
    [SerializeField]
    private GameObject oArcher = null;

    [SerializeField]
    private GameObject[] strongest = new GameObject[2];

    [SerializeField]
    private float heart = 10000f;
    private float heartUp = 1000f;
    private float dp = 10f;
    private float dpUp = 1f;

    private float firstHeart = 0f;

    private bool skeSpawned = false;
    private bool arcSpawned = false;
    private bool umikoSpawned = false;

    private bool destroy1Played = false;
    private bool destroy2Played = false;

    public Vector2 currentPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        stageManager = FindObjectOfType<StageManager>();
        spawnPosition = gameManager.GetEnemyUnitSpawnPosition();

        audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        int enemyUnitNum = fusionManager.GetEnemyUnitNum() + 1;
        fusionManager.SetEnemyUnitNum(enemyUnitNum);
        
        firstHeart = heart;
        SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;
        audi.volume = gameManager.GetSoundValue();

        HealthBar();
        Breaking();
        Spawn();
        umiko();
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
    private void umiko()
    {
        if(!umikoSpawned)
        {
            umikoSpawned = true;
            Instantiate(strongest[0], spawnPosition);
            Instantiate(strongest[1], spawnPosition);
        }
    }
    private IEnumerator skeleton()
    {
        if (!skeSpawned)
        {
            skeSpawned = true;
            yield return new WaitForSeconds(skeletonDelay);
            Instantiate(oSkeleton, spawnPosition);
            skeSpawned = false;
        }
    }
    private IEnumerator archer()
    {
        if (!arcSpawned)
        {
            arcSpawned = true;
            yield return new WaitForSeconds(archerDelay);
            Instantiate(oArcher, spawnPosition);
            arcSpawned = false;
        }
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
        dp =  dpUp;
    }
    void Spawn()
    {
        StartCoroutine(skeleton());
        StartCoroutine(archer());
    }
    void Breaking()
    {
        if (heart <= 0)
        {
            anim.Play("destroy2Idle");
            if (!destroy2Played)
            {
                destroy2Played = true;
                audi.clip = destroy1;
                audi.Play();
                stageManager.StageClear(true);
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

