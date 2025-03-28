using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagController : Stats
{
    [SerializeField] GameObject punchingBag;
    private int curHp, curSp;
    private Hp hp;

    protected override void InitPunchingBagStats(int moveSpeed, int maxHp, int maxSp) => base.InitPunchingBagStats(moveSpeed, maxHp, maxSp);

    protected override void Awake()
    {
        InitPunchingBagStats(1, 100, 15);

        curHp = MaxHp;
        curSp = MaxSp;
        hp = GetComponentInChildren<Hp>();
        hp.SetHpAndShield(curHp, MaxHp, curSp);
    }

    protected override void Update()
    {
        ResetHp();
    }

    public void Hit(int damage)
    {
        if (curSp > 0)
        {
            curSp -= damage;
            if (curSp < 0) curHp -= Mathf.Abs(curSp);
        }
        else curHp -= damage;

        hp.SetHpAndShield(curHp, MaxHp, curSp);
    }

    private void ResetHp()
    {
        if (curHp <= 0)
        {
            curHp = MaxHp;
            hp.SetHpAndShield(curHp, MaxHp, curSp);
        }
    }
}