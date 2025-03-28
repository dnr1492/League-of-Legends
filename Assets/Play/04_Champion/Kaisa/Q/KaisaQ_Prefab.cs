using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaisaQ_Prefab : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject p0, p1, p2, p3;

    private GameObject curHitTarget;
    private int curDamage;
    private Transform quarterViewCamTr;
    private readonly float distanceFromStart = 3;  //시작 지점을 기준으로 얼마나 꺾을 지
    private readonly float distanceFromEnd = 1;  //도착 지점을 기준으로 얼마나 꺾을 지

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION))
        {
            if (other.gameObject == curHitTarget) other.GetComponent<PunchingBagController>().Hit(curDamage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Red_Minion"))
        {
            if (other.gameObject == curHitTarget) other.GetComponent<RedMinion>().Hit(curDamage, true);
        }
    }

    private void Update()
    {
        if (curHitTarget == null) return;
        p3.transform.position = curHitTarget.transform.position;
    }

    public IEnumerator Bezier(GameObject hitTarget, float arrivalTime, int damage)
    {
        if (hitTarget == null)
        {
            Destroy(gameObject);
            yield break;
        }

        p3.transform.position = hitTarget.transform.position;
        curHitTarget = hitTarget;
        curDamage = damage;
        quarterViewCamTr = ChampionController.quarterViewCam.transform;
        
        Randomize();

        float time = 0f;

        while (true)
        {
            if (target.transform.position.ToString("F1") == p3.transform.position.ToString("F1"))
            {
                Destroy(gameObject);
                break;
            }

            Vector3 m0 = Vector3.Lerp(p0.transform.position, p1.transform.position, time);
            Vector3 m1 = Vector3.Lerp(p1.transform.position, p2.transform.position, time);
            Vector3 m2 = Vector3.Lerp(p2.transform.position, p3.transform.position, time);

            Vector3 b0 = Vector3.Lerp(m0, m1, time);
            Vector3 b1 = Vector3.Lerp(m1, m2, time);
            Vector3 bezier = Vector3.Lerp(b0, b1, time);

            target.transform.position = bezier;

            time += Time.deltaTime / arrivalTime;
            yield return Time.deltaTime;
        }
    }

    private void Randomize()
    {
        //시작 지점을 기준으로 랜덤 포인트 지정
        p1.transform.position = p0.transform.position +
            (distanceFromStart * Random.Range(-1f, 1f) * quarterViewCamTr.right) +  //X (좌, 우 전체)
            (distanceFromStart * Random.Range(-1f, 1f) * quarterViewCamTr.up) +  //Y (상, 하 전체)
            (distanceFromStart * Random.Range(-1f, -0.8f) * quarterViewCamTr.forward);  //Z (뒤만)

        //도착 지점을 기준으로 랜덤 포인트 지정
        p2.transform.position = p3.transform.position +
            (distanceFromEnd * Random.Range(-1f, 1f) * quarterViewCamTr.right) +  //X (좌, 우 전체)
            (distanceFromEnd * Random.Range(-1f, 1f) * quarterViewCamTr.up) +  //Y (상, 하 전체)
            (distanceFromEnd * Random.Range(0.8f, 1f) * quarterViewCamTr.forward);  //Z (앞만)
    }
}