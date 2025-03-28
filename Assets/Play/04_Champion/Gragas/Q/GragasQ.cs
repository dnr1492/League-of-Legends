using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GragasQ : MonoBehaviour
{
    private Gragas gragas;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform TempSpellTr;
    private GameObject curBomb;
    private SphereCollider sphereCollider;
    private Vector3 hitPoint;
    private bool isUsingQ = false;

    private readonly float explosionRange = 2;

    private void Awake()
    {
        gragas = GetComponentInParent<Gragas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(UseQ());
    }

    private IEnumerator UseQ()
    {
        float speed = 10;

        isUsingQ = !isUsingQ;

        if (isUsingQ)
        {
            Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitPoint = hit.point;
                hitPoint.y = ChampionController.Champion.transform.position.y;

                ChampionController.Champion.LookAt(hitPoint);

                ChampionController.animator.SetBool(ChampionController.aniSpellQ, true);
                yield return new WaitForSeconds(0.4f);
                ChampionController.animator.SetBool(ChampionController.aniSpellQ, false);

                if (curBomb != null) yield break;
                curBomb = Instantiate(bombPrefab, transform);
                curBomb.transform.SetParent(TempSpellTr);
                sphereCollider = curBomb.GetComponent<SphereCollider>();
                sphereCollider.radius = explosionRange;

                Vector3 tempBomb = curBomb.transform.position;
                tempBomb.y = ChampionController.Champion.transform.position.y;

                curBomb.transform.position = tempBomb;
                curBomb.transform.LookAt(hitPoint);

                float curTime = 0;
                float limitTime = 2;

                while (true)
                {
                    if (curBomb.transform.position.ToString("F1") == hitPoint.ToString("F1"))
                    {
                        //일정 시간 후 자동 폭발
                        while (true)
                        {
                            if (curBomb == null) yield break;

                            if (curTime >= limitTime)
                            {
                                Explode();
                                isUsingQ = !isUsingQ;
                                yield break;
                            }

                            curTime += Time.deltaTime;
                            yield return Time.deltaTime;
                        }
                    }

                    curBomb.transform.Translate(speed * Time.deltaTime * Vector3.forward);
                    yield return Time.deltaTime;
                }
            }
        }
        else
        {
            if (curBomb.transform.position.ToString("F1") == hitPoint.ToString("F1")) Explode();
        }
    }

    private void Explode()
    {
        Collider[] arrCol = Physics.OverlapSphere(curBomb.transform.position, sphereCollider.radius, LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion"));
        foreach (Collider col in arrCol)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) col.GetComponent<PunchingBagController>().Hit(gragas.Damage);
            else if (col.gameObject.layer == LayerMask.NameToLayer("Red_Minion")) col.GetComponent<RedMinion>().Hit(gragas.Damage, true);
        }
        Destroy(curBomb);
    }
}
