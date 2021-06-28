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

    [SerializeField]
    private Transform _playerUnitSpawnPosition = null;
    public Transform playerUnitSpawnPosition
    {
        get { return _playerUnitSpawnPosition; }
    }

    //private int a = 0;

    [SerializeField]
    private UnitScript[] _unitScript = null;
    public UnitScript[] unitScript
    {
        get { return _unitScript; }
        set { _unitScript = value; }
    }
    private double unitNO = 0f;
    [SerializeField]
    private EnemyScript[] _enemyScript = null;
    public EnemyScript[] enemyScript
    {
        get { return _enemyScript; }
        set { _enemyScript = value; }
    }
    [SerializeField]
    private List<ProjectionScript> _projectionScripts = new List<ProjectionScript>();
    public List<ProjectionScript> projectionScripts
    {
        get { return _projectionScripts; }
        set { _projectionScripts = value; }
    }
    private double enemyUnitNO = 0f;
    [SerializeField]
    private BuildingScript _buildingScript = null;
    public BuildingScript buildingScript
    {
        get { return _buildingScript; }
        set { _buildingScript = value; }
    }
    [SerializeField]
    private EnemyBuildingScript _enemyBuildingScript = null;
    public EnemyBuildingScript enemyBuildingScript
    {
        get { return _enemyBuildingScript; }
        set { _enemyBuildingScript = value; }
    }

    private GameManager gameManager = null;

    private bool isUped = false;

    private bool isFollow = false;
    private bool canSetScripts = false;

    // Start is called before the first frame update

    void Start()
    {
        buildingScript = FindObjectOfType<BuildingScript>();
        enemyBuildingScript = FindObjectOfType<EnemyBuildingScript>();
        _unitScript = FindObjectsOfType(typeof(UnitScript)) as UnitScript[];
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
            _unitScript = FindObjectsOfType(typeof(UnitScript)) as UnitScript[];
            enemyScript = FindObjectsOfType(typeof(EnemyScript)) as EnemyScript[];
            canSetScripts = false;
        }
    }
    public void SetCanSetScripts()
    {
        canSetScripts = true;
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
        isUped = false;
    }
    public void upClickMouseCheck()
    {
        isFollow = false;
        isUped = true;
        StartCoroutine(UpedRe());
    }
    private IEnumerator UpedRe()
    {
        yield return new WaitForSeconds(0.1f);
        isUped = false;
    }

}
