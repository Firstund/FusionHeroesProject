using UnityEngine;

public class ProjectionScript : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private Transform parent = null;
    private UnitScript thisUnitScript = null;
    private EnemyScript thisEnemyScript = null;
    [SerializeField]
    private bool isPlayerProjection = true;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float hitRange = 1f;
    private Vector2 originPosition = Vector2.zero;
    private Vector2 currentPosition = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        originPosition = transform.position;

        if (isPlayerProjection)
        {
            thisUnitScript = transform.parent.transform.parent.transform.GetComponent<UnitScript>();
            parent = GameObject.Find("UnitProjections").transform;
        }
        else
        {
            thisEnemyScript = transform.parent.transform.parent.transform.GetComponent<EnemyScript>();
            parent = GameObject.Find("EnemyProjections").transform;
        }

        transform.SetParent(parent);
    }

    void Update()
    {
        Move();
        CheckAttack();
    }

    private void Move()
    {
        currentPosition = transform.position;

        float distanceToOrigin = Vector2.Distance(currentPosition, originPosition);

        if (isPlayerProjection)
        {
            if(distanceToOrigin >= thisUnitScript.attackDistance)
            {
                Destroy(gameObject);
            }
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            if(distanceToOrigin >= thisEnemyScript.attackDistance)
            {
                Destroy(gameObject);
            }
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void CheckAttack()
    {
        float distance = 0f;

        if (isPlayerProjection)
        {
            if(thisUnitScript.buildingIsShortest)
            {
                distance = Vector2.Distance(currentPosition, gameManager.GetEnemyUnitSpawnPosition().position);
            }
            else
            {
                distance = Vector2.Distance(currentPosition, thisUnitScript.shortestEnemyScript.GetCurrentPosition());
            }

            if (distance <= hitRange)
            {
                thisUnitScript.GetDamage();
                Destroy(gameObject);
            }
        }
        else
        {
            if(thisEnemyScript.buildingIsShortest)
            {
                distance = Vector2.Distance(currentPosition, gameManager.GetUnitSpawnPosition().position);
            }
            else
            {
                distance = Vector2.Distance(currentPosition, thisEnemyScript.shortestScript.GetCurrentPosition());
            }

            if (thisEnemyScript.shortestDistance <= hitRange)
            {
                thisEnemyScript.GetDamage();
                Destroy(gameObject);
            }
        }
    }
}
