public class RedMinion : MinionController
{
    protected override void InitMinionStats(float attackSpeed, float moveSpeed, float maxHp) => base.InitMinionStats(attackSpeed, moveSpeed, maxHp);

    protected override void Awake()
    {
        base.Awake();

        InitMinionStats(2.5f, 3.5f, 50);

        hp.SetHpAndShield(curHp, maxHp);
    }

    private void Update()
    {
        Attack(true);
    }

    public void Hit(float damage, bool gainGold = false)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            if (gainGold) GameManager.GetInstance().IncreaseGold(goldAmount);
            Destroy(gameObject);
        }

        hp.SetHpAndShield(curHp, maxHp);
    }
}
