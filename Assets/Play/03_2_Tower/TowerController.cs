using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] GameObject towerBulletPrefab;
    [SerializeField] Transform tempSpellTr;
    [SerializeField] GameObject gameOverPrefab;

    private bool isAttacking = false;

    private readonly float findRadius = 8f;
    private float attackSpeed;
    protected float maxHp, curHp;
    protected Hp hp;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, findRadius);
    //}

    protected virtual void Awake()
    {
        hp = GetComponentInChildren<Hp>();
    }

    /// <summary>
    /// 타워 스탯 Init
    /// </summary>
    /// <param name="attackSpeed">낮을수록 공격속도가 증가</param>
    /// <param name="maxHp">높을수록 체력이 증가</param>
    protected virtual void InitTowerStats(float attackSpeed, float maxHp)
    {
        this.attackSpeed = attackSpeed;
        this.maxHp = maxHp;
        curHp = this.maxHp;
    }

    protected void Attack(bool isRed)
    {
        GameObject target = null;
        if (isRed) target = FindTarget("Blue_Minion");
        else if (!isRed) target = FindTarget("Red_Minion");

        if (target == null || isAttacking) return;

        TowerBullet towerBullet = Instantiate(towerBulletPrefab, transform).GetComponent<TowerBullet>();
        towerBullet.transform.SetParent(tempSpellTr);
        if (isRed) towerBullet.Init(target, LayerMask.NameToLayer("Red_Tower_Bullet"));
        else if (!isRed) towerBullet.Init(target, LayerMask.NameToLayer("Blue_Tower_Bullet"));

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
            return curTarget;
        }
        return null;
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

    protected void GameOver()
    {
        Instantiate(gameOverPrefab);
    }
}
