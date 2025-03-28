using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject minionBulletPrefab;
    private Transform tempSpellTr;

    private bool isAttacking = false;

    private readonly float findRadius = 3.5f;
    private float attackSpeed, moveSpeed;
    protected float maxHp, curHp;
    protected Hp hp;

    protected int goldAmount = 10;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, findRadius);
    //}

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        hp = GetComponentInChildren<Hp>();
    }

    public void Init(Vector3 targetPoint, GameObject minionBulletPrefab, Transform tempSpellTr)
    {
        navMeshAgent.destination = targetPoint;

        this.minionBulletPrefab = minionBulletPrefab;
        this.tempSpellTr = tempSpellTr;
    }

    /// <summary>
    /// 미니언 스탯 Init
    /// </summary>
    /// <param name="attackSpeed">낮을수록 공격속도가 증가</param>
    /// <param name="moveSpeed">높을수록 이동속도가 증가</param>
    /// <param name="maxHp">높을수록 체력이 증가</param>
    protected virtual void InitMinionStats(float attackSpeed, float moveSpeed, float maxHp)
    {
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.maxHp = maxHp;
        curHp = this.maxHp;
    }

    protected void Attack(bool isRed)
    {
        GameObject target = null;
        if (isRed) target = FindTarget("Blue_Minion");
        else if (!isRed) target = FindTarget("Red_Minion");

        if (target == null)
        {
            if (isRed) target = FindTarget("Blue_Tower");
            else if (!isRed) target = FindTarget("Red_Tower");
        }

        if (target == null || isAttacking) return;

        MinionBullet minionBullet = Instantiate(minionBulletPrefab, transform).GetComponent<MinionBullet>();
        minionBullet.transform.SetParent(tempSpellTr);

        if (isRed) minionBullet.Init(target, LayerMask.NameToLayer("Red_Minion_Bullet"));
        else if (!isRed) minionBullet.Init(target, LayerMask.NameToLayer("Blue_Minion_Bullet"));

        StartCoroutine(AttackTimer());
    }

    private GameObject FindTarget(string layerName)
    {
        Collider[] targetColls = Physics.OverlapSphere(transform.position, findRadius, LayerMask.GetMask(layerName));
        if (targetColls.Length != 0)
        {
            GameObject curTarget = targetColls[0].gameObject;
            float minDis = Vector3.Distance(curTarget.transform.position, transform.position);
            foreach (Collider target in targetColls)
            {
                float curDis = Vector3.Distance(target.transform.position, transform.position);
                if (minDis > curDis)
                {
                    minDis = curDis;
                    curTarget = target.gameObject;
                }
            }

            navMeshAgent.speed = 0;
            return curTarget;
        }
        else
        {
            navMeshAgent.speed = moveSpeed;
            return null;
        }
    }

    private IEnumerator AttackTimer()
    {
        isAttacking = true;

        float curTime = 0;
        float limitTime = attackSpeed;

        while (true)
        {
            if (curTime >= limitTime)
            {
                isAttacking = false;
                break;
            }

            curTime += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
}
