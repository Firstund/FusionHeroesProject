using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : PopUpScaleScript
{
    void Start()
    {
        PlusStart();
    }

    void Update()
    {
        SetScale();
    }
}
