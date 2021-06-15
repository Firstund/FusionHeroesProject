using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    //자동소환, 소환대기시간 설정할것
    protected FusionManager fusionManager = null;
    protected StageManager stageManager = null;
    protected GameManager gameManager = null;

    [SerializeField]
    protected bool _isStopByEnemy = true;
    public bool isStopByEnemy
    {
        get { return _isStopByEnemy; }
    }
    [SerializeField]
    protected UnitOnMiniMapScript unitOnMiniMap = null;
    [SerializeField]
    protected Slider slider = null;
    [SerializeField]
    protected UnitScript shortestScript = null;
    [SerializeField]
    protected EnemyScript shortestEnemyScript = null;
    
    [SerializeField]
    protected Strongest1Script strongest1Script = null;

    [SerializeField]
    protected AudioSource audi = null;
    [SerializeField]
    protected AudioClip attackSound = null;
    protected Animator anim = null;

    //단일공격때 필요

    [SerializeField]
    protected float attackDistance = 2f;
    // protected int stopByObjectDistance = 0;
    [SerializeField]
    protected float minimumD = 0f;
    [SerializeField]
    protected float maximumD = 3f;

    [SerializeField]
    protected float heart = 100f;
    protected float firstHeart = 0f;
    [SerializeField]
    protected float heartUp = 1000f;

    [SerializeField]
    protected float ap = 2f;
    protected float firstAp = 0f;
    [SerializeField]
    protected float apUp = 1f;

    [SerializeField]
    protected float dp = 2f;
    protected float firstDp = 0f;
    [SerializeField]
    protected float dpUp = 0.25f;

    [SerializeField]
    protected float attackDelay = 2f;
    [SerializeField]
    protected float speed = 0f;
    protected float firstSpeed = 0f;
    [SerializeField]
    protected float totalAtk = 0f;

    protected float shortestDp = 0f;
    protected float shortestHeart = 0f;

    [SerializeField]
    protected int plusMoney = 25;
    [SerializeField]
    protected float stopByObjectDistance = 1f;
    [SerializeField]
    protected float stopByEnemyDistance = 1.5f;
    protected float firstStopByObjectDistance = 0f;
    protected int thisUnitNum = 0; // 현재 오브젝트들중 몇번째 오브젝트인가 -> 공격체크에 쓰임
    protected double thisUnitNO = 0; // 게임 전체에서 몇번째 소환됬는가 -> 유닛의 이동에 쓰임

    [SerializeField]
    protected float[] objectDistanceArray;
    [SerializeField]
    protected float[] enemyObjectDistanceArray;

    protected float shortestDistance = 100f;
    protected float shortestForwardDistance = 10f;
    protected float shortestEnemyDistance = 10f;

    protected bool _attackedCheck = false;
    public bool attackedCheck
    {
        get { return _attackedCheck; }
        set { _attackedCheck = value; }
    }
    protected bool _attackAnimIsPlaying = false;
    public bool attackAnimIsPlaying
    {
        get { return _attackAnimIsPlaying; }
        set { _attackAnimIsPlaying = value; }
    }
    protected bool buildingIsShortest = false;//building이 shortest일 때 true. unit이 shortest일 때 false
    protected bool isAttackOne = true;
    protected bool isDead = false;
    protected bool _canSetSpeed = true;
    public bool canSetSpeed
    {
        get { return _canSetSpeed; }
        set { _canSetSpeed = value; }
    }

    protected Vector2 currentPosition = new Vector2(100f, 100f);
    void Awake()
    {
        gameObject.transform.SetParent(GameObject.Find("EnemyUnits").gameObject.transform, true);
        fusionManager = FindObjectOfType<FusionManager>();

        fusionManager.SetCanSetScripts();

        gameManager = GameManager.Instance;

        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();
        strongest1Script = GetComponent<Strongest1Script>();

        firstSpeed = speed;
    }
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        fusionManager.SetEnemyUnitNum(thisUnitNum = fusionManager.GetEnemyUnitNum() + 1);

        fusionManager.SetEnemyUnitNO(thisUnitNO = fusionManager.GetEnemyUnitNO() + 1d);

        firstHeart = heart;
        firstAp = ap;
        firstDp = dp;
        firstStopByObjectDistance = stopByObjectDistance;

        setStat();
        SetDistanceArrayIndex();


        SetMaxHealth();

        Instantiate(unitOnMiniMap.gameObject, GameObject.Find("MapSlider").transform).GetComponent<UnitOnMiniMapScript>().targetUnitTrm = this.gameObject.transform;

    }

    void Update()
    {
        if (!gameManager.tutoIsPlaying)
        {
            currentPosition = transform.localPosition;
            audi.volume = gameManager.GetSoundValue();

            SetDistanceArrayIndex();

            EDCheck();
            ODCheck();

            if (gameManager.GetCST())
                AttackCheck();

            Move();
            HealthBar();
            DestroyCheck();
        }
    }
    public void SetDistanceArrayIndex()
    {
        if (objectDistanceArray.Length != fusionManager.GetUnitNum())
            objectDistanceArray = new float[fusionManager.GetUnitNum()];

        if (enemyObjectDistanceArray.Length != fusionManager.GetEnemyUnitNum())
            enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];
    }
    public void SetMaxHealth()
    {
        slider.maxValue = heart;
        slider.value = heart;
        slider.minValue = 0;
    }
    protected void HealthBar()
    {
        slider.DOValue(heart, gameManager.dovalueTime);
    }
    protected void CheckHe()
    {
        if (heart <= 0)
        {
            ap = 0f;
            speed = 0f;
        }
    }
    public Vector2 GetCurrentPosition()
    {
        return currentPosition;
    }
    public void SetCurrentPosition(Vector2 a)
    {
        currentPosition = a;
    }
    public float getHe()
    {
        return heart;
    }
    public float GetFirstHP()
    {
        return firstHeart;
    }
    public float getD()
    {
        return dp;

    }
    public void SetHP(float he)
    {
        heart = he;
    }
    protected void setStat()
    {
        heart = firstHeart + heartUp * stageManager.GetCurrentStage(); // heartUp * 라운드 수
        ap = firstAp + apUp * stageManager.GetCurrentStage();
        dp = firstDp + dpUp * stageManager.GetCurrentStage();
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
    public bool GetAttackedCheck()
    {
        return attackedCheck;
    }
    protected void Move()
    {

        if (!isDead)
        {
            if (attackDistance < stopByObjectDistance)
            {
                stopByObjectDistance = attackDistance;
            }

            if (enemyObjectDistanceArray[0] < stopByObjectDistance)
            {
                stopByObjectDistance = firstStopByObjectDistance + 0.5f;
            }
            else
            {
                stopByObjectDistance = firstStopByObjectDistance;
            }

            if (!attackAnimIsPlaying && !isDead)
                anim.Play("WalkL");

            if (shortestScript != null)
                if (shortestScript.getHe() <= 0f)
                {
                    speed = 0f;
                }
                else
                {
                    speed = firstSpeed;
                }

            if (shortestEnemyScript != null)
            {
                if (shortestEnemyScript.isStopByEnemy && isStopByEnemy)
                {
                    if ((shortestForwardDistance > stopByEnemyDistance) && (shortestDistance > stopByObjectDistance) && canSetSpeed)
                        speed = firstSpeed;
                    else
                    {
                        speed = 0f;
                    }
                }
                else
                {
                    if ((shortestDistance > stopByObjectDistance) && canSetSpeed)
                        speed = firstSpeed;
                    else
                        speed = 0f;
                }
            }
            else
            {
                if ((shortestDistance > stopByObjectDistance) && canSetSpeed)
                    speed = firstSpeed;
                else
                    speed = 0f;
            }

            CheckHe();

            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
    protected IEnumerator ReTrue(float time)
    {
        yield return new WaitForSeconds(time);
        attackedCheck = false;
    }
    protected void Attack()
    {
        ////단일공격
        if (shortestDistance < attackDistance)
        {
            if (strongest1Script == null)
            {
                if (!attackedCheck)
                {
                    attackedCheck = true;
                    attackAnimIsPlaying = true;

                    if (!isDead)
                    {
                        if (speed <= 0f)
                            anim.Play("AttackL");
                        else
                        {
                            anim.Play("WalkAttackL");
                        }
                        Invoke("ResetAttackedCheck", attackDelay);
                    }

                }
            }
            else if (!strongest1Script.GetSkillUsed())
                if (!attackedCheck)
                {
                    attackedCheck = true;
                    attackAnimIsPlaying = true;

                    if (!isDead)
                        anim.Play("AttackL");
                    Invoke("ResetAttackedCheck", attackDelay);
                    //공격 애니메이션 출력
                }

        }
    }
    public void GetDamage()
    {
        if (shortestDistance < attackDistance)
        {
            bool attackOne = isAttackOne;
            float minimumD = 0f;
            float maximumD = attackDistance;
            if (audi.clip != attackSound)
                audi.clip = attackSound;
            //공격 애니메이션 출력
            audi.Play();

            if (attackOne)
            {
                if (buildingIsShortest)
                {
                    shortestHeart = fusionManager.buildingScript.getHe();
                    shortestDp = fusionManager.buildingScript.getD();

                    totalAtk = (ap - shortestDp);

                    if (totalAtk <= 0f)
                    {
                        totalAtk = 0.2f;
                    }

                    shortestHeart -= totalAtk;

                    fusionManager.buildingScript.SetHP(shortestHeart);
                }
                else if (shortestScript != null)
                {
                    shortestHeart = shortestScript.getHe();
                    shortestDp = shortestScript.getD();

                    totalAtk = (ap - shortestDp);//데미지 공식 적용

                    if (totalAtk <= 0f)
                    {
                        totalAtk = 0.2f;
                    }
                    shortestHeart -= totalAtk; //단일공격
                    shortestScript.SetHP(shortestHeart);
                }
            }
            else
            {
                attackedCheck = true;

                for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
                {
                    if (objectDistanceArray[a] < maximumD && objectDistanceArray[a] >= minimumD)//minimum, maxism attackDistance를 이용하여 공격 범위 설정가능
                    {
                        float dp = fusionManager.unitScript[a].getD();
                        float heart = fusionManager.unitScript[a].getHe();

                        totalAtk = (ap - dp);

                        if (totalAtk <= 0f)
                        {
                            totalAtk = 0.2f;
                        }


                        heart -= totalAtk;

                        fusionManager.unitScript[a].SetHP(heart);
                    }
                }
            }
        }
    }
    public void ResetAttackedCheck()
    {
        attackedCheck = false;
    }
    public void ResetAttackAnimIsPlaying()
    {
        attackAnimIsPlaying = false;
    }
    public void DoAttackSkill(bool attackOne, float ap, float attackDelay, float minimumD, float maximumD)
    {
        StartCoroutine(AttackSkill(attackOne, ap, attackDelay, minimumD, maximumD));
    }
    protected IEnumerator AttackSkill(bool attackOne, float ap, float attackDelay, float minimumD, float maximumD)
    {
        //단일공격

        if (attackOne)
        {

            yield return new WaitForSeconds(attackDelay);

            if (buildingIsShortest)
            {
                shortestHeart = fusionManager.buildingScript.getHe();
                shortestDp = fusionManager.buildingScript.getD();

                totalAtk = (ap - shortestDp);

                if (totalAtk <= 0f)
                {
                    totalAtk = 0.2f;
                }
                shortestHeart -= totalAtk;
                fusionManager.buildingScript.SetHP(shortestHeart);
            }
            else if (shortestScript != null)
            {
                shortestHeart = shortestScript.getHe();
                shortestDp = shortestScript.getD();

                totalAtk = (ap - shortestDp);//데미지 공식 적용

                if (totalAtk <= 0f)
                {
                    totalAtk = 0.2f;
                }

                shortestHeart -= totalAtk; //단일공격

                shortestScript.SetHP(shortestHeart);
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

                    if (totalAtk <= 0f)
                    {
                        totalAtk = 0.2f;
                    }


                    heart -= totalAtk;

                    fusionManager.unitScript[a].SetHP(heart);
                }
            }
        }
    }
    protected void AttackCheck()
    {
        if (shortestDistance < attackDistance)
        {
            Attack();
        }
    }
    protected void FirstODSet() // FirstEDSet
    {
        objectDistanceArray[0] = Vector2.Distance(gameManager.GetUnitSpawnPosition().position, currentPosition);

        //buildingScript를 shortest로 지정
        buildingIsShortest = true;
        shortestDistance = objectDistanceArray[0];

    }
    protected void FirstEDSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(gameManager.GetEnemyUnitSpawnPosition().position, currentPosition);

        shortestEnemyDistance = enemyObjectDistanceArray[0];
    }
    protected void ODCheck()
    {
        FirstODSet();
        if (fusionManager.GetUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
            {
                objectDistanceArray[a + 1] = Vector2.Distance(fusionManager.unitScript[a].GetCurrentPosition(), currentPosition);

                if (objectDistanceArray[a + 1] < shortestDistance && fusionManager.unitScript[a].GetCurrentPosition().x <= currentPosition.x + 0.5f)
                {
                    bool arrayDistanceCheck = (objectDistanceArray[a + 1] == 0);

                    if (!arrayDistanceCheck)
                    {
                        shortestDistance = objectDistanceArray[a + 1];
                        shortestScript = fusionManager.unitScript[a];
                        buildingIsShortest = false;
                    }
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
        bool shortestForwardIsSet = false;
        float LShortestForwardDistance = 100f;

        FirstEDSet();
        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
            {
                enemyObjectDistanceArray[a + 1] = Vector2.Distance(fusionManager.enemyScript[a].GetCurrentPosition(), currentPosition);

                if (enemyObjectDistanceArray[a + 1] < shortestEnemyDistance)  // shortest 갱신을 위한 조건문                                                                                     
                {
                    bool arrayDistanceCheck = (enemyObjectDistanceArray[a + 1] == 0);

                    if (!arrayDistanceCheck)
                    {
                        shortestEnemyScript = fusionManager.enemyScript[a];
                        shortestEnemyDistance = enemyObjectDistanceArray[a + 1];
                        buildingIsShortest = false;
                    }
                }
                if (fusionManager.enemyScript[a].GetThisUnitNO() < thisUnitNO) // unitScript에도 이거 적용할것
                {
                    if (enemyObjectDistanceArray[a + 1] < LShortestForwardDistance)
                    {
                        LShortestForwardDistance = enemyObjectDistanceArray[a + 1];
                        shortestForwardDistance = LShortestForwardDistance;
                        shortestForwardIsSet = true;
                    }
                }
                if (!shortestForwardIsSet)
                {
                    shortestForwardDistance = 10f;
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
    protected void DestroyCheck()
    {
        // Destroy(unitOnMiniMap);

        if (heart <= 0f && !isDead)
        {
            anim.Play("Dead");
            ap = 0f;
            isDead = true;
            int enemyUnitNum = fusionManager.GetEnemyUnitNum() - 1;
            fusionManager.SetEnemyUnitNum(enemyUnitNum);
        }
    }
    public void Destroye()
    {

        int money = gameManager.GetMoney() + plusMoney;
        int hadMoney = 0;

        if (money >= gameManager.maxMoney)
            money = Mathf.Clamp(money, 0, gameManager.maxMoney);
        else
        {
            hadMoney = gameManager.hadMoney + plusMoney;
            gameManager.hadMoney = hadMoney;
        }


        gameManager.SetMoney(money);

        stageManager.killedEnemyUnitNum++;

        fusionManager.SetCanSetScripts();

        Destroy(gameObject);
    }
    public AudioSource GetAudi()
    {
        return audi;
    }
    public double GetThisUnitNO()
    {
        return thisUnitNO;
    }
    public int GetThisUnitNum()
    {
        return thisUnitNum;
    }
    public bool GetIsDead()
    {
        return isDead;
    }
    public bool GetBuildingIsShortest()
    {
        return buildingIsShortest;
    }
    public float GetAttackDistance()
    {
        return attackDistance;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
