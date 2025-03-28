using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AurelionSolW : MonoBehaviour
{
    private AurelionSol aurelionSol;
    [SerializeField] AurelionSolW_Prefab aurelionSolW_Prefab;

    private readonly float skillSpeedBeforeUse = 40f;
    private readonly float skillSpeedAfterUse = 80f;
    private readonly float skillSwitchSpeed = 5f;
    private readonly float skillRangeBeforeUse = 2.5f;
    private readonly float skillRangeAfterUse = 5f;
    private readonly float skillDurationTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION))
        {
            other.GetComponent<PunchingBagController>().Hit(aurelionSol.Damage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Red_Minion"))
        {
            other.GetComponent<RedMinion>().Hit(aurelionSol.Damage, true);
        }
    }

    private void Awake()
    {
        aurelionSol = GetComponentInParent<AurelionSol>();
        aurelionSolW_Prefab.Init(skillSpeedBeforeUse, skillRangeBeforeUse, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(UseW());
    }

    private IEnumerator UseW()
    {
        yield return StartCoroutine(StartW());

        float limitTime = skillDurationTime;
        float curTime = 0;

        while (true)
        {
            if (curTime >= limitTime) break;
            curTime += Time.deltaTime;
            yield return Time.deltaTime;
        }

        StartCoroutine(EndW());
    }

    private IEnumerator StartW()
    {
        float before = skillRangeBeforeUse;
        float after = skillRangeAfterUse;

        while (true)
        {
            if (before >= after) yield break;
            aurelionSolW_Prefab.Init(skillSpeedAfterUse, before, false);
            before += Time.deltaTime * skillSwitchSpeed;
            yield return Time.deltaTime;
        }
    }

    private IEnumerator EndW()
    {
        float before = skillRangeAfterUse;
        float after = skillRangeBeforeUse;

        while (true)
        {
            if (before <= after) yield break;
            aurelionSolW_Prefab.Init(skillSpeedBeforeUse, before, false);
            before -= Time.deltaTime * skillSwitchSpeed;
            yield return Time.deltaTime;
        }
    }
}
