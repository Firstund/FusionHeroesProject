using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    private int followingUnitNum = 0;
    [SerializeField]
    private int unitNum = 0;
    [SerializeField]
    private int enemyUnitNum = 0;

    //private int a = 0;

    private bool isAround = false;

    public UnitScript[] unitScript = null;
    private double unitNO = 0f;
    public EnemyScript[] enemyScript = null;
    private double enemyUnitNO = 0f;
    public BuildingScript buildingScript = null;
    public EnemyBuildingScript enemyBuildingScript = null;

    private GameManager gameManager = null;

    private GameObject[] fusionObject = null;


    private bool monsterSpawned = false;

    private bool isDowned = false;
    private bool isUped = false;

    private bool isFollow = false;
    private bool canSetScripts = false;

    // Start is called before the first frame update

    void Start()
    {
        buildingScript = FindObjectOfType<BuildingScript>();
        enemyBuildingScript = FindObjectOfType<EnemyBuildingScript>();
        unitScript = FindObjectsOfType(typeof(UnitScript)) as UnitScript[];
        enemyScript = FindObjectsOfType(typeof(EnemyScript)) as EnemyScript[];
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        SetScripts();
    }
    private void SetScripts()
    {
        if (canSetScripts)
        {
            unitScript = FindObjectsOfType(typeof(UnitScript)) as UnitScript[];
            enemyScript = FindObjectsOfType(typeof(EnemyScript)) as EnemyScript[];
            canSetScripts = false;
        }
    }
    public void SetCanSetScripts()
    {
        canSetScripts = true;
    }
    public void StageReset()
    {
        buildingScript.Reset();
        enemyBuildingScript.Reset();

        for (int i = 0; i < unitNum; i++)
        {
            unitScript[i].Destroye(unitScript[i]);

        }
        for (int i = 0; i < enemyUnitNum; i++)
        {
            enemyScript[i].Destroye();
        }
    }
    public bool GetIsFollow()
    {
        return isFollow;
    }
    public void SetIsFollow(bool a)
    {
        isFollow = a;
    }
    public bool GetIsUped()
    {
        return isUped;
    }
    public void SetIsUped(bool a)
    {
        isUped = a;
    }
    public bool GetIsDowned()
    {
        return isDowned;
    }
    public void SetIsDowned(bool a)
    {
        isDowned = a;
    }
    public bool GetIsAround()
    {
        return isAround;
    }
    public void SetIsAround(bool a)
    {
        isAround = a;
    }
    public int GetFollowingUnitNum()
    {
        return followingUnitNum;
    }
    public void SetFollowingUnitNum(int a)
    {
        followingUnitNum = a;
    }
    public int GetUnitNum()
    {
        return unitNum;
    }
    public void SetUnitNum(int a)
    {
        unitNum = a;
    }
    public int GetEnemyUnitNum()
    {
        return enemyUnitNum;
    }
    public void SetEnemyUnitNum(int a)
    {
        enemyUnitNum = a;
    }
    public void SetUnitNO(double a)
    {
        unitNO = a;
    }
    public double GetUnitNO()
    {
        return unitNO;
    }
    public void SetEnemyUnitNO(double a)
    {
        enemyUnitNO = a;
    }
    public double GetEnemyUnitNO()
    {
        return enemyUnitNO;
    }
    public void clickMouseCheck()
    {
        isAround = false;
        monsterSpawned = false;
        isDowned = true;
        isUped = false;
    }
    public void upClickMouseCheck()
    {
        isFollow = false;
        isDowned = false;
        isUped = true;
        StartCoroutine(UpedRe());
    }
    private IEnumerator UpedRe()
    {
        yield return new WaitForSeconds(0.1f);
        isUped = false;
    }

}
