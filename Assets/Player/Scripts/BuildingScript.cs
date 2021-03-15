using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour
{
    FusionManager fusionManager = null;
    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private AudioClip destroy1 = null;

    private Animator anim = null;

    [SerializeField]
    private float heart = 10000f;
    private float dp = 10f;

    private float firstHeart = 0f;

    private bool destroy1Played = false;
    private bool destroy2Played = false;

    public Vector2 currentPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        fusionManager = FindObjectOfType<FusionManager>();

        audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        int unitNum = fusionManager.GetUnitNum() + 1;
        fusionManager.SetUnitNum(unitNum);
       
        firstHeart = heart;
        SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;
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
    public void setStat(float he, float d)
    {
        heart = he;
        dp = d;
    }
    void breaking()
    {
        if (heart <= 0)
        {
            anim.Play("destory2Idle");
            if (!destroy2Played)
            {
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
