using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strongest2Script : MonoBehaviour
{
    [SerializeField]
    private bool skillUsed = false;

    [SerializeField]
    private bool[] canUseSkill = new bool[2] { true, true };
    [SerializeField]
    private AudioClip[] skillSound = new AudioClip[2] { null, null };
    [SerializeField]
    private new ParticleSystem particleSystem = null;

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
    private float[] skillDamageUpPerStage;

    private bool particleIsPlaying = true;
    bool MaxCheck;

    void Start()
    {
        thisObjectScript = GetComponent<EnemyScript>();
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        shortestScript = thisObjectScript.GetShortest();

        if (thisObjectScript.GetSpeed() == 0f)
        {
            if (particleIsPlaying)
            {
                particleSystem.Stop();
                particleIsPlaying = false;
            }

        }
        else
        {
            if (!particleIsPlaying)
            {
                particleSystem.Play();
                particleIsPlaying = true;
            }

        }

        if (gameManager.GetCST())
        {
            StartCoroutine(skill1());
        }
    }
    private IEnumerator skill1()
    {
        bool MaxCheck = thisObjectScript.GetFirstHP() >= thisObjectScript.getHe();
        if ((shortestScript != null) && canUseSkill[0] && MaxCheck)
        {
            thisObjectScript.GetAudi().clip = skillSound[0];
            thisObjectScript.GetAudi().Play();

            skillUsed = true;
            canUseSkill[0] = false;

            yield return new WaitForSeconds(skiilDelay[0]);

            skillUsed = false;
            StartCoroutine(skill1Re());

            thisObjectScript.SetHP(thisObjectScript.getHe() + skillDamage[0] + skillDamage[0] * gameManager.GetSaveData().currentStage);//스킬 사용
            //

            //skill1Used를 EnemyScript에서 참조할 수 있도록 하고 Skill1Used가 true일 때 이동, 공격 기능이 정지되도록 해보자.
        }
    }
    private IEnumerator skill1Re()
    {
        yield return new WaitForSeconds(skillReTime[0]);

        if (!thisObjectScript.GetIsDead())
            canUseSkill[0] = true;
    }
    public bool GetSkillUsed()
    {
        return skillUsed;
    }
}
