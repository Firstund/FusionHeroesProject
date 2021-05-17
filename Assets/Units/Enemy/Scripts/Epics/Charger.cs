using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : EnemyScript
{
    private float chargeDelay = 5f;
    private float chargeDistance = 5f;
    private float chargeSpeed = 1f;
    private bool isCharge = false;
    void Update()
    {
        currentPosition = transform.localPosition;
        audi.volume = gameManager.GetSoundValue();

        objectDistanceArray = new float[fusionManager.GetUnitNum()];
        enemyObjectDistanceArray = new float[fusionManager.GetEnemyUnitNum()];

        EDCheck();
        ODCheck();

        if (gameManager.GetCST())
            AttackCheck();

        Move();
        CheckCharge();
        HealthBar();
        DestroyCheck();
    }
    private void CheckCharge()
    {
        if(shortestDistance < chargeDistance && !isCharge)
        {
            Charge();
            isCharge = true;
        }
    }
    private void Charge()
    {
        Vector2 u_targetPosition = Vector2.zero; // 밀쳐진 플레이어 유닛이 이동할 Vector
        Vector2 e_targetPosition = Vector2.zero; // 차저가 돌진 후 이동할 vector
        
        // 적 유닛이 밀쳐질 때, 적 유닛이 날아갈 때 플레이어의 유닛들의 이동에 지장이 있으면 안됀다. 
        // 밀쳐질 때의 상태를 저장해서 밀쳐질 때는 잠시 objectDistanceArray에서 빼둘까?
        // objectDistanceArray에서 빼두기만 하면 모든 유닛들이 이 유닛을 감지하지 못하므로, 될 듯 하다.

        // Lerp()를 이용해 부드러운 Charge를 구현할것.
    }
}
