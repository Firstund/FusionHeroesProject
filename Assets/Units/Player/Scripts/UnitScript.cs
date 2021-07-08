using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitScript : MonoBehaviour
{
    //공격대기시간, 소환 대기시간 설정할것.
    private FusionManager fusionManager = null;
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    private UnitPooling poolManager = null;

    [SerializeField]
    private Slider slider = null;
    private MapSliderScript mapSliderScript = null;

    [SerializeField]
    private int unitId = 01; // 유닛의 종류
                             // 첫번째 자리: 이 유닛이 (n * 10 + 세번째 자리) 번째 수라면, n + 1로 지정
                             // 두번째 자리: 0 (특수 케이스가 떠오를 때를 대비) // 특수케이스->다른 유닛에 의해 소환된경우 그 유닛의 ID에, 두번째자리만 변경. 
                             // 두번째 자리는 이 유닛이 소환자 유닛의 몇번째 소환수인지를 의미.
                             // 세번째 자리: unitId의 첫번째 자리가 같은 숫자의 그룹 중 이 유닛이 생긴 순서
    public int unitStatIndex
    {
        get { return (unitId / 50 + ((unitId % 100) % 10)); }
    }
    // [SerializeField]
    // private int[] fusionUnitId = new int[5]; // 이 유닛과 퓨전할 수 있는 유닛들의 유닛 아이디 
    //// 퓨전은 잠시 보류 (사유: 게임은 퓨전이 없어도 충분히 복잡하다.)
    // [SerializeField]
    // private GameObject[] nextUnit = new GameObject[5]; // 이 유닛이 퓨전한 후 나올수 있는 유닛들,
    //                                                      // 인덱스 값은 해당 유닛의 아이디의 (세번째 숫자 - 1)로 설정한다.
    //                                                      // 첫번 째 배열에 들어가는 유닛의 아이디는 무조건 이 스크립트의 unitId값이다.
    [SerializeField]
    private UnitOnMiniMapScript unitOnMiniMap = null;
    private GameObject _unitOnMiniMap = null;

    [SerializeField]
    private GameObject projection = null;
    [SerializeField]
    private Transform projectionSpawnPosition = null;
    [SerializeField]
    private float _attackDistance = 2f;
    public float attackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
    }
    [SerializeField]
    private float damageDistance = 10; //투사체 광역공격의 데미지 계산에 이용되는 변수
    [SerializeField]
    private float _heart = 100f;
    public float heart
    {
        get { return _heart; }
        set { _heart = value; }
    }
    private float firstHeart = 0f;
    [SerializeField]
    private float heartUp = 5f;
    [SerializeField]
    private float plusHeartUpPerLev = 0.2f;
    [SerializeField]
    private float _heartUpPerLev = 0.1f;
    public float heartUpPerLev
    {
        get { return _heartUpPerLev; }
        set { _heartUpPerLev = value; }
    }
    [SerializeField]
    private float _ap = 2f;
    public float ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    private float firstAp = 0f;
    [SerializeField]
    private float apUp = 0.5f;
    [SerializeField]
    private float plusApUpPerLev = 1f;
    [SerializeField]
    private float _apUpPerLev = 0.02f;
    public float apUpPerLev
    {
        get { return _apUpPerLev; }
        set { _apUpPerLev = value; }
    }
    [SerializeField]
    private float _dp = 2f;
    public float dp
    {
        get { return _dp; }
        set { _dp = value; }
    }
    private float _firstDp = 0f;
    public float firstDp
    {
        get { return _firstDp; }
        set { _firstDp = value; }
    }
    [SerializeField]
    private float dpUp = 0.25f;
    [SerializeField]
    private float plusDpUpPerLev = 0.1f;
    [SerializeField]
    private float _dpUpPerLev = 0.01f;
    public float dpUpPerLev
    {
        get { return _dpUpPerLev; }
        set { _dpUpPerLev = value; }
    }

    [SerializeField]
    private float _speed = 0f;
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    [SerializeField]
    private float attackDelay = 1f;
    private float _firstSpeed = 0f;
    public float firstSpeed
    {
        get { return _firstSpeed; }
    }
    [SerializeField]
    private float totalAtk = 0f;

    [SerializeField]
    private GameObject Lev = null;
    private TextMesh levelText = null;
    [SerializeField]
    private Text costText = null;

    private float shortestHeart = 0f;
    private float shortestDp = 0f;

    [SerializeField]
    private UnitScript shortestScript = null;
    [SerializeField]
    private EnemyScript _shortestEnemyScript = null;
    public EnemyScript shortestEnemyScript
    {
        get { return _shortestEnemyScript; }
        set { _shortestEnemyScript = value; }
    }

    [SerializeField]
    private AudioSource _audi = null;
    public AudioSource audi
    {
        get { return _audi; }
        set { _audi = value; }
    }
    [SerializeField]
    private AudioClip attackSound = null;
    private Animator _anim = null;
    public Animator anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    [SerializeField]
    private bool onlyOneFollowUnitNum = false;
    [SerializeField]
    private bool _isStopByObject = true;
    public bool isStopByObject
    {
        get { return _isStopByObject; }
        set { _isStopByObject = value; }
    }

    [SerializeField]
    private int thisUnitNum = 0; // 현재 소환된 오브젝트들 중 몇번째 오브젝트인가
    [SerializeField]
    private double thisUnitNO = 0; // 게임 전체에서 몇 번째 소환된 오브젝트인가
    [SerializeField]
    private int unitLev = 01; // 유닛의 레벨
    [SerializeField]
    private int levelUpCost = 10;
    [SerializeField]
    private float unitClickableRange = 0f;
    [SerializeField]

    private float firstUnitClickableRange = 0f;

    [SerializeField]
    private float mouseDistance = 0f;

    [SerializeField]
    private float[] objectDistanceArray;
    [SerializeField]
    private float[] enemyObjectDistanceArray;

    [SerializeField]
    private float _shortestDistance = 10f;
    public float shortestDistance
    {
        get { return _shortestDistance; }
        set { _shortestDistance = value; }
    }
    [SerializeField]
    private float shortestForwardDistance = 10f;
    [SerializeField]
    private float shortestBackwardDistance = 10f; // 넉백됬을 때 unitNO를 새로 설정해 줄 때 필요.
    [SerializeField]
    private float _stopByEnemyDistance = 1f;
    public float stopByEnemyDistance
    {
        get { return _stopByEnemyDistance; }
        set { _stopByEnemyDistance = value; }
    }
    [SerializeField]
    private float stopByObjectDistance = 1.5f;
    private float firstStopByEnemyDistance = 0f;
    [SerializeField]
    private UnitScript shortestForwardScript = null;
    private UnitScript shortestBackwardScript = null;
    [SerializeField]
    private float _shortestEnemyDistance = 10f;
    public float shortestEnemyDistance
    {
        get { return _shortestEnemyDistance; }
        set { _shortestEnemyDistance = value; }
    }
    //
    private Vector2 currentPosition = new Vector2(100f, 100f);

    private Vector2 targetPosition = Vector2.zero;


    private Vector2 firstPosition = Vector2.zero;
    //
    private bool firstPositionSet = false;
    private bool followingCheck = false;
    private bool _canAttack = true;
    public bool canAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    [SerializeField]
    private bool _attackAnimIsPlaying = false;
    public bool attackAnimIsPlaying
    {
        get { return _attackAnimIsPlaying; }
        set { _attackAnimIsPlaying = value; }
    }
    private bool _buildingIsShortest = false;
    public bool buildingIsShortest
    {
        get { return _buildingIsShortest; }
        set { _buildingIsShortest = value; }
    }
    private bool mouseCheck = false;
    private bool followingMouse = false;
    [SerializeField]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
    }
    [SerializeField]
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
        _firstSpeed = speed;
        gameManager = GameManager.Instance;

        firstUnitClickableRange = unitClickableRange;

        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

        firstAp = ap;
        firstDp = dp;
        firstHeart = heart;
        firstStopByEnemyDistance = stopByEnemyDistance;
    }

    void Start()
    {
        fusionManager = FindObjectOfType<FusionManager>();

        stageManager = FindObjectOfType<StageManager>();
        poolManager = FindObjectOfType<UnitPooling>();

        levelText = Lev.GetComponent<TextMesh>();

        mapSliderScript = FindObjectOfType<MapSliderScript>();

        _unitOnMiniMap = Instantiate(unitOnMiniMap.gameObject, GameObject.Find("MapSlider").transform);

        UnitOnMiniMapScript unitOnMiniMapScript = _unitOnMiniMap.GetComponent<UnitOnMiniMapScript>();

        unitOnMiniMapScript.targetObject = gameObject;
        unitOnMiniMapScript.targetUnitTrm = gameObject.transform;

        SpawnSet();

    }

    public void SpawnSet()
    {
        _isDead = false;
        attackAnimIsPlaying = false;
        canAttack = true;

        gameObject.transform.position = gameManager.GetUnitSpawnPosition().position;

        fusionManager.SetCanSetScripts();

        _unitOnMiniMap.SetActive(true);

        PlusUnitNum();

        setStat();
        SetDistanceArrayIndex();

        SetMaxHealth();
        FusionCheck();
    }

    private void PlusUnitNum()
    {
        fusionManager.SetUnitNum(thisUnitNum = fusionManager.GetUnitNum() + 1);

        fusionManager.SetUnitNO(thisUnitNO = fusionManager.GetUnitNO() + 1d);
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

        }
    }
    public void SetDistanceArrayIndex()
    {
        if (objectDistanceArray.Length != fusionManager.GetUnitNum())
            objectDistanceArray = new float[fusionManager.GetUnitNum()];

        if (enemyObjectDistanceArray.Length != fusionManager.GetEnemyUnitNum())
            enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];
    }
    private void Attack()
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
            float maximumD = damageDistance;


            if (attackOne) // 단일공격
            {
                if (buildingIsShortest)
                {
                    BuildingAttack();
                }
                else if (shortestEnemyScript != null)
                {
                    ShortestEnemyAttack();
                }
            }
            else // 광역공격
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

                int a = 0;
                foreach (var item in fusionManager.enemyScript)
                {
                    try
                    {
                        if (enemyObjectDistanceArray[a] < maximumD && enemyObjectDistanceArray[a] >= minimumD)//minimum, maxism attackDistance를 이용하여 공격 범위 설정가능
                        {
                            float dp = item.getD();
                            float heart = item.getHe();

                            totalAtk = (ap - dp);

                            if (totalAtk <= 0f)
                            {
                                totalAtk = 0.2f;
                            }


                            heart -= totalAtk;

                            item.SetHP(heart);
                        }

                        a++;
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        break;
                    }
                }
            }

        }
    }

    public void GetDamage(Transform projection) // 투사체를 날리는 공격을 실행하는 함수
    {

        float minimumD = 0f;
        float maximumD = damageDistance;
        float enemyBuildingDistance = Vector2.Distance(projection.position, gameManager.GetEnemyUnitSpawnPosition().position);

        if (isAttackOne)
        {
            if (buildingIsShortest)
            {
                BuildingAttack();
            }
            else if (shortestEnemyScript != null)
            {
                ShortestEnemyAttack();
            }
        }
        else
        {
            if (enemyBuildingDistance < maximumD && enemyBuildingDistance >= minimumD)
            {
                BuildingAttack();
            }

            foreach (var item in fusionManager.enemyScript)
            {
                float distance = Vector2.Distance(projection.position, item.transform.position);

                if (distance < maximumD && distance >= minimumD)//minimum, maxism damageDistance를 이용하여 공격 범위 설정가능
                {
                    float dp = item.getD();
                    float heart = item.getHe();

                    totalAtk = (ap - dp);

                    if (totalAtk <= 0f)
                    {
                        totalAtk = 0.2f;
                    }


                    heart -= totalAtk;

                    item.SetHP(heart);
                }
            }
        }

    }

    private void BuildingAttack()
    {
        shortestHeart = fusionManager.enemyBuildingScript.getHe();
        shortestDp = fusionManager.enemyBuildingScript.getD();

        totalAtk = (ap - shortestDp);
        if (totalAtk <= 0f)
        {
            totalAtk = 1f;
        }

        shortestHeart -= totalAtk;

        fusionManager.enemyBuildingScript.SetHP(shortestHeart);
    }
    private void ShortestEnemyAttack()
    {
        shortestHeart = shortestEnemyScript.getHe();
        shortestDp = shortestEnemyScript.getD();

        totalAtk = (ap - shortestDp);//데미지 공식 적용

        if (totalAtk <= 0)
        {
            totalAtk = 1;
        }

        shortestHeart -= totalAtk;

        shortestEnemyScript.SetHP(shortestHeart);
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
    private void setStat()
    {
        float _heartUp = heartUp + plusHeartUpPerLev * gameManager.GetSaveData().unitHeartLev[unitStatIndex];
        heart = firstHeart + heartUpPerLev * gameManager.GetSaveData().unitHeartLev[unitStatIndex] + _heartUp * ((unitLev * unitLev) - 1);

        float _dpUp = dpUp + plusDpUpPerLev * gameManager.GetSaveData().unitDpLev[unitStatIndex];
        dp = firstDp + dpUpPerLev * gameManager.GetSaveData().unitDpLev[unitStatIndex] + _dpUp * ((unitLev * unitLev) - 1);

        float _apUp = apUp + plusApUpPerLev * gameManager.GetSaveData().unitApLev[unitStatIndex];
        ap = firstAp + apUpPerLev * gameManager.GetSaveData().unitApLev[unitStatIndex] + _apUp * ((unitLev * unitLev) - 1);
    }
    private void FirstEDSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(gameManager.GetEnemyUnitSpawnPosition().position, currentPosition);

        //buildingScript를 shortest로 지정
        buildingIsShortest = true;
        shortestEnemyDistance = enemyObjectDistanceArray[0];
    }
    private void FirstODSet()
    {
        objectDistanceArray[0] = Vector2.Distance(gameManager.GetUnitSpawnPosition().position, currentPosition);

        shortestDistance = objectDistanceArray[0];
    }
    private void DestroyCheck()
    {
        if (heart <= 0f && !isDead)
        {
            anim.Play("Dead");
            speed = 0f;
            ap = 0f;
            _isDead = true;
            MinusUnitNum();

        }
    }

    private void MinusUnitNum()
    {
        int unitNum = fusionManager.GetUnitNum() - 1;
        fusionManager.SetUnitNum(unitNum);
    }

    public void Destroye()
    {
        stageManager.deathPlayerUnitNum++;

        fusionManager.SetCanSetScripts();

        unitLev = 1;

        poolManager.units.Add(this);

        gameObject.transform.position = gameManager.GetUnitSpawnPosition().position;

        gameObject.SetActive(false);
    }
    private void AttackCheck()
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
                else if (canSetSpeed)
                {
                    speed = 0f;
                }
            }
            else
            {
                if ((shortestEnemyDistance > stopByEnemyDistance) && canSetSpeed)
                    speed = firstSpeed;
                else if (canSetSpeed)
                    speed = 0f;
            }
        }
        else
        {
            if ((shortestEnemyDistance > stopByEnemyDistance) && canSetSpeed)
                speed = firstSpeed;
            else if (canSetSpeed)
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
                        if (!shortestScript.isDead)
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
    private void ComeBack()
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
                Destroye();
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
        SetEnemyObjectDistanceArray();
    }

    private void SetEnemyObjectDistanceArray()
    {
        EnemyScript _shortestEnemyScript = null;

        FirstEDSet();

        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            int a = 0;
            foreach (var item in fusionManager.enemyScript)
            {
                try
                {
                    a++;

                    enemyObjectDistanceArray[a] = Vector2.Distance(item.GetCurrentPosition(), currentPosition);

                    if (enemyObjectDistanceArray[a] < shortestEnemyDistance &&  // shortest 갱신을 위한 조건문
                        item.GetCurrentPosition().x >= currentPosition.x - 0.5f) // 해당 enemyScript가 전방에 있는지 체크하기 위한 조건문                                                                                
                    {
                        _shortestEnemyScript = item;
                        shortestEnemyDistance = enemyObjectDistanceArray[a];
                        buildingIsShortest = false;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    break;
                }
            }
            shortestEnemyScript = _shortestEnemyScript;
        }
    }

    private void ForwardODCheck()
    {

        UnitScript _ShortestForwardScript = null;

        float _ShortestForwardDistance = 100f;
        bool shortestForwardIsSet = false;

        int a = 0;
        foreach (var item in fusionManager.unitScript)
        {
            a++;

            if (item.GetThisUnitNO() < thisUnitNO) // 해당 unit오브젝트가 먼저 소환된 오브젝트인지 체크
            {
                try
                {
                    if (objectDistanceArray[a] < _ShortestForwardDistance)
                    {
                        _ShortestForwardDistance = objectDistanceArray[a];
                        shortestForwardDistance = _ShortestForwardDistance;
                        _ShortestForwardScript = item;
                        shortestForwardIsSet = true;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    break;
                }
            }


        }

        shortestForwardScript = _ShortestForwardScript;

        if (!shortestForwardIsSet)
        {
            shortestForwardDistance = 10f;
        }
    }
    private void BackwardODCheck()
    {

        UnitScript _shortestBackWardScript = null;
        float _ShortestBackwardDistance = 100f;
        bool shortestBackwardIsSet = false;

        int a = 0;
        foreach (var item in fusionManager.unitScript)
        {
            a++;

            if (item.GetThisUnitNO() > thisUnitNO) // 해당 unit오브젝트가 나중에 소환된 오브젝트인지 체크
            {
                try
                {

                    if (objectDistanceArray[a] < _ShortestBackwardDistance)
                    {
                        _ShortestBackwardDistance = objectDistanceArray[a];
                        shortestBackwardDistance = _ShortestBackwardDistance;
                        _shortestBackWardScript = item;
                        shortestBackwardIsSet = true;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    break;
                }
            }

            if (!shortestBackwardIsSet)
            {
                shortestBackwardDistance = 10f;
            }
        }
        shortestBackwardScript = _shortestBackWardScript;

    }
    public void ODCheck()//이 함수 복사, 수정 후 Enemy의 위치를 구하는 함수로 변환
    {
        SetObjectDistanceArray();
    }

    private void SetObjectDistanceArray()
    {
        UnitScript _shortestScript = null;
        float _ShortestDistance = 100f;

        FirstODSet();
        int a = 0;
        foreach (var item in fusionManager.unitScript)
        {
            a++;

            if (item != null)
            {
                try
                {
                    objectDistanceArray[a] = Vector2.Distance(item.currentPosition, currentPosition);

                    if (objectDistanceArray[a] < _ShortestDistance)
                    {
                        bool isThisObject = (item == this);

                        if (!isThisObject)
                        {
                            if (item != this)
                            {
                                _shortestScript = item;
                                _ShortestDistance = objectDistanceArray[a];
                                shortestDistance = _ShortestDistance;
                            }
                        }
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    break;
                }

            }


        }

        shortestScript = _shortestScript;
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
                    SetObjectDistanceArray();

                    unitClickableRange = 10f;
                    currentPosition = targetPosition;

                    transform.localPosition = currentPosition;

                    followingMouse = true;
                    gameManager.SetCSt(false);
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
