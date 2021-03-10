﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //자동소환, 소환대기시간 설정할것
    FusionManager fusionManager = null;
    GameManager gameManager = null;

    //private TestUnitScript testUnitScript = null;
    [SerializeField]
    private UnitScript shortestScript = null;
    [SerializeField]
    private EnemyScript shortestEnemyScript = null;
    [SerializeField]
    private Strongest1Script strongest1Script = null;

    [SerializeField]
    private AudioSource audi = null;
    private Animator anim = null;

    //단일공격때 필요

    [SerializeField]
    private float attackDistance = 2f;
    [SerializeField]
    private float minimumD = 0f;
    [SerializeField]
    private float maximumD = 3f;

    [SerializeField]
    private float heart = 100f;
    [SerializeField]
    private float ap = 2f;
    [SerializeField]
    private float dp = 2f;
    [SerializeField]
    private float attackDelay = 2f;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float totalAtk = 0f;

    private float shortestDp = 0f;
    private float shortestHeart = 0f;

    [SerializeField]
    private int plusMoney = 25;
    private int thisUnitNum = 0;

    [SerializeField]
    private float[] objectDistanceArray;
    [SerializeField]
    private float[] enemyObjectDistanceArray;

    [SerializeField]
    private float shortestDistance = 100f;
    [SerializeField]
    private float shortestForwardDistance = 10f;
    [SerializeField]
    private float shortestEnemyDistance = 10f;

    [SerializeField]
    private bool attackedCheck = false;
    [SerializeField]
    private bool buildingIsShortest = false;//building이 shortest일 때 true. unit이 shortest일 때 false
    [SerializeField]
    private bool attackOne = true;

    private Vector2 currentPosition = Vector2.zero;//

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.SetParent(null, true);

        fusionManager = FindObjectOfType<FusionManager>();
        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();
        strongest1Script = GetComponent<Strongest1Script>();

        int enemyUnitNum = fusionManager.GetEnemyUnitNum() + 1;
        thisUnitNum = enemyUnitNum;
        fusionManager.SetEnemyUnitNum(enemyUnitNum);
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;

        objectDistanceArray = new float[fusionManager.GetUnitNum()];
        enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];

        EDCheck();
        ODCheck();

        if (gameManager.GetCST())
            AttackCheck();
        Move();
        DestroyCheck();
    }
    public Vector2 GetCurrentPosition()
    {
        return currentPosition;
    }
    public void SetCurrentPosition(Vector2 a)
    {
        currentPosition = a;
    }
    public float getHe(float he)
    {
        he = heart;
        return he;
    }
    public float getD(float d)
    {
        d = dp;
        return d;

    }
    public void setStat(float he, float d)
    {
        heart = he;
        dp = d;
    }
    public float GetShortestDistance()
    {
        return shortestDistance;
    }
    public UnitScript GetShortest()
    {
        return shortestScript;
    }
    public void AttackedCheck(float time)
    {
        attackedCheck = true;
        StartCoroutine(ReTrue(time));
    }
    void Move()
    {
        int stopByObjectDistance = 1;

     
            if (!attackedCheck)
                anim.Play("WalkL");

            if (buildingIsShortest)
            {
                stopByObjectDistance = 5;
            }

            if ((shortestForwardDistance > 1) && (shortestDistance > stopByObjectDistance))
                transform.Translate(Vector2.left * speed * Time.deltaTime);
    
    }
    private IEnumerator ReTrue(float time)
    {
        yield return new WaitForSeconds(time);
        attackedCheck = false;

    }
    private IEnumerator Attack(bool attackOne)
    {
        ////단일공격
        if (strongest1Script == null)
        {
            if (attackOne)
            {

                if (!attackedCheck)
                {
                    attackedCheck = true;
                    anim.Play("AttackL");

                    yield return new WaitForSeconds(attackDelay);
                    //공격 애니메이션 출력
                    audi.Play();

                    if (buildingIsShortest)
                    {
                        shortestHeart = fusionManager.buildingScript.getHe();
                        shortestDp = fusionManager.buildingScript.getD();

                        totalAtk = (ap - shortestDp);

                        if (totalAtk <= 0)
                        {
                            totalAtk = 1;
                        }
                        shortestHeart -= totalAtk;
                        fusionManager.buildingScript.setStat(shortestHeart, shortestDp);
                    }
                    else if (shortestScript != null)
                    {
                        shortestHeart = shortestScript.getHe();
                        shortestDp = shortestScript.getD();

                        totalAtk = (ap - shortestDp);//데미지 공식 적용

                        if (totalAtk <= 0)
                        {
                            totalAtk = 1;
                        }
                        shortestHeart -= totalAtk; //단일공격
                        shortestScript.setStat(shortestHeart, shortestDp);

                    }
                    attackedCheck = false;
                }
            }
        }
        else if (!strongest1Script.GetSkillUsed())
            if (attackOne)
            {

                if (!attackedCheck)
                {
                    attackedCheck = true;
                    anim.Play("AttackL");

                    yield return new WaitForSeconds(attackDelay);
                    //공격 애니메이션 출력
                    audi.Play();

                    if (buildingIsShortest)
                    {
                        shortestHeart = fusionManager.buildingScript.getHe();
                        shortestDp = fusionManager.buildingScript.getD();

                        totalAtk = (ap - shortestDp);

                        if (totalAtk <= 0)
                        {
                            totalAtk = 1;
                        }
                        shortestHeart -= totalAtk;
                        fusionManager.buildingScript.setStat(shortestHeart, shortestDp);
                    }
                    else if (shortestScript != null)
                    {
                        shortestHeart = shortestScript.getHe();
                        shortestDp = shortestScript.getD();

                        totalAtk = (ap - shortestDp);//데미지 공식 적용

                        if (totalAtk <= 0)
                        {
                            totalAtk = 1;
                        }
                        shortestHeart -= totalAtk; //단일공격
                        shortestScript.setStat(shortestHeart, shortestDp);

                    }
                    attackedCheck = false;
                }
            }
            else
            {
                //광역공격
                if (!attackedCheck)
                {
                    attackedCheck = true;
                    //anim.Play("TestAnimationAttack");
                    yield return new WaitForSeconds(attackDelay);
                    //공격 애니메이션 출력
                    anim.Play("AttackL");

                    for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
                    {
                        if (objectDistanceArray[a] < maximumD && objectDistanceArray[a] >= minimumD)//minimum, maxism attackDistance를 이용하여 공격 범위 설정가능
                        {
                            float dp = fusionManager.unitScript[a].getD();
                            float heart = fusionManager.unitScript[a].getHe();

                            totalAtk = (ap - dp);

                            if (totalAtk <= 0)
                            {
                                totalAtk = 1;
                            }

                            heart -= totalAtk;

                            fusionManager.unitScript[a].setStat(heart, dp);

                        }
                    }
                    attackedCheck = false;
                }
            }

}
public void DoAttackSkill(bool attackOne, float ap, float attackDelay, float minimumD, float maximumD)
    {
        StartCoroutine(AttackSkill(attackOne, ap, attackDelay, minimumD, maximumD));
    }
    private IEnumerator AttackSkill(bool attackOne, float ap, float attackDelay, float minimumD, float maximumD)
    {
        //단일공격

            if (attackOne)
            {

                yield return new WaitForSeconds(attackDelay);

                audi.Play();

                if (buildingIsShortest)
                {
                    shortestHeart = fusionManager.buildingScript.getHe();
                    shortestDp = fusionManager.buildingScript.getD();

                    totalAtk = (ap - shortestDp);

                    if (totalAtk <= 0)
                    {
                        totalAtk = 1;
                    }
                    shortestHeart -= totalAtk;
                    fusionManager.buildingScript.setStat(shortestHeart, shortestDp);
                }
                else if (shortestScript != null)
                {
                    shortestHeart = shortestScript.getHe();
                    shortestDp = shortestScript.getD();

                    totalAtk = (ap - shortestDp);//데미지 공식 적용

                    if (totalAtk <= 0)
                    {
                        totalAtk = 1;
                    }
                    shortestHeart -= totalAtk; //단일공격
                    shortestScript.setStat(shortestHeart, shortestDp);

                }
            }
            else
            {
                //광역공격

                attackedCheck = true;
                //anim.Play("TestAnimationAttack");
                yield return new WaitForSeconds(attackDelay);

                for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
                {
                if (objectDistanceArray[a] < maximumD && objectDistanceArray[a] >= minimumD)//minimum, maxism attackDistance를 이용하여 공격 범위 설정가능
                    {
                        float dp = fusionManager.unitScript[a].getD();
                        float heart = fusionManager.unitScript[a].getHe();

                        totalAtk = (ap - dp);

                        if (totalAtk <= 0)
                        {
                            totalAtk = 1;
                        }

                        heart -= totalAtk;

                    fusionManager.unitScript[a].setStat(heart, dp);

                    }
                }
            }
    }
    private void AttackCheck()
    {
        if (shortestDistance < attackDistance)
        {
            StartCoroutine(Attack(attackOne));
        }
    }
    private void FirstODSet()
    {
            objectDistanceArray[0] = Vector2.Distance(fusionManager.buildingScript.currentPosition, currentPosition);
            
                //buildingScript를 shortest로 지정
                buildingIsShortest = true;
                shortestDistance = objectDistanceArray[0];
            
    }
    private void ODCheck()
    {
        FirstODSet();
        if (fusionManager.GetUnitNum() > 0)
        {
            for (int a = 0; a < (fusionManager.GetUnitNum() - 1); a++)
            {
                objectDistanceArray[a] = Vector2.Distance(fusionManager.unitScript[a].GetCurrentPosition(), currentPosition);

                if (objectDistanceArray[a] < shortestDistance && fusionManager.unitScript[a].GetCurrentPosition().x <= currentPosition.x + 0.5f)
                {
                    shortestDistance = objectDistanceArray[a];
                    shortestScript = fusionManager.unitScript[a];
                    buildingIsShortest = false;
                }
            }
        }
        else
        {
            attackedCheck = false;
            shortestDistance = 100f;
            shortestScript = null;
        }
    }
    public void EDCheck()
    {
        float LShortestForwardDistance = 100f;

        FirstODSet();
        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
            {
                enemyObjectDistanceArray[a] = Vector2.Distance(fusionManager.enemyScript[a].GetCurrentPosition(), currentPosition);

                if (enemyObjectDistanceArray[a] < shortestEnemyDistance)  // shortest 갱신을 위한 조건문                                                                                     
                {
                    bool arrayDistanceCheck = (enemyObjectDistanceArray[a] == 0);

                    if (!arrayDistanceCheck)
                    {
                        shortestEnemyScript = fusionManager.enemyScript[a];
                        shortestEnemyDistance = enemyObjectDistanceArray[a];
                        buildingIsShortest = false;
                    }
                }
                if(fusionManager.enemyScript[a].GetThisUnitNum() < thisUnitNum)
                {
                    if (enemyObjectDistanceArray[a] < LShortestForwardDistance)
                    {
                        LShortestForwardDistance = enemyObjectDistanceArray[a];
                        shortestForwardDistance = LShortestForwardDistance;
                    }
                }
            }
        }
        else
        {
            attackedCheck = false;
            shortestEnemyDistance = 100f;
            shortestEnemyScript = null;
        }
    }
    private void DestroyCheck()
    {
        if (heart <= 0f)
            Destroye();
    }
    private void Destroye()
    {
        int enemyUnitNum = fusionManager.GetEnemyUnitNum() - 1;
        fusionManager.SetEnemyUnitNum(enemyUnitNum);

        int money = gameManager.GetMoney() + plusMoney;
        gameManager.SetMoney(money);

        Destroy(gameObject);
    }
    public int GetThisUnitNum()
    {
        return thisUnitNum;
    }
}