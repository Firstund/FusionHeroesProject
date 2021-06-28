using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBuildingScript : MonoBehaviour
{
    //enemy 자동소환기능 넣기

    FusionManager fusionManager = null;
    StageManager stageManager = null;
    GameManager gameManager = null;
    MapSliderScript mapSliderScript = null;

    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private GameObject g_slider = null;
    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private AudioClip destroy1 = null;

    private Animator anim = null;

    [SerializeField]
    private GameObject[] strongest = new GameObject[2];

    [SerializeField]
    private float heart = 10000f;
    [SerializeField]
    private float heartUp = 1000f;
    [SerializeField]
    private float dp = 2f;
    private float firstDp = 0f;
    [SerializeField]
    private float dpUp = 1.1f;

    private float firstHeart = 0f;
    [SerializeField]
    private float[] _strongestSpawnPerHp;
    public float[] strongestSpawnPerHp
    {
        get { return _strongestSpawnPerHp; }
        set { _strongestSpawnPerHp = value; }
    }

    private int _spawnStrongestNum = 0;
    public int spawnStrongestNum
    {
        get { return _spawnStrongestNum; }
        set { _spawnStrongestNum = value; }
    }

    [SerializeField]
    private bool[] strongestSpawned;

    private bool destroy1Played = false;
    private bool destroy2Played = false;

    public Vector2 currentPosition = Vector2.zero;


    void Start()
    {
        gameManager = GameManager.Instance;
        fusionManager = FindObjectOfType<FusionManager>();
        stageManager = FindObjectOfType<StageManager>();
        mapSliderScript = FindObjectOfType<MapSliderScript>();


        audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        int enemyUnitNum = fusionManager.GetEnemyUnitNum() + 1;
        fusionManager.SetEnemyUnitNum(enemyUnitNum);

        firstHeart = heart;
        firstDp = dp;

        spawnStrongestNum = Random.Range(0, strongestSpawned.Length);

        for (int i = 0; i < gameManager.GetSaveData().currentStage; i++)
            setStat();

        SetMaxHealth();
    }

    void Update()
    {
        currentPosition = transform.localPosition;
        audi.volume = gameManager.GetSoundValue();

        HealthBar();
        Breaking();
    }



    public void SetMaxHealth()
    {
        slider.maxValue = heart;
        slider.value = heart;
        slider.minValue = 0;
    }
    private void HealthBar()
    {
        if (mapSliderScript.mapSlider.value <= 40f - 3.5f)
        {
            g_slider.SetActive(false);
        }
        else
        {
            g_slider.SetActive(true);
        }
        slider.DOValue(heart, gameManager.dovalueTime);
    }
    public float getHe()
    {
        return heart;
    }
    public float getFirstHe()
    {
        return firstHeart + heartUp * gameManager.GetSaveData().currentStage;
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
        spawnStrongestNum = Random.Range(0, strongestSpawned.Length);

        for (int i = 0; i < strongestSpawned.Length - 1; i++)
        {
            strongestSpawned[i] = false;
        }

        setStat();

        destroy1Played = false;
        destroy2Played = false;
    }
    private void setStat() // 나중에 건물 업그레이드 기능을 넣었을 때 제대로 작동시킬것
    {

        heart = firstHeart + heartUp * gameManager.GetSaveData().currentStage;
        SetMaxHealth();

        dp = firstDp + dpUp * gameManager.GetSaveData().currentStage;
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
        else
        {
            anim.Play("Idle");
        }
    }
    public bool GetStrongestSpawned(int index)
    {
        return strongestSpawned[index];
    }
    public GameObject GetStrongest(int index)
    {
        return strongest[index];
    }
    public void SetstrongestSpawned(int index, bool a)
    {
        strongestSpawned[index] = a;
    }
}

