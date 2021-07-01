using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPooling : MonoBehaviour
{
    [SerializeField]
    private List<UnitScript> _units;
    public List<UnitScript> units
    {
        get { return _units; }
        set { _units = value; }
    }

    public bool Go(int unitId)
    {
        foreach (var item in units)
        {
            if (item.GetUnitID() == unitId)
            {
                item.gameObject.SetActive(true);
                item.SpawnSet();
                units.Remove(item);

                return true;
            }
        }

        return false;
    }
}
