public class BlueMinion : MinionController
{
    protected override void InitMinionStats(float attackSpeed, float moveSpeed, float maxHp) => base.InitMinionStats(attackSpeed, moveSpeed, maxHp);

    protected override void Awake()
    {
        base.Awake();

        InitMinionStats(5, 3.5f, 50);

        hp.SetHpAndShield(curHp, maxHp);
    }

    private void Update()
    {
        Attack(false);
    }

    public void Hit(float damage)
    {
        curHp -= damage;
        if (curHp <= 0) Destroy(gameObject);

        hp.SetHpAndShield(curHp, maxHp);
    }
}
