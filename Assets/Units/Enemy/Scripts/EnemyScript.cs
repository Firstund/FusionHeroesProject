using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    //자동소환, 소환대기시간 설정할것
    private FusionManager fusionManager = null;
    private StageManager stageManager = null;
    private GameManager gameManager = null;

    [SerializeField]
    private bool _isStopByEnemy = true;
    public bool isStopByEnemy
    {
        get { return _isStopByEnemy; }
    }

    [SerializeField]
    private UnitOnMiniMapScript unitOnMiniMap = null;
    [SerializeField]
    private GameObject projection = null;
    [SerializeField]
    private Transform projectionSpawnPosition = null;

    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private UnitScript _shortestScript = null;
    public UnitScript shortestScript
    {
        get { return _shortestScript; }
        set { _shortestScript = value; }
    }

    [SerializeField]
    private EnemyScript LShortestEnemyScript = null;

    [SerializeField]
    private Strongest1Script strongest1Script = null;

    [SerializeField]
    private AudioSource audi = null;
    [SerializeField]
    private AudioClip attackSound = null;
    private Animator anim = null;

    //단일공격때 필요

    [SerializeField]
    private float _attackDistance = 2f;
    public float attackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
    }
    // private int stopByObjectDistance = 0;
    [SerializeField]
    private float minimumD = 0f;
    [SerializeField]
    private float maximumD = 3f;

    [SerializeField]
    private float heart = 100f;
    private float firstHeart = 0f;
    [SerializeField]
    private float heartUp = 1000f;

    [SerializeField]
    private float ap = 2f;
    private float firstAp = 0f;
    [SerializeField]
    private float apUp = 1f;

    [SerializeField]
    private float dp = 2f;
    private float firstDp = 0f;
    [SerializeField]
    private float dpUp = 0.25f;

    [SerializeField]
    private float attackDelay = 2f;
    [SerializeField]
    private float speed = 0f;
    private float firstSpeed = 0f;
    [SerializeField]
    private float totalAtk = 0f;

    private float shortestDp = 0f;
    private float shortestHeart = 0f;

    [SerializeField]
    private int plusMoney = 25;
    [SerializeField]
    private float stopByObjectDistance = 1f;
    [SerializeField]
    private float stopByEnemyDistance = 1.5f;
    private float firstStopByObjectDistance = 0f;
    private int thisUnitNum = 0; // 현재 오브젝트들중 몇번째 오브젝트인가 -> 공격체크에 쓰임
    private double thisUnitNO = 0; // 게임 전체에서 몇번째 소환됬는가 -> 유닛의 이동에 쓰임

    [SerializeField]
    private float[] objectDistanceArray;
    [SerializeField]
    private float[] enemyObjectDistanceArray;

    private float _shortestDistance = 100f;
    public float shortestDistance
    {
        get { return _shortestDistance; }
        set { _shortestDistance = value; }
    }
    private float shortestForwardDistance = 10f;
    private float shortestEnemyDistance = 10f;

    private bool _attackedCheck = false;
    public bool attackedCheck
    {
        get { return _attackedCheck; }
        set { _attackedCheck = value; }
    }
    private bool _attackAnimIsPlaying = false;
    public bool attackAnimIsPlaying
    {
        get { return _attackAnimIsPlaying; }
        set { _attackAnimIsPlaying = value; }
    }
    private bool _buildingIsShortest = false;//building이 shortest일 때 true. unit이 shortest일 때 false
    public bool buildingIsShortest
    {
        get { return _buildingIsShortest; }
        set { _buildingIsShortest = value; }
    }
    private bool isAttackOne = true;
    private bool isDead = false;
    private bool _canSetSpeed = true;
    public bool canSetSpeed
    {
        get { return _canSetSpeed; }
        set { _canSetSpeed = value; }
    }

    private Vector2 currentPosition = new Vector2(100f, 100f);
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

            if (gameManager.GetCST())
            {
                EDCheck();
                ODCheck();
            }
            if (gameManager.GetCST())
            {
                AttackCheck();
            }

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
    private void HealthBar()
    {
        slider.DOValue(heart, gameManager.dovalueTime);
    }
    private void CheckHe()
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
    private void setStat()
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
    private void Move()
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

            if (LShortestEnemyScript != null)
            {
                if (LShortestEnemyScript.isStopByEnemy && isStopByEnemy)
                {
                    if ((shortestForwardDistance >= stopByEnemyDistance) && (shortestDistance > stopByObjectDistance) && canSetSpeed)
                        speed = firstSpeed;
                    else
                    {
                        speed = 0f;
                    }
                }
                else
                {
                    if ((shortestDistance >= stopByObjectDistance) && canSetSpeed)
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
    private IEnumerator ReTrue(float time)
    {
        yield return new WaitForSeconds(time);
        attackedCheck = false;
    }
    private void Attack()
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
    public void SpawnProjection()
    {
        Instantiate(projection, projectionSpawnPosition);
    }
    public void GetDamage()
    {
        if (shortestDistance < attackDistance)
        {
            bool attackOne = isAttackOne;
            float minimumD = 0f;
            float maximumD = attackDistance;

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
    public void PlayAttackSound()
    {
        if (audi.clip != attackSound)
            audi.clip = attackSound;

        audi.Play();
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
    private IEnumerator AttackSkill(bool attackOne, float ap, float attackDelay, float minimumD, float maximumD)
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

            yield return new WaitForSeconds(attackDelay);
            SetObjectDistanceArray();

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

    private void AttackCheck()
    {
        if (shortestDistance < attackDistance)
        {
            Attack();
        }
    }
    private void FirstODSet() // FirstEDSet
    {
        objectDistanceArray[0] = Vector2.Distance(gameManager.GetUnitSpawnPosition().position, currentPosition);

        //buildingScript를 shortest로 지정
        buildingIsShortest = true;
        shortestDistance = objectDistanceArray[0];

    }
    private void FirstEDSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(gameManager.GetEnemyUnitSpawnPosition().position, currentPosition);

        shortestEnemyDistance = enemyObjectDistanceArray[0];
    }
    private void ODCheck()
    {
        if (shortestScript == null || shortestScript.isDead)
        {
            SetObjectDistanceArray();
        }
        else
        {
            shortestDistance = Vector2.Distance(shortestScript.GetCurrentPosition(), currentPosition);
            buildingIsShortest = false;
        }
    }
    private void SetObjectDistanceArray()
    {
        UnitScript _ShortestScript = null;

        FirstODSet();
        if (fusionManager.GetUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
            {
                objectDistanceArray[a + 1] = Vector2.Distance(fusionManager.unitScript[a].GetCurrentPosition(), currentPosition);

                if (objectDistanceArray[a + 1] < shortestDistance && fusionManager.unitScript[a].GetCurrentPosition().x <= currentPosition.x + 0.5f)
                {
                    shortestDistance = objectDistanceArray[a + 1];
                    _ShortestScript = fusionManager.unitScript[a];
                    buildingIsShortest = false;
                }
            }
            shortestScript = _ShortestScript;
        }
    }
    public void EDCheck()
    {
        if (LShortestEnemyScript == null || LShortestEnemyScript.GetIsDead()) // 최적화 위한 코드
        {
            SetEnemyObjectDistanceArray();
        }
        else
        {
            shortestForwardDistance = Vector2.Distance(LShortestEnemyScript.GetCurrentPosition(), currentPosition);
        }
    }

    private void SetEnemyObjectDistanceArray()
    {
        EnemyScript _LShortestEnemyScript = null;
        bool shortestForwardIsSet = false;
        float LShortestForwardDistance = 100f;

        FirstEDSet();
        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
            {
                enemyObjectDistanceArray[a + 1] = Vector2.Distance(fusionManager.enemyScript[a].GetCurrentPosition(), currentPosition);

                if (fusionManager.enemyScript[a].GetThisUnitNO() < thisUnitNO) // unitScript에도 이거 적용할것
                {
                    if (enemyObjectDistanceArray[a + 1] < LShortestForwardDistance)
                    {
                        LShortestForwardDistance = enemyObjectDistanceArray[a + 1];
                        _LShortestEnemyScript = fusionManager.enemyScript[a];
                        shortestForwardDistance = LShortestForwardDistance;
                        shortestForwardIsSet = true;
                    }
                }
                if (!shortestForwardIsSet)
                {
                    shortestForwardDistance = 10f;
                }
            }

            LShortestEnemyScript = _LShortestEnemyScript;
        }
    }

    private void DestroyCheck()
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
