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

    [SerializeField]
    protected int unitId = 01; // 유닛의 종류
                               // 첫번째 자리: (이 유닛을 소환하기 위해 필요한 퓨전 수 + 1)
                               // 두번째 자리: 0 (특수 케이스가 떠오를 때를 대비)
                               // 세번째 자리: unitId의 첫번째 자리가 같은 숫자들 중 이 유닛이 생긴 순서
    public int unitStatIndex
    {
        get{return (unitId / 50 + unitId % 100);}
    }
    [SerializeField]
    protected int[] fusionUnitId = new int[5]; // 이 유닛과 퓨전할 수 있는 유닛들의 유닛 아이디
    [SerializeField]
    protected GameObject[] nextUnit = new GameObject[5]; // 이 유닛이 퓨전한 후 나올수 있는 유닛들,
                                                         // 인덱스 값은 해당 유닛의 아이디의 (세번째 숫자 - 1)로 설정한다.
                                                         // 첫번 째 배열에 들어가는 유닛의 아이디는 무조건 이 스크립트의 unitId값이다.
    [SerializeField]
    protected float attackDistance = 2f;
    [SerializeField]
    protected float _heart = 100f;
    public float heart{
        get{return _heart;}
        set{_heart = value;}
    }
    protected float firstHeart = 0f;
    [SerializeField]
    protected float heartUp = 5f;
    [SerializeField]
    protected float _heartUpPerLev = 0.1f;
    public float heartUpPerLev
    {
        get{return _heartUpPerLev;}
        set{_heartUpPerLev = value;}
    }
    [SerializeField]
    protected float _ap = 2f;
    public float ap
    {
        get{return _ap;}
        set{_ap = value;}
    }
    protected float firstAp = 0f;
    [SerializeField]
    protected float apUp = 0.5f;
    [SerializeField]
    protected float _apUpPerLev = 0.02f;
    public float apUpPerLev
    {
        get{return _apUpPerLev;}
        set{_apUpPerLev = value;}
    }
    [SerializeField]
    protected float _dp = 2f;
    public float dp
    {
        get{return _dp;}
        set{_dp = value;}
    }
    protected float firstDp = 0f;
    [SerializeField]
    protected float dpUp = 0.25f;
    [SerializeField]
    protected float _dpUpPerLev = 0.01f;
    public float dpUpPerLev
    {
        get{return _dpUpPerLev;}
        set{_dpUpPerLev = value;}
    }

    [SerializeField]
    protected float attackDelay = 2f;
    [SerializeField]
    protected float speed = 0f;
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
    protected EnemyScript shortestEnemyScript = null;

    [SerializeField]
    protected AudioSource audi = null;
    protected Animator anim = null;

    [SerializeField]
    protected bool onlyOneFollowUnitNum = false;

    [SerializeField]
    protected int thisUnitNum = 0; // 현재 소환된 오브젝트들 중 몇번째 오브젝트인가
    [SerializeField]
    protected double thisUnitNO = 0; // 게임 전체에서 몇 번째 소환된 오브젝트인가
                                     // 이게 왜 곂치지???
    [SerializeField]
    protected int unitLev = 01; // 유닛의 레벨
    [SerializeField]
    protected int levelUpCost = 10;

    [SerializeField]
    protected float clickableX = 1f;

    protected float mouseDistance = 0f;
    
    [SerializeField]
    protected float[] objectDistanceArray;
    [SerializeField]
    protected float[] enemyObjectDistanceArray;

    [SerializeField]
    protected float shortestDistance = 10f;
    [SerializeField]
    protected float shortestForwardDistance = 10f;
    [SerializeField]
    protected float shortestBackwardDistance = 10f; // 넉백됬을 때 unitNO를 새로 설정해 줄 때 필요.
    protected UnitScript shortestForwardScript = null;
    protected UnitScript shortestBackwardScript = null;

    protected float shortestEnemyDistance = 10f;
    //
    protected Vector2 currentPosition = new Vector2(100f, 100f);

    protected Vector2 targetPosition = Vector2.zero;

    protected float testDistance = 0f;

    protected Vector2 firstPosition = Vector2.zero;
    //
    protected bool firstPositionSet = false;
    protected bool followingCheck = false;
    protected bool attackedCheck = false;
    protected bool buildingIsShortest = false;
    protected bool mouseCheck = false;
    protected bool followingMouse = false;
    protected bool isDead = false;
    private bool isAttackOne = true;
    public bool isFollow = false;

    void Awake()
    {
        gameObject.transform.SetParent(GameObject.Find("Units").gameObject.transform, true);
        firstSpeed = speed;

        fusionManager = FindObjectOfType<FusionManager>();

        fusionManager.SetCanSetScripts();

        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

        firstAp = ap;
        firstDp = dp;
        firstHeart = heart;
    }

    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        fusionManager.SetUnitNum(thisUnitNum = fusionManager.GetUnitNum() + 1);

        fusionManager.SetUnitNO(thisUnitNO = fusionManager.GetUnitNO() + 1d);

        levelText = Lev.GetComponent<TextMesh>();

        SetDistanceArrayIndex();

        // unitStatIndex = (unitId / 50 + unitId % 100); // stat에 쓰일 Index설정

        setStat();

       

        SetMaxHealth();
    }

    void Update()
    {
        audi.volume = gameManager.GetSoundValue();
        levelText.text = string.Format("Level: {0}", unitLev); // 레벨 텍스트
        currentPosition = transform.localPosition;
        isFollow = fusionManager.GetIsFollow();

        SetDistanceArrayIndex();

        onClickMouse();

        ODCheck();
        EDCheck();
        FusionCheck();
        Move();
        HealthBar();
        DestroyCheck();

        // if(transform.position.y != fusionManager.playerUnitSpawnPosition.position.y)
        // {
        //     Vector2 targetPosition = Vector2.zero;
        //     targetPosition.y = fusionManager.playerUnitSpawnPosition.position.y;
        //     transform.position = targetPosition;
        // }

        if (gameManager.GetCST())
            AttackCheck();

        if (followingCheck)
        {
            gameManager.SetCSt(false);
        }
    }
    public void SetDistanceArrayIndex()
    {
        if(objectDistanceArray.Length != fusionManager.GetUnitNum())
            objectDistanceArray = new float[fusionManager.GetUnitNum()];
            
        if(enemyObjectDistanceArray.Length != fusionManager.GetEnemyUnitNum())
            enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];
    }
    protected void Attack()
    {
        //단일공격
        if (!attackedCheck && !onlyOneFollowUnitNum)
        {
            attackedCheck = true;

            if (!isDead)
            {
                if (speed <= 0f)
                    anim.Play("AttackR");
                else
                    anim.Play("WalkAttackR");
            }
            //공격 애니메이션 출력

        }
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
                audi.Play();
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
    public void ResetAttackedCheck()
    {
        attackedCheck = false;
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
    public float GetDistance()
    {
        return testDistance;
    }
    public void SetDistance(float a)
    {
        testDistance = a;
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
        heart = firstHeart + heartUpPerLev * gameManager.GetSaveData().unitHeartLev[unitStatIndex] + heartUp * unitLev;
        
        dp =  firstDp + dpUpPerLev * gameManager.GetSaveData().unitDpLev[unitStatIndex] + dpUp * unitLev;

        ap = firstAp + apUpPerLev * gameManager.GetSaveData().unitApLev[unitStatIndex] + apUp * unitLev;
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
        float stopByEnemyDistance = 1f;

        if (attackDistance < stopByEnemyDistance)
        {
            stopByEnemyDistance = attackDistance;
        }

        if (shortestScript != null)
        {
            if (shortestScript.getHe() <= 0f)
            {
                speed = firstSpeed;
            }
            else
            {
                speed = 0f;
            }
        }

        if ((shortestForwardDistance > 1) && (shortestEnemyDistance > stopByEnemyDistance))
            speed = firstSpeed;
        else
        {
            speed = 0f;
        }

        if (!attackedCheck && !isDead)
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
            gameManager.SetCSt(true);

        }

        if (fusionManager.GetIsUped() && mouseCheck)
        {
            bool unitNumCheck = (fusionManager.GetFollowingUnitNum() == 0);

            onlyOneFollowUnitNum = false;

            if (!unitNumCheck)
            {
                fusionManager.SetFollowingUnitNum(0);
                followingCheck = false;
            }

            if (shortestDistance < clickableX && gameManager.GetMoney() >= levelUpCost)
            {
                LevelUp(shortestScript.unitId, unitLev, shortestScript.unitLev);
            }
            else
            {
                Instantiate(stageManager.notEnoughMoneyText, stageManager.textSpawnPosition);
                ComeBack();
            }

            mouseCheck = false;
        }
        else if (followingMouse && shortestDistance < clickableX && unitLev == shortestScript.GetUnitLev())
        // 다른 fusion들과 호환이 가능하도록 변경, 각 fusion마다 levelUpCost 값이 다르다.
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
    protected void ComeBack()
    {
        followingMouse = false;
        transform.localPosition = firstPosition;
    }
    public void LevelUp(int id, int aLev, int bLev)
    {
        int money = 0;
        for (int i = 0; i < 5; i++)
        {
            if (id == fusionUnitId[i] && unitLev == shortestScript.GetUnitLev())
            {
                switch (i)
                {
                    case 0: // 그냥 유닛의 레벨만 오름
                            // levelUpCost 지정
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
                        break;
                    case 1: // 여기부터 퓨전 // 함수로 빼두자 // 일단 비활성화, 추후에 추가
                        // UnitScript nextUnitScript;

                        // money = gameManager.GetMoney() - levelUpCost;

                        // int unitNum = fusionManager.GetUnitNum() - 1;
                        // fusionManager.SetUnitNum(unitNum);

                        // Destroye(shortestScript);

                        // nextUnitScript = Instantiate(nextUnit[i], transform).GetComponent<UnitScript>();

                        // nextUnitScript.SetUnitLev(unitLev);

                        // gameManager.SetMoney(money);

                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        ComeBack();
                        break;
                }
            }
            else
            {
                ComeBack();
            }

            gameManager.SetCSt(true);

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
                    bool arrayDistanceCheck = (enemyObjectDistanceArray[a + 1] == 0);

                    if (!arrayDistanceCheck)
                    {
                        shortestEnemyScript = fusionManager.enemyScript[a];
                        shortestEnemyDistance = enemyObjectDistanceArray[a + 1];
                        buildingIsShortest = false;
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
    public void ODCheck()//이 함수 복사, 수정 후 Enemy의 위치를 구하는 함수로 변환
    {
        bool shortestForwardIsSet = false;
        bool shortestBackwardIsSet = false;
        float _ShortestDistance = 100f;
        float _ShortestForwardDistance = 100f;
        float _ShortestBackwardDistance = 100f;

        FirstODSet();
        for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
        {
            if (fusionManager.unitScript[a] != null)
            {
                objectDistanceArray[a + 1] = Vector2.Distance(fusionManager.unitScript[a].currentPosition, currentPosition);

                if (objectDistanceArray[a + 1] < _ShortestDistance)
                {
                    bool arrayDistanceCheck = (objectDistanceArray[a + 1] == 0);

                    if (!arrayDistanceCheck)
                    {
                        if (fusionManager.unitScript[a] != this)
                        {
                            shortestScript = fusionManager.unitScript[a];
                            _ShortestDistance = objectDistanceArray[a + 1];
                            shortestDistance = _ShortestDistance;
                        }
                    }
                }
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

                if (!shortestForwardIsSet)
                {
                    shortestForwardDistance = 10f;
                }
                if (!shortestBackwardIsSet)
                {
                    shortestBackwardDistance = 10f;
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
        if (isFollow && mouseCheck && !gameManager.GetMapSliderMoving())
        {
            bool canFollow = (mouseDistance < clickableX);

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
                    currentPosition = targetPosition;

                    transform.localPosition = currentPosition;

                    followingMouse = true;
                }
                else
                {
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

            if (mouseDistance < clickableX)
            {
                mouseCheck = true;
            }
            moveByMouse();
        }
    }

}
