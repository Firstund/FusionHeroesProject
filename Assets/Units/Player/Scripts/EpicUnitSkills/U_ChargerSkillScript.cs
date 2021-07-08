using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_ChargerSkillScript : MonoBehaviour
{
    [SerializeField]
    private UnitScript thisScript = null;
    [SerializeField]
    private float stopByEnemy = 1f;
    [SerializeField]
    private float skillDelay = 1f;
    [SerializeField]
    private float skillRange = 5f;
    [SerializeField]
    private float skillMoveSpeed = 3f;
    [SerializeField]
    private Vector2 targetPosition = Vector2.zero;
    [SerializeField]
    private bool canUseSkill = true;
    [SerializeField]
    private bool isCharging = false;

    void Start()
    {
        thisScript = GetComponent<UnitScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSpeed();
        SetCanSetSpeed();
        if (canUseSkill && thisScript.shortestEnemyDistance <= skillRange)
            Charge();
        MoveBack();
    }
    private void SetSpeed()
    {
        if (thisScript.shortestEnemyDistance <= thisScript.stopByEnemyDistance)
        {
            thisScript.speed = 0f;
        }
        else if (!canUseSkill)
        {
            thisScript.speed = skillMoveSpeed;
        }
        else
        {
            thisScript.speed = thisScript.firstSpeed;
            isCharging = false;
        }
    }
    private void MoveBack()
    {
        if(thisScript.shortestEnemyDistance < stopByEnemy && isCharging && thisScript.shortestEnemyScript != null)
        {
            thisScript.shortestEnemyScript.MoveBack();
        }
    }
    private void Charge()
    {
        if (canUseSkill && !thisScript.buildingIsShortest)
        {
            canUseSkill = false;
            isCharging = true;

            targetPosition = thisScript.GetCurrentPosition();
            targetPosition.x += skillRange;

            thisScript.speed = skillMoveSpeed;
            thisScript.canSetSpeed = false;

            Invoke("SetCanUseSkill", skillDelay);
        }
    }
    private void SetCanSetSpeed()
    {
        if (targetPosition.x <= thisScript.GetCurrentPosition().x)
        {
            thisScript.canSetSpeed = true;
            isCharging = false;
        }
    }
    private void SetCanUseSkill()
    {
        canUseSkill = true;
    }
}
