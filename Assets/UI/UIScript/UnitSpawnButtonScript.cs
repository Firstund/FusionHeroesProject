using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnButtonScript : MonoBehaviour
{
    [SerializeField]
    private List<Transform> unitSpawnButtons;
    [SerializeField]
    private List<int> unitSpawnCosts;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            unitSpawnButtons.Add(transform.GetChild(i));
        }
        foreach (var item in unitSpawnButtons)
        {
            int cost = 9999;

            if (item.GetComponentInChildren<TestSpawnScript>() != null)
            {
                cost = item.GetComponentInChildren<TestSpawnScript>().spawnMoney;
            }

            unitSpawnCosts.Add(cost);
        }

        unitSpawnCosts.Sort();

        List<Transform> _unitSpawnButtons = new List<Transform>();

        foreach (var item in unitSpawnCosts) // Costs값들을 이용해 unitSpawnButtons 정렬
        {
            foreach (var _item in unitSpawnButtons)
            {
                int cost = 9999;

                if (_item.GetComponentInChildren<TestSpawnScript>() != null)
                    cost = _item.GetComponentInChildren<TestSpawnScript>().spawnMoney;

                if (item == cost)
                {
                    bool a = false;

                    foreach (var paste in _unitSpawnButtons) // 중복 방지
                    {
                        if (paste == _item)
                        {
                            a = true;
                            break;
                        }
                    }

                    if (!a)
                    {
                        _unitSpawnButtons.Add(_item);
                        break;
                    }
                }
            }
        }

        unitSpawnButtons = _unitSpawnButtons;

        int j = 0;
        foreach (var item in unitSpawnButtons)
        {
            item.SetSiblingIndex(j);
            j++;
        }
    }

    void Update()
    {

    }
}
