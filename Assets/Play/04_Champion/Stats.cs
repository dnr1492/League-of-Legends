using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //이동 속도
    public int MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    private int _moveSpeed;

    //최대 체력
    public int MaxHp { get => _maxHp; }
    private int _maxHp;

    //최대 방어막
    public int MaxSp { get => _maxSp; }
    private int _maxSp;

    //공격력
    public int Damage { get => _damage; set => _damage = value; }
    private int _damage;

    //Q스킬 최소 사거리
    public int SkillRangeMinQ { get => _skillRangeMinQ; }
    private int _skillRangeMinQ;

    //Q스킬 최대 사거리
    public int SkillRangeMaxQ { get => _skillRangeMaxQ; }
    private int _skillRangeMaxQ;

    //W스킬 최소 사거리
    public float SkillRangeMinW { get => _skillRangeMinW; }
    private float _skillRangeMinW;

    //W스킬 최대 사거리
    public float SkillRangeMaxW { get => _skillRangeMaxW; }
    private float _skillRangeMaxW;

    //E스킬 돌진 속도
    public int SkillRushSpeed { get => _skillRushSpeed; }
    private int _skillRushSpeed;

    //E스킬 돌진 거리
    public int SkillRushDistance { get => _skillRushDistance; }
    private int _skillRushDistance;

    //E스킬 에어본 시간
    public float AirBornTime { get => _airBornTime; }
    private float _airBornTime;

    //R스킬 에어본 중앙 사거리
    public float AirBornCenterRange { get => _airBornCenterRange; }
    private float _airBornCenterRange;

    //R스킬 에어본 그 외 사거리
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

        Debug.Log("챔피언 " + "\n"
            + "이동 속도 : " + _moveSpeed + "\n"
            + "공격력 : " + _damage + "\n"
            + "Q스킬 최소 사거리 : " + _skillRangeMinQ + "\n"
            + "Q스킬 최대 사거리 : " + _skillRangeMaxQ + "\n"
            + "W스킬 최소 사거리 : " + _skillRangeMinW + "\n"
            + "W스킬 최대 사거리 : " + _skillRangeMaxW + "\n"
            + "E스킬 돌진 속도 : " + _skillRushSpeed + "\n"
            + "E스킬 돌진 거리 : " + _skillRushDistance + "\n"
            + "E스킬 에어본 시간 : " + _airBornTime + "\n"
            + "R스킬 에어본 중앙 사거리 : " + _airBornCenterRange + "\n"
            + "R스킬 에어본 그 외 사거리 " + _airBornTotalRange);
    }

    protected virtual void InitPunchingBagStats(int moveSpeed, int maxHp, int maxSp)
    {
        _moveSpeed = moveSpeed;
        _maxHp = maxHp;
        _maxSp = maxSp;

        Debug.Log("허수아비 (샌드백) " + "\n"
            + "이동 속도 : " + _moveSpeed + "\n"
            + "최대 체력 : " + _maxHp + "\n"
            + "최대 방어막 : " + _maxSp);
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
