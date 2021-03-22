using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanelScript : MonoBehaviour
{
    UnitScript unitScript = null;
    FusionManager fusionManager = null;

    Vector2 testUnitPosition = Vector2.zero;
    Vector2 currentPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        unitScript = FindObjectOfType<UnitScript>();
        fusionManager = FindObjectOfType<FusionManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
