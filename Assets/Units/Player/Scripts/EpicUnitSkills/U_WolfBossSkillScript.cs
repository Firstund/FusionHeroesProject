using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_WolfBossSkillScript : MonoBehaviour
{
    private float firstSpeed = 0f;
    private UnitScript thisScript = null;
    [SerializeField]
    private GameObject wolf = null;
    [SerializeField]
    private GameObject soundBox = null;
    [SerializeField]
    private float wolfSpawnTime = 0f;
    [SerializeField]
    private float minusWolfSpawnTimePerLev = 0.02f;
    private bool isHowling = true;
    private Transform spawnPosition = null;
    private void Start()
    {
        thisScript = GetComponent<UnitScript>();
        spawnPosition = GameManager.Instance.GetUnitSpawnPosition();
        Invoke("ResetIsHowling", wolfSpawnTime);
        firstSpeed = thisScript.speed;
    }
    private void Update()
    {
        if (!isHowling && thisScript.canAttack && !thisScript.isDead)
        {
            thisScript.canAttack = false;
            HowlingSound();

            thisScript.canSetSpeed = false;
            thisScript.speed = 0f;
            isHowling = true;
            thisScript.anim.Play("Howling");
        }
    }
    private void SpawnWolf()
    {
        // ResetSpeed();
        Vector2 a = spawnPosition.position;
        a.x -= 0.1f;
        Instantiate(wolf, a, Quaternion.identity);

    }
    private void HowlingSound()
    {
        Instantiate(soundBox); // 아우우우우우우우우웅우ㅜ
    }
    private void ResetSpeed()
    {
        thisScript.canAttack = true;
        thisScript.speed = firstSpeed;
        thisScript.canSetSpeed = true;
        Invoke("ResetIsHowling", wolfSpawnTime - minusWolfSpawnTimePerLev * thisScript.GetUnitLev());
    }
    private void ResetIsHowling()
    {
        isHowling = false;
    }
}
