using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTower : TowerController
{
    protected override void InitTowerStats(float attackSpeed, float maxHp) => base.InitTowerStats(attackSpeed, maxHp);

    protected override void Awake()
    {
        base.Awake();

        InitTowerStats(2.5f, 100);

        hp.SetHpAndShield(curHp, maxHp);
    }

    private void Update()
    {
        Attack(true);
    }

    public void Hit(float damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            if (gameObject.name == "Red_Nexus") GameOver();
            Destroy(gameObject);
        }

        hp.SetHpAndShield(curHp, maxHp);
    }
}