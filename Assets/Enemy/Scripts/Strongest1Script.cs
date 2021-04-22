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
    private float[] skillReTime = new float[2] { 0.3f, 0.3f }; // 스킬 재사용 쿨타임
    [SerializeField]
    private float[] skiilDelay = new float[2]; // 스킬 딜레이
    [SerializeField]
    private float[] skillDistance = new float[2] { 1f, 1f };
    [SerializeField]
    private float[] skillDamage = new float[2];
    [SerializeField]
    private float deathSpeed = 1f;

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

        if(thisObjectScript.GetIsDead())
        {
            transform.Translate(Vector2.right * deathSpeed * Time.deltaTime);
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

            thisObjectScript.AttackedCheck(skiilDelay[0]); // 재사용 대기시간이 빠른 스킬이 있을 땐 빼줘야 하는 코드
                                                           // 재사용 대기시간이 빠른 스킬이 실행중일 때 다른 공격이 실행되지 않길
                                                           // 원하면 이 코드를 실행시키되, 위에 조건문에 !thisObjectScript.GetAttackedCheck();를 추가하자

            yield return new WaitForSeconds(skiilDelay[0]);

            skillUsed = false;
            StartCoroutine(skill1Re());

            transform.localPosition = shortestScript.transform.localPosition;//스킬 사용
        }
    }
    private IEnumerator skill1Re()
    {
        yield return new WaitForSeconds(skillReTime[0]);

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


            thisObjectScript.AttackedCheck(skiilDelay[1]);
            Debug.Log("aaa");

            //애니 출력

            thisObjectScript.DoAttackSkill(false, skillDamage[1], skiilDelay[1], 0f, skillDistance[1]);//(단일공격인가?, 스킬 데미지, 스킬 쿨타임, 광역공격일 때 사용되는 광역공격 범위들(미니멈, 멕시멈))

            yield return new WaitForSeconds(skiilDelay[1]);

            skillUsed = false;
            StartCoroutine(skill2Re());
        }
    }
    private IEnumerator skill2Re()
    {
        yield return new WaitForSeconds(skillReTime[1]);

        if (!thisObjectScript.GetIsDead())
            canUseSkill[1] = true;
    }
    public bool GetSkillUsed()
    {
        return skillUsed;
    }
}
