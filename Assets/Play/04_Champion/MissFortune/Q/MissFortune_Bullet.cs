using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_Bullet : MonoBehaviour
{
    private MissFortuneQ missFortuneQ;
    private GameObject phase2Target;
    private Vector3 curTargetFinalPos1, curTargetFinalPos2;

    private void Awake()
    {
        missFortuneQ = FindObjectOfType<MissFortuneQ>();
    }

    public IEnumerator UseQ(GameObject phase1Target, float range, float bulletSpeed, int damage)
    {
        ChampionController.animator.SetBool(ChampionController.aniSpellQ, true);

        while (true)
        {
            //총알이 날아가는 도중 타겟이 사라진 경우
            if (phase1Target == null)
            {
                if (transform.position.ToString("F1") == curTargetFinalPos1.ToString("F1"))
                {
                    FindPhase2Target(phase1Target, range);
                    break;
                }

                if (curTargetFinalPos1 == Vector3.zero)
                {
                    FindPhase2Target(phase1Target, range);
                    break;
                }

                transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
            }
            else
            {
                curTargetFinalPos1 = phase1Target.transform.position;

                if (transform.position.ToString("F1") == phase1Target.transform.position.ToString("F1"))
                {
                    if (phase1Target.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) phase1Target.GetComponent<PunchingBagController>().Hit(damage);  //Hit 1
                    else if (phase1Target.layer == LayerMask.NameToLayer("Red_Minion")) phase1Target.GetComponent<RedMinion>().Hit(damage, true);  //Hit 1

                    FindPhase2Target(phase1Target, range);
                    break;
                }

                transform.LookAt(phase1Target.transform);
                transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
            }
            yield return Time.deltaTime;
        }

        ChampionController.animator.SetBool(ChampionController.aniSpellQ, false);

        while (phase2Target != null)
        {
            //총알이 날아가는 도중 타겟이 사라진 경우
            if (phase2Target == null)
            {
                if (transform.position.ToString("F1") == curTargetFinalPos2.ToString("F1"))
                {
                    Destroy(gameObject);
                    break;
                }

                if (curTargetFinalPos2 == Vector3.zero)
                {
                    Destroy(gameObject);
                    break;
                }

                transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
            }
            else
            {
                curTargetFinalPos2 = phase2Target.transform.position;

                if (transform.position.ToString("F1") == phase2Target.transform.position.ToString("F1"))
                {
                    if (phase2Target.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) phase2Target.GetComponent<PunchingBagController>().Hit(damage);  //Hit 2
                    else if (phase2Target.layer == LayerMask.NameToLayer("Red_Minion")) phase2Target.GetComponent<RedMinion>().Hit(damage, true);  //Hit 2
                    Destroy(gameObject);
                    break;
                }

                transform.LookAt(phase2Target.transform);
                transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
            }
            yield return Time.deltaTime;
        }
    }

    private void FindPhase2Target(GameObject phase1Target, float range)
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion"));
        List<GameObject> targets = new List<GameObject>();
        for (int i = 0; i < colls.Length; i++)
        {
            if (phase1Target == colls[i].gameObject)
            {
                Debug.Log("2페이즈 타겟 중에서 1페이즈 타겟은 제외시키기");
                continue;
            }
            targets.Add(colls[i].gameObject);
        }

        phase2Target = missFortuneQ.CircularSector(gameObject, targets.ToArray());
        if (phase2Target == null) Destroy(gameObject);  /*phase2Target = null*/;
    }
}
