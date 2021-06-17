using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitScript : MonoBehaviour
{
    //공격대기시간, 소환 대기시간 설정할것.
    protected FusionManager fusionManager = null;
    protected GameManager gameManager = null;
    protected StageManager stageManager = null;
    [SerializeField]
    protected Slider slider = null;
    private MapSliderScript mapSliderScript = null;

    [SerializeField]
    protected int unitId = 01; // 유닛의 종류
                               // 첫번째 자리: (이 유닛을 소환하기 위해 필요한 퓨전 수 + 1)
                               // 두번째 자리: 0 (특수 케이스가 떠오를 때를 대비) // 특수케이스->다른 유닛에 의해 소환된경우 그 유닛의 ID에, 두번째자리만 변경. 
                               // 두번째 자리는 이 유닛이 소환자 유닛의 몇번째 소환수인지를 의미.
                               // 세번째 자리: unitId의 첫번째 자리가 같은 숫자의 그룹 중 이 유닛이 생긴 순서
    public int unitStatIndex
    {
        get { return (unitId / 50 + ((unitId % 100) % 10)); }
    }
    // [SerializeField]
    // protected int[] fusionUnitId = new int[5]; // 이 유닛과 퓨전할 수 있는 유닛들의 유닛 아이디 
    //// 퓨전은 잠시 보류 (사유: 게임은 퓨전이 없어도 충분히 복잡하다.)
    // [SerializeField]
    // protected GameObject[] nextUnit = new GameObject[5]; // 이 유닛이 퓨전한 후 나올수 있는 유닛들,
    //                                                      // 인덱스 값은 해당 유닛의 아이디의 (세번째 숫자 - 1)로 설정한다.
    //                                                      // 첫번 째 배열에 들어가는 유닛의 아이디는 무조건 이 스크립트의 unitId값이다.
    [SerializeField]
    protected UnitOnMiniMapScript unitOnMiniMap = null;
    [SerializeField]
    protected GameObject projection = null;
    [SerializeField]
    protected Transform projectionSpawnPosition = null;
    [SerializeField]
    protected float _attackDistance = 2f;
    public float attackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
    }
    [SerializeField]
    protected float _heart = 100f;
    public float heart
    {
        get { return _heart; }
        set { _heart = value; }
    }
    protected float firstHeart = 0f;
    [SerializeField]
    protected float heartUp = 5f;
    [SerializeField]
    protected float plusHeartUpPerLev = 0.2f;
    [SerializeField]
    protected float _heartUpPerLev = 0.1f;
    public float heartUpPerLev
    {
        get { return _heartUpPerLev; }
        set { _heartUpPerLev = value; }
    }
    [SerializeField]
    protected float _ap = 2f;
    public float ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    protected float firstAp = 0f;
    [SerializeField]
    protected float apUp = 0.5f;
    [SerializeField]
    protected float plusApUpPerLev = 1f;
    [SerializeField]
    protected float _apUpPerLev = 0.02f;
    public float apUpPerLev
    {
        get { return _apUpPerLev; }
        set { _apUpPerLev = value; }
    }
    [SerializeField]
    protected float _dp = 2f;
    public float dp
    {
        get { return _dp; }
        set { _dp = value; }
    }
    protected float firstDp = 0f;
    [SerializeField]
    protected float dpUp = 0.25f;
    [SerializeField]
    protected float plusDpUpPerLev = 0.1f;
    [SerializeField]
    protected float _dpUpPerLev = 0.01f;
    public float dpUpPerLev
    {
        get { return _dpUpPerLev; }
        set { _dpUpPerLev = value; }
    }

    [SerializeField]
    protected float _speed = 0f;
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    [SerializeField]
    protected float attackDelay = 1f;
    protected float firstSpeed = 0f;
    [SerializeField]
    protected float totalAtk = 0f;

    [SerializeField]
    protected GameObject Lev = null;
    protected TextMesh levelText = null;
    [SerializeField]
    protected Text costText = null;

    protected float shortestHeart = 0f;
    protected float shortestDp = 0f;

    [SerializeField]
    protected UnitScript shortestScript = null;
    [SerializeField]
    protected EnemyScript _shortestEnemyScript = null;
    public EnemyScript shortestEnemyScript
    {
        get { return _shortestEnemyScript; }
        set { _shortestEnemyScript = value; }
    }

    [SerializeField]
    protected AudioSource _audi = null;
    public AudioSource audi
    {
        get { return _audi; }
        set { _audi = value; }
    }
    [SerializeField]
    protected AudioClip attackSound = null;
    protected Animator _anim = null;
    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    [SerializeField]
    protected bool onlyOneFollowUnitNum = false;
    [SerializeField]
    protected bool _isStopByObject = true;
    public bool isStopByObject
    {
        get { return _isStopByObject; }
    }

    [SerializeField]
    protected int thisUnitNum = 0; // 현재 소환된 오브젝트들 중 몇번째 오브젝트인가
    [SerializeField]
    protected double thisUnitNO = 0; // 게임 전체에서 몇 번째 소환된 오브젝트인가
    [SerializeField]
    protected int unitLev = 01; // 유닛의 레벨
    [SerializeField]
    protected int levelUpCost = 10;
    [SerializeField]
    protected float unitClickableRange = 0f;
    [SerializeField]

    private float firstUnitClickableRange = 0f;

    [SerializeField]
    protected float mouseDistance = 0f;

    [SerializeField]
    protected float[] objectDistanceArray;
    [SerializeField]
    protected float[] enemyObjectDistanceArray;

    [SerializeField]
    protected float _shortestDistance = 10f;
    public float shortestDistance
    {
        get { return _shortestDistance; }
        set { _shortestDistance = value; }
    }
    [SerializeField]
    protected float shortestForwardDistance = 10f;
    [SerializeField]
    protected float shortestBackwardDistance = 10f; // 넉백됬을 때 unitNO를 새로 설정해 줄 때 필요.
    [SerializeField]
    protected float stopByEnemyDistance = 1f;
    [SerializeField]
    protected float stopByObjectDistance = 1.5f;
    protected float firstStopByEnemyDistance = 0f;
    protected UnitScript shortestForwardScript = null;
    protected UnitScript shortestBackwardScript = null;

    protected float shortestEnemyDistance = 10f;
    //
    protected Vector2 currentPosition = new Vector2(100f, 100f);

    protected Vector2 targetPosition = Vector2.zero;


    protected Vector2 firstPosition = Vector2.zero;
    //
    protected bool firstPositionSet = false;
    protected bool followingCheck = false;
    protected bool _canAttack = true;
    public bool canAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    protected bool _attackAnimIsPlaying = false;
    public bool attackAnimIsPlaying
    {
        get { return _attackAnimIsPlaying; }
        set { _attackAnimIsPlaying = value; }
    }
    protected bool _buildingIsShortest = false;
    public bool buildingIsShortest
    {
        get { return _buildingIsShortest; }
        set { _buildingIsShortest = value; }
    }
    protected bool mouseCheck = false;
    protected bool followingMouse = false;
    protected bool isDead = false;
    private bool isAttackOne = true;
    public bool isFollow = false;
    private bool _canSetSpeed = true;
    public bool canSetSpeed
    {
        get { return _canSetSpeed; }
        set { _canSetSpeed = value; }
    }

    void Awake()
    {
        gameObject.transform.SetParent(GameObject.Find("Units").gameObject.transform, true);
        firstSpeed = speed;
        gameManager = GameManager.Instance;

        firstUnitClickableRange = unitClickableRange;

        fusionManager = FindObjectOfType<FusionManager>();

        fusionManager.SetCanSetScripts();

        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

        firstAp = ap;
        firstDp = dp;
        firstHeart = heart;
        firstStopByEnemyDistance = stopByEnemyDistance;
    }

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        fusionManager.SetUnitNum(thisUnitNum = fusionManager.GetUnitNum() + 1);

        fusionManager.SetUnitNO(thisUnitNO = fusionManager.GetUnitNO() + 1d);

        levelText = Lev.GetComponent<TextMesh>();

        mapSliderScript = FindObjectOfType<MapSliderScript>();

        SetDistanceArrayIndex();

        // unitStatIndex = (unitId / 50 + unitId % 100); // stat에 쓰일 Index설정

        setStat();

        SetMaxHealth();

        Instantiate(unitOnMiniMap.gameObject, GameObject.Find("MapSlider").transform).GetComponent<UnitOnMiniMapScript>().targetUnitTrm = this.gameObject.transform;
    }

    void Update()
    {
        if (!gameManager.tutoIsPlaying)
        {
            audi.volume = gameManager.GetSoundValue();
            levelText.text = string.Format("Level: {0}", unitLev); // 레벨 텍스트
            currentPosition = transform.localPosition;
            isFollow = fusionManager.GetIsFollow();

            SetDistanceArrayIndex();

            onClickMouse();

            ODCheck();
            if (gameManager.GetCST())
            {
                ForwardODCheck();
                BackwardODCheck();
                EDCheck();
            }
            FusionCheck();
            Move();
            HealthBar();
            DestroyCheck();

            if (gameManager.GetCST())
                AttackCheck();

            if (followingCheck)
            {
                gameManager.SetCSt(false);
            }
        }
    }
    public void SetDistanceArrayIndex()
    {
        if (objectDistanceArray.Length != fusionManager.GetUnitNum())
            objectDistanceArray = new float[fusionManager.GetUnitNum()];

        if (enemyObjectDistanceArray.Length != fusionManager.GetEnemyUnitNum())
            enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];
    }
    protected void Attack()
    {
        //단일공격
        if (canAttack && !onlyOneFollowUnitNum)
        {
            canAttack = false;
            attackAnimIsPlaying = true;

            if (!isDead)
            {
                if (speed <= 0f)
                    anim.Play("AttackR");
                else
                    anim.Play("WalkAttackR");
                Invoke("ResetAttackedCheck", attackDelay);

            }
            //공격 애니메이션 출력
        }
    }
    public void SpawnProjection()
    {
        Instantiate(projection, projectionSpawnPosition);
    }
    public void GetDamage()
    {

        if (shortestEnemyDistance < attackDistance)
        {

            bool attackOne = isAttackOne;
            float minimumD = 0f;
            float maximumD = attackDistance;


            if (attackOne)
            {
                if (buildingIsShortest)
                {
                    shortestHeart = fusionManager.enemyBuildingScript.getHe();
                    shortestDp = fusionManager.enemyBuildingScript.getD();

                    totalAtk = (ap - shortestDp);
                    if (totalAtk <= 0)
                    {
                        totalAtk = 1;
                    }

                    shortestHeart -= totalAtk;

                    fusionManager.enemyBuildingScript.SetHP(shortestHeart);
                }
                else if (shortestEnemyScript != null)
                {
                    shortestHeart = shortestEnemyScript.getHe();
                    shortestDp = shortestEnemyScript.getD();

                    totalAtk = (ap - shortestDp);//데미지 공식 적용

                    if (totalAtk <= 0)
                    {
                        totalAtk = 1;
                    }

                    shortestHeart -= totalAtk; //단일공격

                    shortestEnemyScript.SetHP(shortestHeart);
                }
            }
            else
            {
                for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
                {
                    if (enemyObjectDistanceArray[a] < maximumD && enemyObjectDistanceArray[a] >= minimumD)//minimum, maxism attackDistance를 이용하여 공격 범위 설정가능
                    {
                        float dp = fusionManager.enemyScript[a].getD();
                        float heart = fusionManager.enemyScript[a].getHe();

                        totalAtk = (ap - dp);

                        if (totalAtk <= 0f)
                        {
                            totalAtk = 0.2f;
                        }


                        heart -= totalAtk;

                        fusionManager.enemyScript[a].SetHP(heart);
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
        canAttack = true;
    }
    public void ResetAttackAnimIsPlaying()
    {
        attackAnimIsPlaying = false;
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
    #region GetSet
    public Vector2 GetCurrentPosition()
    {
        return currentPosition;
    }
    public void SetCurrentPosition(Vector2 a)
    {
        currentPosition = a;
    }
    public Vector2 GetTargetPosition()
    {
        return targetPosition;
    }
    public void SetTargetPosition(Vector2 a)
    {
        targetPosition = a;
    }
    public Vector2 GetFirstPosition()
    {
        return firstPosition;
    }
    public void SetFirstPosition(Vector2 a)
    {
        firstPosition = a;
    }
    public int GetUnitLev()
    {
        return unitLev;
    }
    public void SetUnitLev(int a)
    {
        unitLev = a;
    }
    public int GetUnitID()
    {
        return unitId;
    }
    public void SetUnitID(int a)
    {
        unitId = a;
    }
    public int GetThisUnitNum()
    {
        return thisUnitNum;
    }
    public void SetThisUnitNum(int a)
    {
        thisUnitNum = a;
    }
    public double GetThisUnitNO()
    {
        return thisUnitNO;
    }
    public void SetThisUnitNO(int a)
    {
        thisUnitNO = a;
    }
    public float getHe()
    {
        return heart;
    }
    public float getD()
    {
        return dp;
    }
    #endregion
    public void SetHP(float he)
    {
        heart = he;
    }
    protected void setStat()
    {
        float _heartUp = heartUp + plusHeartUpPerLev * gameManager.GetSaveData().unitHeartLev[unitStatIndex];
        heart = firstHeart + heartUpPerLev * gameManager.GetSaveData().unitHeartLev[unitStatIndex] + _heartUp * ((unitLev * unitLev) - 1);

        float _dpUp = dpUp + plusDpUpPerLev * gameManager.GetSaveData().unitDpLev[unitStatIndex];
        dp = firstDp + dpUpPerLev * gameManager.GetSaveData().unitDpLev[unitStatIndex] + _dpUp * ((unitLev * unitLev) - 1);

        float _apUp = apUp + plusApUpPerLev * gameManager.GetSaveData().unitApLev[unitStatIndex];
        ap = firstAp + apUpPerLev * gameManager.GetSaveData().unitApLev[unitStatIndex] + _apUp * ((unitLev * unitLev) - 1);
    }
    protected void FirstEDSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(gameManager.GetEnemyUnitSpawnPosition().position, currentPosition);

        //buildingScript를 shortest로 지정
        buildingIsShortest = true;
        shortestEnemyDistance = enemyObjectDistanceArray[0];
    }
    protected void FirstODSet()
    {
        objectDistanceArray[0] = Vector2.Distance(gameManager.GetUnitSpawnPosition().position, currentPosition);

        shortestDistance = objectDistanceArray[0];
    }
    protected void DestroyCheck()
    {
        if (heart <= 0f && !isDead)
        {
            anim.Play("Dead");
            speed = 0f;
            ap = 0f;
            isDead = true;
            MinusUnitNum();
            // Destroy(unitOnMiniMap);
        }
    }

    private void MinusUnitNum()
    {
        int unitNum = fusionManager.GetUnitNum() - 1;
        fusionManager.SetUnitNum(unitNum);
    }

    public void Destroye(UnitScript a)
    {
        if (a == null)
            a = this;

        stageManager.deathPlayerUnitNum++;
        fusionManager.SetCanSetScripts();
        Destroy(a.gameObject);
    }
    protected void AttackCheck()
    {
        if (!followingMouse)
        {
            if (shortestEnemyDistance < attackDistance)
            {
                Attack();
            }
        }
    }
    void Move()
    {
        if (attackDistance < stopByEnemyDistance)
        {
            stopByEnemyDistance = attackDistance;
        }

        if (objectDistanceArray[0] < stopByEnemyDistance) // 이 유닛이 소환되어 spawnPosition에서 1f이상 이동한 상태가 아니라면
        {
            stopByEnemyDistance = firstStopByEnemyDistance + 0.5f; // 이 유닛은 적 유닛이 이 유닛을 감지하여 이동을 멈추게 되는 거리보다 먼 거리에서 멈춘다.
                                                                   // 그렇게 되면 적은 이 유닛보다, 건물을 우선 공격하게 된다.
        }
        else
        {
            stopByEnemyDistance = firstStopByEnemyDistance;
        }

        if (shortestScript != null)
        {
            if (shortestScript.isStopByObject && isStopByObject)
            {
                if ((shortestForwardDistance > stopByObjectDistance) && (shortestEnemyDistance > stopByEnemyDistance) && canSetSpeed)
                    speed = firstSpeed;
                else
                {
                    speed = 0f;
                }
            }
            else
            {
                if ((shortestEnemyDistance > stopByEnemyDistance) && canSetSpeed)
                    speed = firstSpeed;
                else
                    speed = 0f;
            }
        }
        else
        {
            if ((shortestEnemyDistance > stopByEnemyDistance) && canSetSpeed)
                speed = firstSpeed;
            else
                speed = 0f;
        }

        if (!attackAnimIsPlaying && !isDead)
        {
            if (speed != 0f)
                anim.Play("WalkR");
        }


        CheckHe();

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    #region fusion, levelUp 관련 함수들
    public void FusionCheck()
    {
        if (Input.GetMouseButtonUp(0))
        {
            firstPositionSet = false;
            onlyOneFollowUnitNum = false;
            followingMouse = false;

        }

        if (Input.GetMouseButtonUp(0) && mouseCheck)
        {
            bool unitNumCheck = (fusionManager.GetFollowingUnitNum() == 0);

            onlyOneFollowUnitNum = false;

            if (!unitNumCheck)
            {
                fusionManager.SetFollowingUnitNum(0);
                followingCheck = false;
            }
            if (shortestScript != null)
            {
                if (shortestScript.unitId == unitId)
                {
                    if (shortestDistance < firstUnitClickableRange && gameManager.GetMoney() >= levelUpCost)
                    {
                        LevelUp(shortestScript.unitId, unitLev, shortestScript.unitLev);
                    }
                    else if (gameManager.GetMoney() < levelUpCost)
                    {
                        Instantiate(stageManager.notEnoughMoneyText, stageManager.textSpawnPosition);
                    }
                    else if (unitLev >= gameManager.GetSaveData().maxFusionLev)
                    {
                        Instantiate(stageManager.maxLevelText, stageManager.textSpawnPosition);
                    }
                }
            }

            ComeBack();

            mapSliderScript.gameObject.SetActive(true);
            mouseCheck = false;
        }
        else if (shortestScript != null)
        {
            if (followingMouse && shortestDistance < firstUnitClickableRange && unitId == shortestScript.GetUnitID() && unitLev == shortestScript.GetUnitLev())
            {
                if (unitLev >= gameManager.GetSaveData().maxFusionLev)
                {
                    costText.text = "이미 최대레벨입니다.";
                    return;
                }
                costText.text = $"{levelUpCost} 원";
            }
            else
            {
                costText.text = "";
            }
        }
        else
        {
            costText.text = "";
        }
    }
    protected void ComeBack()
    {
        unitClickableRange = firstUnitClickableRange;
        followingMouse = false;
        transform.localPosition = firstPosition;
        currentPosition = transform.localPosition;

        gameManager.SetCSt(true);
    }
    public void LevelUp(int id, int aLev, int bLev)
    {
        int money = 0;

        if (id == unitId && unitLev == shortestScript.GetUnitLev())
        {

            if (unitLev < gameManager.GetSaveData().maxFusionLev)
            {
                money = gameManager.GetMoney() - levelUpCost;

                gameManager.SetMoney(money);

                int a = shortestScript.GetUnitLev() + 1;

                shortestScript.SetUnitLev(a);

                shortestScript.setStat();
                shortestScript.SetMaxHealth();

                fusionManager.SetUnitNum(thisUnitNum = fusionManager.GetUnitNum() - 1);
                Destroye(this);
            }
            else
            {
                ComeBack();
            }

        }
    }
    #endregion
    #region distance 관련 함수들
    public void EDCheck()
    {
        FirstEDSet();
        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
            {
                enemyObjectDistanceArray[a + 1] = Vector2.Distance(fusionManager.enemyScript[a].GetCurrentPosition(), currentPosition);

                if (enemyObjectDistanceArray[a + 1] < shortestEnemyDistance &&  // shortest 갱신을 위한 조건문
                    fusionManager.enemyScript[a].GetCurrentPosition().x >= currentPosition.x - 0.5f) // 해당 enemyScript가 전방에 있는지 체크하기 위한 조건문                                                                                
                {
                    
                    shortestEnemyScript = fusionManager.enemyScript[a];
                    shortestEnemyDistance = enemyObjectDistanceArray[a + 1];
                    buildingIsShortest = false;
                    
                }
            }
        }
        else
        {
            canAttack = true;
            shortestEnemyDistance = 100f;
            shortestEnemyScript = null;
        }
    }
    private void ForwardODCheck()
    {
        float _ShortestForwardDistance = 100f;
        bool shortestForwardIsSet = false;

        for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
        {
            if (fusionManager.unitScript[a].GetThisUnitNO() < thisUnitNO) // 해당 unit오브젝트가 먼저 소환된 오브젝트인지 체크
            {
                if (objectDistanceArray[a + 1] < _ShortestForwardDistance)
                {
                    _ShortestForwardDistance = objectDistanceArray[a + 1];
                    shortestForwardDistance = _ShortestForwardDistance;
                    shortestForwardScript = fusionManager.unitScript[a];
                    shortestForwardIsSet = true;
                }
            }
        }

        if (!shortestForwardIsSet)
        {
            shortestForwardDistance = 10f;
        }
    }
    private void BackwardODCheck()
    {
        float _ShortestBackwardDistance = 100f;
        bool shortestBackwardIsSet = false;

        for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
        {
            if (fusionManager.unitScript[a].GetThisUnitNO() > thisUnitNO) // 해당 unit오브젝트가 나중에 소환된 오브젝트인지 체크
            {
                if (objectDistanceArray[a + 1] < _ShortestBackwardDistance)
                {
                    _ShortestBackwardDistance = objectDistanceArray[a + 1];
                    shortestBackwardDistance = _ShortestBackwardDistance;
                    shortestBackwardScript = fusionManager.unitScript[a];
                    shortestBackwardIsSet = true;
                }
            }

            if (!shortestBackwardIsSet)
            {
                shortestBackwardDistance = 10f;
            }
        }
    }
    public void ODCheck()//이 함수 복사, 수정 후 Enemy의 위치를 구하는 함수로 변환
    {
        float _ShortestDistance = 100f;

        FirstODSet();
        for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
        {
            if (fusionManager.unitScript[a] != null)
            {
                objectDistanceArray[a + 1] = Vector2.Distance(fusionManager.unitScript[a].currentPosition, currentPosition);

                if (objectDistanceArray[a + 1] < _ShortestDistance)
                {
                    bool isThisObject = (fusionManager.unitScript[a] == this);

                    if (!isThisObject)
                    {
                        if (fusionManager.unitScript[a] != this)
                        {
                            shortestScript = fusionManager.unitScript[a];
                            _ShortestDistance = objectDistanceArray[a + 1];
                            shortestDistance = _ShortestDistance;
                        }
                    }
                }
            }
        }
    }
    #endregion
    public void followCheck()
    {
        bool a = fusionManager.GetFollowingUnitNum() < 2;

        if (a)
            followingCheck = true;

    }
    public void moveByMouse()
    {
        if (isFollow && mouseCheck && !gameManager.GetMapSliderMoving() && !gameManager.popUpIsSpawned)
        {
            bool canFollow = (mouseDistance < unitClickableRange);

            if (canFollow)
            {
                if ((!onlyOneFollowUnitNum))
                {
                    onlyOneFollowUnitNum = true;

                    int followingUnitNum =
                        fusionManager.GetFollowingUnitNum() + 1;
                    fusionManager.SetFollowingUnitNum(followingUnitNum);

                    followCheck();
                }
                if (followingCheck)
                {
                    mapSliderScript.gameObject.SetActive(false);

                    unitClickableRange = 10f;
                    currentPosition = targetPosition;

                    transform.localPosition = currentPosition;

                    followingMouse = true;
                }
                else
                {
                    unitClickableRange = firstUnitClickableRange;
                    mouseCheck = false;
                }
            }
        }
    }
    public void onClickMouse()
    {
        if (Input.GetMouseButton(0) && !isDead)
        {
            fusionManager.SetIsFollow(true);

            if (!(firstPositionSet))
            {
                firstPosition = currentPosition;
                firstPositionSet = true;
            }

            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseDistance = Vector2.Distance(currentPosition, targetPosition);

            if (mouseDistance < unitClickableRange)
            {
                mouseCheck = true;
            }
            moveByMouse();
        }
    }

}
