using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strongest1Script : MonoBehaviour
{
    [SerializeField]
    private bool skillUsed = false;

    [SerializeField]
    private bool[] canUseSkill = new bool[2] { true, true };
    [SerializeField]
    private AudioClip[] skillSound = new AudioClip[2] { null, null };

    private EnemyScript thisObjectScript = null;
    private UnitScript shortestScript = null;
    private GameManager gameManager = null;
    private Animator anim = null;

    [SerializeField]
    private float[] skillWait = new float[2] { 0.3f, 0.3f };
    [SerializeField]
    private float[] skillDistance = new float[2] { 1f, 1f };
    [SerializeField]
    private float[] skillDamage = new float[2];

    void Start()
    {
        thisObjectScript = GetComponent<EnemyScript>();
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        shortestScript = thisObjectScript.GetShortest();

        if (gameManager.GetCST())
        {
            StartCoroutine(skill1());
            StartCoroutine(skill2());
        }
    }
    private IEnumerator skill1()//Warp
    {
        bool distanceCheck = (thisObjectScript.GetShortestDistance() < skillDistance[0] &&
         thisObjectScript.GetAttackDistance() < thisObjectScript.GetShortestDistance());


        if ((shortestScript != null) && canUseSkill[0] && distanceCheck)
        {
            thisObjectScript.GetAudi().clip = skillSound[0];
            thisObjectScript.GetAudi().Play();

            skillUsed = true;
            canUseSkill[0] = false;

            if (!thisObjectScript.GetIsDead())
                anim.Play("strongest1Warp");

            float time = 0.3f;

            thisObjectScript.AttackedCheck(time);

            yield return new WaitForSeconds(time);

            skillUsed = false;
            StartCoroutine(skill1Re());

            transform.localPosition = shortestScript.transform.localPosition;//스킬 사용

            //skill1Used를 EnemyScript에서 참조할 수 있도록 하고 Skill1Used가 true일 때 이동, 공격 기능이 정지되도록 해보자.
        }
    }
    private IEnumerator skill1Re()
    {
        yield return new WaitForSeconds(skillWait[0]);

        if (!thisObjectScript.GetIsDead())
            canUseSkill[0] = true;
    }
    private IEnumerator skill2()
    {
        bool distanceCheck = (thisObjectScript.GetShortestDistance() < skillDistance[1]);


        if ((shortestScript != null) && canUseSkill[1] && distanceCheck)
        {
            thisObjectScript.GetAudi().clip = skillSound[1];
            thisObjectScript.GetAudi().Play();

            skillUsed = true;
            canUseSkill[1] = false;

            if (!thisObjectScript.GetIsDead())
                anim.Play("Strongest1Attack2");

            float time = 1f;

            thisObjectScript.AttackedCheck(time);

            //애니 출력

            thisObjectScript.DoAttackSkill(false, 10f, time, 0f, skillDistance[1]);//(단일공격인가?, 스킬 데미지, 스킬 쿨타임, 광역공격일 때 사용되는 광역공격 범위들(미니멈, 멕시멈))

            yield return new WaitForSeconds(time);

            skillUsed = false;
            StartCoroutine(skill2Re());
        }
    }
    private IEnumerator skill2Re()
    {
        yield return new WaitForSeconds(skillWait[1]);

        if (!thisObjectScript.GetIsDead())
            canUseSkill[1] = true;
    }
    public bool GetSkillUsed()
    {
        return skillUsed;
    }
}
