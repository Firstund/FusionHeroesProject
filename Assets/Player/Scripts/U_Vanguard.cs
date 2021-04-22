using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Vanguard : UnitScript
{
    [SerializeField]
    private ParticleSystem dependParticle = null;
    public void SetDP()
    {
        if (attackedCheck)
        {
            PlusDp();
        }
        else
        {
            MinusDp();
        }
    }
    private void PlusDp()
    {
        SetParticle(true);
        dp = firsstDp + firsstDp / 2;
    }
    private void MinusDp()
    {
        SetParticle(false);
        dp = firsstDp;
    }
    private void SetParticle(bool isActive)
    {
        var emissionVal = dependParticle.emission;
        emissionVal.enabled = isActive;
    }
}
