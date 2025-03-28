using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //�̵� �ӵ�
    public int MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    private int _moveSpeed;

    //�ִ� ü��
    public int MaxHp { get => _maxHp; }
    private int _maxHp;

    //�ִ� ��
    public int MaxSp { get => _maxSp; }
    private int _maxSp;

    //���ݷ�
    public int Damage { get => _damage; set => _damage = value; }
    private int _damage;

    //Q��ų �ּ� ��Ÿ�
    public int SkillRangeMinQ { get => _skillRangeMinQ; }
    private int _skillRangeMinQ;

    //Q��ų �ִ� ��Ÿ�
    public int SkillRangeMaxQ { get => _skillRangeMaxQ; }
    private int _skillRangeMaxQ;

    //W��ų �ּ� ��Ÿ�
    public float SkillRangeMinW { get => _skillRangeMinW; }
    private float _skillRangeMinW;

    //W��ų �ִ� ��Ÿ�
    public float SkillRangeMaxW { get => _skillRangeMaxW; }
    private float _skillRangeMaxW;

    //E��ų ���� �ӵ�
    public int SkillRushSpeed { get => _skillRushSpeed; }
    private int _skillRushSpeed;

    //E��ų ���� �Ÿ�
    public int SkillRushDistance { get => _skillRushDistance; }
    private int _skillRushDistance;

    //E��ų ��� �ð�
    public float AirBornTime { get => _airBornTime; }
    private float _airBornTime;

    //R��ų ��� �߾� ��Ÿ�
    public float AirBornCenterRange { get => _airBornCenterRange; }
    private float _airBornCenterRange;

    //R��ų ��� �� �� ��Ÿ�
    public float AirBornTotalRange { get => _airBornTotalRange; }
    private float _airBornTotalRange;

    protected virtual void InitChampionStats(int moveSpeed, int damage, 
        int skillRangeMinQ, int skillRangeMaxQ, float skillRangeMinW, float skillRangeMaxW,
        int skillRushSpeed, int skillRushDistance, float airBornTime,
        float airBornCenterRange, float airBornTotalRange)
    {
        _moveSpeed = moveSpeed;
        _damage = damage;
        _skillRangeMinQ = skillRangeMinQ;
        _skillRangeMaxQ = skillRangeMaxQ;
        _skillRangeMinW = skillRangeMinW;
        _skillRangeMaxW = skillRangeMaxW;
        _skillRushSpeed = skillRushSpeed;
        _skillRushDistance = skillRushDistance;
        _airBornTime = airBornTime;
        _airBornCenterRange = airBornCenterRange;
        _airBornTotalRange = airBornTotalRange;

        Debug.Log("è�Ǿ� " + "\n"
            + "�̵� �ӵ� : " + _moveSpeed + "\n"
            + "���ݷ� : " + _damage + "\n"
            + "Q��ų �ּ� ��Ÿ� : " + _skillRangeMinQ + "\n"
            + "Q��ų �ִ� ��Ÿ� : " + _skillRangeMaxQ + "\n"
            + "W��ų �ּ� ��Ÿ� : " + _skillRangeMinW + "\n"
            + "W��ų �ִ� ��Ÿ� : " + _skillRangeMaxW + "\n"
            + "E��ų ���� �ӵ� : " + _skillRushSpeed + "\n"
            + "E��ų ���� �Ÿ� : " + _skillRushDistance + "\n"
            + "E��ų ��� �ð� : " + _airBornTime + "\n"
            + "R��ų ��� �߾� ��Ÿ� : " + _airBornCenterRange + "\n"
            + "R��ų ��� �� �� ��Ÿ� " + _airBornTotalRange);
    }

    protected virtual void InitPunchingBagStats(int moveSpeed, int maxHp, int maxSp)
    {
        _moveSpeed = moveSpeed;
        _maxHp = maxHp;
        _maxSp = maxSp;

        Debug.Log("����ƺ� (�����) " + "\n"
            + "�̵� �ӵ� : " + _moveSpeed + "\n"
            + "�ִ� ü�� : " + _maxHp + "\n"
            + "�ִ� �� : " + _maxSp);
    }

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }
}
