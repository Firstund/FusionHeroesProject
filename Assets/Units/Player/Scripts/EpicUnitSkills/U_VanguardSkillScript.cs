using UnityEngine;

public class U_VanguardSkillScript : MonoBehaviour
{
    [SerializeField]
    private UnitScript thisScript = null;
    [SerializeField]
    private ParticleSystem dependParticle = null;
    void Start()
    {
        thisScript = GetComponent<UnitScript>();
    }
    public void SetDP()
    {
        if (thisScript.canAttack)
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
        thisScript.dp = thisScript.firstDp + thisScript.firstDp / 2;
    }
    private void MinusDp()
    {
        SetParticle(false);
        thisScript.dp = thisScript.firstDp;
    }
    private void SetParticle(bool isActive)
    {
        var emissionVal = dependParticle.emission;
        emissionVal.enabled = isActive;
    }
}
