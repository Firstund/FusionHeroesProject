using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    [SerializeField]
    private List<EnemyScript> _enemies;
    public List<EnemyScript> enemies
    {
        get { return _enemies; }
        set { _enemies = value; }
    }

    public bool Go(int unitId)
    {
        foreach (var item in enemies)
        {
            if (item.GetUnitID() == unitId)
            {
                item.gameObject.SetActive(true);
                item.SpawnSet();
                enemies.Remove(item);

                return true;
            }
        }

        return false;
    }
}
