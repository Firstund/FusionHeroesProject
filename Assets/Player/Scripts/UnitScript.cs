using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
    //공격대기시간, 소환 대기시간 설정할것.
    FusionManager fusionManager = null;
    GameManager gameManager = null;
    [SerializeField]
    private Slider slider = null;

    [SerializeField]
    private int unitId = 01; // 유닛의 종류
                             // 첫번째 자리: (이 유닛을 소환하기 위해 필요한 퓨전 수 + 1)
                             // 두번째 자리: 0 (특수 케이스가 떠오를 때를 대비)
                             // 세번째 자리: unitId의 첫번째 자리가 같은 숫자들 중 이 유닛이 생긴 순서
    [SerializeField]
    private int[] fusionUnitId = new int[5]; // 이 유닛과 퓨전할 수 있는 유닛들의 유닛 아이디
    [SerializeField]
    private GameObject[] nextUnit = new GameObject[5]; // 이 유닛이 퓨전한 후 나올수 있는 유닛들,
                                                       // 인덱스 값은 해당 유닛의 아이디의 (세번째 숫자 - 1)로 설정한다.
                                                       // 첫번 째 배열에 들어가는 유닛의 아이디는 무조건 이 스크립트의 unitId값이다.
    [SerializeField]
    private float attackDistance = 2f;
    [SerializeField]
    private float heart = 100f;
    [SerializeField]
    private float heartUp = 5f;
    [SerializeField]
    private float ap = 2f;
    [SerializeField]
    private float apUp = 0.5f;
    [SerializeField]
    private float dp = 2f;
    [SerializeField]
    private float dpUp = 0.25f;
    [SerializeField]
    private float attackDelay = 2f;
    [SerializeField]
    private float speed = 0f;
    private float firstSpeed = 0f;
    [SerializeField]
    private float totalAtk = 0f;

    [SerializeField]
    private GameObject Lev = null;
    private TextMesh levelText = null;

    private float shortestHeart = 0f;
    private float shortestDp = 0f;

    [SerializeField]
    private UnitScript shortestScript = null;
    [SerializeField]
    private EnemyScript shortestEnemyScript = null;

    [SerializeField]
    private AudioSource audi = null;
    private Animator anim = null;

    [SerializeField]
    private bool onlyOneFollowUnitNum = false;

    [SerializeField]
    private int thisUnitNum = 0; // 현재 소환된 오브젝트들 중 몇번째 오브젝트인가
    [SerializeField]
    private int thisUnitNO = 0; // 게임 전체에서 몇 번째 소환된 오브젝트인가
    [SerializeField]
    private int unitLev = 01; // 유닛의 레벨
    [SerializeField]
    private int levelUpCost = 10;

    [SerializeField]
    private float clickableX = 1f;

    [SerializeField]
    private float mouseDistance = 0f;

    [SerializeField]
    private float[] objectDistanceArray;
    [SerializeField]
    private float[] enemyObjectDistanceArray;

    [SerializeField]
    private float shortestDistance = 10f;
    [SerializeField]
    private float shortestForwardDistance = 10f;
    private UnitScript shortestForwardScipt = null;

    [SerializeField]
    private float shortestEnemyDistance = 10f;
    //
    [SerializeField]
    private Vector2 currentPosition = new Vector2(100f, 100f);

    private Vector2 targetPosition = Vector2.zero;

    private float testDistance = 0f;

    [SerializeField]
    private Vector2 firstPosition = Vector2.zero;
    //
    [SerializeField]
    private bool firstPositionSet = false;
    [SerializeField]
    private bool followingCheck = false;
    [SerializeField]
    private bool attackedCheck = false;
    [SerializeField]
    private bool buildingIsShortest = false;
    [SerializeField]
    private bool mouseCheck = false;
    [SerializeField]
    private bool followingMouse = false;
    [SerializeField]
    private bool isDead = false;
    public bool isFollow = false;

    void Awake()
    {
        gameObject.transform.SetParent(GameObject.Find("Units").gameObject.transform, true);
        firstSpeed = speed;

        fusionManager = FindObjectOfType<FusionManager>();

        int unitNO = fusionManager.GetUnitNO() + 1;
        thisUnitNO = unitNO;
        fusionManager.SetUnitNO(unitNO);

        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();
    }

    void Start()
    {
        levelText = Lev.GetComponent<TextMesh>();

        int unitNum = fusionManager.GetUnitNum() + 1;
        thisUnitNum = unitNum;
        fusionManager.SetUnitNum(unitNum);

        SetMaxHealth();
    }

    void Update()
    {
        levelText.text = string.Format("Level: {0}", unitLev); // 레벨 텍스트
        currentPosition = transform.localPosition;
        isFollow = fusionManager.GetIsFollow();

        objectDistanceArray = new float[fusionManager.GetUnitNum()];
        enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];

        onClickMouse();

        ODCheck();
        EDCheck();
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
    private IEnumerator Attack()
    {
        //단일공격
        if (!attackedCheck && !onlyOneFollowUnitNum)
        {
            attackedCheck = true;

            if (!isDead)
                anim.Play("AttackR");

            yield return new WaitForSeconds(attackDelay);
            //공격 애니메이션 출력
            if (shortestEnemyDistance < attackDistance)
            {
                audi.Play();
                if (buildingIsShortest)
                {
                    shortestHeart = fusionManager.enemyBuildingScript.getHe(shortestHeart);
                    shortestDp = fusionManager.enemyBuildingScript.getD(shortestDp);

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
                    shortestHeart = shortestEnemyScript.getHe(shortestHeart);
                    shortestDp = shortestEnemyScript.getD(shortestDp);

                    totalAtk = (ap - shortestDp);//데미지 공식 적용

                    if (totalAtk <= 0)
                    {
                        totalAtk = 1;
                    }

                    shortestHeart -= totalAtk; //단일공격

                    shortestEnemyScript.SetHP(shortestHeart);
                }  
            }
            attackedCheck = false;
        }
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
    private void CheckHe()
    {
        if (heart <= 0)
        {
            ap = 0f;
            speed = 0f;
        }
    }
    private IEnumerator IsAroundSet()
    {

        yield return new WaitForSeconds(0.1f);
        fusionManager.SetIsAround(false);

        int unitNum = fusionManager.GetUnitNum() - 1;

        fusionManager.SetUnitNum(unitNum);

        Destroy(gameObject);
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
    public int GetThisUnitNO()
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
        // 이렇게 되면 유닛의 스탯 증가가 나중에 기하급수적으로 늘게된다. 
        /*         
        heart += heartUp * unitLev;
        dp += dpUp * unitLev;
        ap += apUp * unitLev;
        */
        // 기획에 따라 이렇게 할 수도 있지만, 지금은 스탯의 증가를 등차수열로 하자.

        heart += heartUp * unitLev;
        dp += dpUp * unitLev;
        ap += apUp * unitLev;
    }
    private void FirstODSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(gameManager.GetEnemyUnitSpawnPosition().position, currentPosition);

        //buildingScript를 shortest로 지정
        buildingIsShortest = true;
        shortestEnemyDistance = enemyObjectDistanceArray[0];
    }
    private void DestroyCheck()
    {
        if (heart <= 0f)
        {
            anim.Play("Dead");
            speed = 0f;
            ap = 0f;

            StartCoroutine(Destroye(gameObject));
        }
    }
    private IEnumerator Destroye(GameObject obj)
    {
        if (!isDead)
        {
            isDead = true;

            int unitNum = fusionManager.GetUnitNum() - 1;
            fusionManager.SetUnitNum(unitNum);

            yield return new WaitForSeconds(0.7f);

            Destroy(obj);
        }
    }
    private void AttackCheck()
    {
        if (!followingMouse)
        {
            if (shortestEnemyDistance < attackDistance)
            {
                StartCoroutine(Attack());
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

        if (!attackedCheck && !isDead)
            anim.Play("WalkR");

        if (buildingIsShortest)
        {
            stopByEnemyDistance = 5;
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

            if (shortestDistance < clickableX && !fusionManager.GetIsAround() && gameManager.GetMoney() >= levelUpCost)
            {
                LevelUp(shortestScript.unitId, unitLev, shortestScript.unitLev);
            }
            else
            {
                ComeBack();
            }

            mouseCheck = false;
        }
    }
    private void ComeBack()
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
                        fusionManager.SetIsAround(true);
                        money = gameManager.GetMoney() - levelUpCost;

                        gameManager.SetMoney(money);

                        int a = shortestScript.GetUnitLev() + 1;
                        shortestScript.SetUnitLev(a);

                        shortestScript.setStat();

                        StartCoroutine(IsAroundSet());
                        break;
                    case 1: // 여기부터 퓨전
                        UnitScript nextUnitScript;

                        fusionManager.SetIsAround(true);
                        money = gameManager.GetMoney() - levelUpCost;

                        int unitNum = fusionManager.GetUnitNum() - 1;
                        fusionManager.SetUnitNum(unitNum);

                        Destroy(shortestScript.gameObject);

                        nextUnitScript = Instantiate(nextUnit[i], transform).GetComponent<UnitScript>();

                        if (shortestScript.GetThisUnitNO() > thisUnitNO)
                            nextUnitScript.SetThisUnitNO(thisUnitNO);
                        else
                        {
                            nextUnitScript.SetThisUnitNO(shortestScript.GetThisUnitNO());
                        }

                        nextUnitScript.SetUnitLev(unitLev);

                        gameManager.SetMoney(money);

                        StartCoroutine(IsAroundSet());
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }

            gameManager.SetCSt(true);

        }
    }
    #endregion
    #region distance 관련 함수들
    public void EDCheck()
    {
        FirstODSet();
        if (fusionManager.GetEnemyUnitNum() > 0)
        {
            for (int a = 0; a < fusionManager.GetEnemyUnitNum() - 1; a++)
            {
                enemyObjectDistanceArray[a] = Vector2.Distance(fusionManager.enemyScript[a].GetCurrentPosition(), currentPosition);

                if (enemyObjectDistanceArray[a] < shortestEnemyDistance &&  // shortest 갱신을 위한 조건문
                    fusionManager.enemyScript[a].GetCurrentPosition().x >= currentPosition.x - 0.5f) // 해당 enemyScript가 전방에 있는지 체크하기 위한 조건문                                                                                
                {
                    bool arrayDistanceCheck = (enemyObjectDistanceArray[a] == 0);

                    if (!arrayDistanceCheck)
                    {
                        shortestEnemyScript = fusionManager.enemyScript[a];
                        shortestEnemyDistance = enemyObjectDistanceArray[a];
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
        float LShortestDistance = 100f;
        float LShortestForwardDistance = 100f;

        FirstODSet();
        for (int a = 0; a < fusionManager.GetUnitNum() - 1; a++)
        {
            if (fusionManager.unitScript[a] != null)
            {
                objectDistanceArray[a] = Vector2.Distance(fusionManager.unitScript[a].currentPosition, currentPosition);

                if (objectDistanceArray[a] < LShortestDistance)
                {
                    bool arrayDistanceCheck = (objectDistanceArray[a] == 0);

                    if (!arrayDistanceCheck)
                    {
                        if (fusionManager.unitScript[a] != this)
                        {
                            shortestScript = fusionManager.unitScript[a];
                            LShortestDistance = objectDistanceArray[a];
                            shortestDistance = LShortestDistance;
                        }
                    }
                }
                if (fusionManager.unitScript[a].GetThisUnitNO() < thisUnitNO) // 해당 unit오브젝트가 먼저 소환된 오브젝트인지 체크
                {
                    if (objectDistanceArray[a] < LShortestForwardDistance)
                    {
                        LShortestForwardDistance = objectDistanceArray[a];
                        shortestForwardDistance = LShortestForwardDistance;
                        shortestForwardScipt = fusionManager.unitScript[a];
                        shortestForwardIsSet = true;
                    }
                }

                if (!shortestForwardIsSet)
                {
                    shortestForwardDistance = 10f;
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
            fusionManager.SetIsAround(false);

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
