using System.Collections;
using UnityEngine;

public class Galio : ChampionController
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

        InitChampionStats(5, 9, 4, 10, 1.5f, 3f, 150, 6, 1.5f, 2.5f, 6);
    }

    protected override void Update()
    {
        base.Update();
    }
}
