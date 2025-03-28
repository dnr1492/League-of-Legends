using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garen : ChampionController
{
    protected override void InitChampionStats(int moveSpeed, int damage,
        int skillRangeMinQ, int skillRangeMaxQ,
        float skillRangeMinW, float skillRangeMaxW,
        int skillRushSpeed, int skillRushDistance, float airBornTime,
        float airBornCenterRange, float airBornTotalRange)
        =>
        base.InitChampionStats(moveSpeed, damage,
            skillRangeMinQ, skillRangeMaxQ,
            skillRangeMinW, skillRangeMaxW,
            skillRushSpeed, skillRushDistance, airBornTime,
            airBornCenterRange, airBornTotalRange);

    protected override void Awake()
    {
        base.Awake();

        InitChampionStats(5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }

    protected override void Update()
    {
        base.Update();
    }
}
