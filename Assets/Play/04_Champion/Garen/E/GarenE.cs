using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarenE : MonoBehaviour
{
    private Garen garen;
    private bool isUsingE = false;

    private readonly float findRadius = 3;
    private readonly float rotTime = 2f;
    private readonly int hitTimes = 10;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, findRadius);
    }

    private void Awake()
    {
        garen = GetComponentInParent<Garen>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(UseE());
    }

    private IEnumerator UseE()
    {
        if (isUsingE) yield break;

        //float rotSpeed = 300f;
        float curTime = 0;
        float curHitTimes = rotTime / hitTimes;
        int curHitCount = 1;
        
        while (true)
        {
            if (curTime >= rotTime)
            {
                ChampionController.animator.SetBool(ChampionController.aniSpellE, false);
                isUsingE = false;
                yield break;
            }

            if (curTime.ToString("F1") == (curHitTimes * curHitCount).ToString("F1"))
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, findRadius, LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion"));
                foreach (Collider target in colls)
                {
                    if (target.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) target.GetComponent<PunchingBagController>().Hit(garen.Damage);
                    else if (target.gameObject.layer == LayerMask.NameToLayer("Red_Minion")) target.GetComponent<RedMinion>().Hit(garen.Damage, true);
                }
                curHitCount++;
            }

            curTime += Time.deltaTime;
            //transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
            ChampionController.animator.SetBool(ChampionController.aniSpellE, true);
            isUsingE = true;
            yield return Time.deltaTime;
        }
    }
}
