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
        dp = firstDp + firstDp / 2;
    }
    private void MinusDp()
    {
        SetParticle(false);
        dp = firstDp;
    }
    private void SetParticle(bool isActive)
    {
        var emissionVal = dependParticle.emission;
        emissionVal.enabled = isActive;
    }
}
