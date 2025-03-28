using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaisaQ : MonoBehaviour
{
    private Kaisa kaisa;
    [SerializeField] GameObject kaisaQ_Prefab;
    [SerializeField] Transform tempSpellTr;

    private readonly int skillRange = 5;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }

    private void Awake()
    {
        kaisa = GetComponentInParent<Kaisa>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(UseQ());
    }

    private IEnumerator UseQ()
    {
        int shotCount = 10;  //총 발사 개수

        GameObject hitTarget = Find();

        ChampionController.animator.SetBool(ChampionController.aniSpellQ, true);
        yield return new WaitForSeconds(0.1f);
        ChampionController.animator.SetBool(ChampionController.aniSpellQ, false);

        while (shotCount > 0)
        {
            float random = UnityEngine.Random.Range(0.3f, 0.6f);
            GameObject go = Instantiate(kaisaQ_Prefab, transform);
            StartCoroutine(go.GetComponent<KaisaQ_Prefab>().Bezier(hitTarget, random, kaisa.Damage));
            go.transform.SetParent(tempSpellTr);
            shotCount--;
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 가장 가까운 대상 찾기
    /// </summary>
    /// <returns></returns>
    private GameObject Find()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, skillRange, LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion"));
        if (colls.Length == 0) return null;
        GameObject curTarget = colls[0].gameObject;
        float shortDis = Vector3.Distance(curTarget.transform.position, kaisa.transform.position);
        foreach (Collider target in colls)
        {
            float dis = Vector3.Distance(target.transform.position, kaisa.transform.position);
            if (dis < shortDis)
            {
                curTarget = target.gameObject;
                shortDis = dis;
            }
        }

        Debug.Log("가장 가까운 Target : " + curTarget.name);
        return curTarget;
    }
}