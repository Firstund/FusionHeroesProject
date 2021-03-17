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
    private float attackDistance = 2f;
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
    private GameObject nextLevel = null;

    [SerializeField]
    private bool onlyOneFollowUnitNum = false;

    [SerializeField]
    private int unitId = 01; // 유닛의 종류
    [SerializeField]
    private int thisUnitNum = 0; // 이 유닛이 몇번째 소환됬는가
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
    private Vector2 currentPosition = Vector2.zero;

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
        firstSpeed = speed;
    }

    void Start()
    {
        levelText = Lev.GetComponent<TextMesh>();
        //building인 것을 구분, building일 시 unit일 땐 필요없는 것은 switch를 이용해 비활성화
        gameObject.transform.SetParent(null, true);
        fusionManager = FindObjectOfType<FusionManager>();
        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

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

        if(gameManager.GetCST())
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
            
            if(!isDead)
                anim.Play("AttackR");

            yield return new WaitForSeconds(attackDelay);
            //공격 애니메이션 출력
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

                fusionManager.enemyBuildingScript.setStat(shortestHeart, shortestDp);
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

                shortestEnemyScript.setStat(shortestHeart, shortestDp);
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
        if(heart <= 0)
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
    private void FirstODSet()
    {
        enemyObjectDistanceArray[0] = Vector2.Distance(fusionManager.enemyBuildingScript.currentPosition, currentPosition);

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

            isDead = true;

            StartCoroutine(Destroye(gameObject));
        }
    }
    private IEnumerator Destroye(GameObject obj)
    {
        yield return new WaitForSeconds(0.7f);
        

        int unitNum = fusionManager.GetUnitNum() - 1;
        fusionManager.SetUnitNum(unitNum);
        
        Destroy(obj);
    }
    private void AttackCheck()
    {
        if (!followingMouse)
        {
            //
            float usingDistance;
       
            if (buildingIsShortest)
                usingDistance = attackDistance + 2f; // 건물 오브젝트의 currentPosition이 꽤 안쪽에 있어 근접 유닛이 공격할 때
                                                     // 건물 이미지 안으로 들어가는 사이드 이펙트를 해결하기 위함.
            else
                usingDistance = attackDistance;
            //
            if (shortestEnemyDistance < usingDistance)
            {
                StartCoroutine(Attack());
            }
        }
    }
    void Move()
    {
        int stopByEnemyDistance = 1;

        if (!attackedCheck && !isDead)
            anim.Play("WalkR");

        if(buildingIsShortest)
        {
            stopByEnemyDistance = 5;
        }

        if(shortestScript != null)
        {
                if(shortestScript.getHe() <= 0f)
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
                 LevelUp(unitId, shortestScript.unitId, unitLev, shortestScript.unitLev); 
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
    public void LevelUp(int aId, int bId, int aLev, int bLev)
    {
        if ((aId == bId) && (aLev == bLev))
        {
            fusionManager.SetIsAround(true);
            int money = gameManager.GetMoney() - levelUpCost;

            gameManager.SetMoney(money);

            int a = shortestScript.GetUnitLev() + 1;
            shortestScript.SetUnitLev(a);

            StartCoroutine(IsAroundSet());
        }
        else
        {
            ComeBack();
        }

        gameManager.SetCSt(true);

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
                if(fusionManager.unitScript[a].GetThisUnitNum() < thisUnitNum) // 해당 unit오브젝트가 먼저 소환된 오브젝트인지 체크
                {
                    if (objectDistanceArray[a] < LShortestForwardDistance) // 이 코드는 사이드 이펙트가 발생한다.
                                                                           // 사이드 이펙트에 대비하거나 다른 코드를 이용해보자.
                    {
                        LShortestForwardDistance = objectDistanceArray[a];
                        shortestForwardDistance = LShortestForwardDistance;
                        shortestForwardScipt = fusionManager.unitScript[a];
                        shortestForwardIsSet = true;
                    }
                }

                if(!shortestForwardIsSet)
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
        if (isFollow)
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
        if (Input.GetMouseButton(0))
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
