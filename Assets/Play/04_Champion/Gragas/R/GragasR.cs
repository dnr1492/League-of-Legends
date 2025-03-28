using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GragasR : MonoBehaviour
{
    private Gragas gragas;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Sprite sprCircle;
    private GameObject indicateGo;
    private SpriteRenderer sr;
    
    private readonly float explosionRange = 5;

    private void Awake()
    {
        gragas = GetComponentInParent<Gragas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) UseR();
    }

    private void UseR()
    {
        Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = ChampionController.Champion.position.y;

            ChampionController.Champion.LookAt(hitPoint);
            ChampionController.movePos = ChampionController.Champion.position;

            IndicateExplosionRanage(hitPoint);

            GameObject bomb = Instantiate(bombPrefab);
            bomb.transform.localPosition = ChampionController.Champion.position;

            StartCoroutine(ThrowBomb(hitPoint, bomb));
        }
    }

    #region 포물선으로 폭탄 던지기
    /// <summary>
    /// 폭탄 던지기
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="bomb"></param>
    /// <returns></returns>
    private IEnumerator ThrowBomb(Vector3 hitPoint, GameObject bomb)
    {
        float timer = 0;
        float speed = 2;
        float height = 1.5f;

        ChampionController.animator.SetBool(ChampionController.aniSpellR, true);
        ChampionController.usingSkill = true;
        yield return new WaitForSeconds(1.2f);

        while (true)
        {
            if (timer >= 0.1f)
            {
                ChampionController.animator.SetBool(ChampionController.aniSpellR, false);
                ChampionController.usingSkill = false;
            }

            if (bomb.transform.position.y < ChampionController.Champion.position.y)
            {
                indicateGo.SetActive(false);
                ExplodeBomb(bomb);
                yield break;
            }

            timer += speed * Time.deltaTime;
            Vector3 tempPos = Parabola(ChampionController.Champion.position, hitPoint, height, timer);
            bomb.transform.position = tempPos;
            yield return Time.deltaTime;
        }
    }

    /// <summary>
    /// 포물선 계산
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="height"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector3 Parabola(Vector3 start, Vector3 end, float height, float time)
    {
        ////로컬 함수 버전
        //Func<float, float> f = x => -4 * height * x * x + 4 * height * x;  //방법 1
        float f (float x) => -4 * height * x * x + 4 * height * x;  //방법 2

        ////일반 변수 버전
        //float f = -4 * height * time * time + 4 * height * time

        Vector3 mid = Vector3.Lerp(start, end, time);
        return new Vector3(mid.x, f(time) + Mathf.Lerp(start.y, end.y, time), mid.z);
    }
    #endregion

    /// <summary>
    /// 폭탄이 폭발하다
    /// </summary>
    /// <param name="bomb"></param>
    /// <returns></returns>
    private void ExplodeBomb(GameObject bomb)
    {
        float forceAmount = 8;
        float airBounAmount = 1.5f;

        Collider[] colls = Physics.OverlapSphere(bomb.transform.position, explosionRange, LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion"));
        foreach (Collider hit in colls)
        {
            Vector3 dir = hit.transform.position - bomb.transform.position;
            float dis = dir.magnitude;
            dir = dir.normalized;
            dir += Vector3.up / airBounAmount;

            Rigidbody rigidbody;
            if (hit.GetComponent<Rigidbody>() == null) rigidbody = hit.gameObject.AddComponent<Rigidbody>();
            else rigidbody = hit.gameObject.GetComponent<Rigidbody>();
            rigidbody.GetComponent<Collider>().isTrigger = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rigidbody.useGravity = true;
            rigidbody.AddForce(dir * (forceAmount - dis), ForceMode.VelocityChange);

            if (hit.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) hit.GetComponent<PunchingBagController>().Hit(gragas.Damage);
            else if (hit.gameObject.layer == LayerMask.NameToLayer("Red_Minion"))
            {
                StartCoroutine(Stop(rigidbody));
                hit.GetComponent<RedMinion>().Hit(gragas.Damage, true);
            }
        }

        Destroy(bomb);
    }

    /// <summary>
    /// 폭발 범위를 표시하다
    /// </summary>
    /// <param name="hitPoint"></param>
    private void IndicateExplosionRanage(Vector3 hitPoint)
    {
        if (indicateGo == null) indicateGo = new GameObject();
        indicateGo.SetActive(true);
        if (sr == null) sr = indicateGo.AddComponent<SpriteRenderer>();
        sr.name = "Circle";
        sr.sprite = sprCircle;
        sr.transform.eulerAngles = new Vector3(90, 45, 0);
        sr.transform.position = hitPoint;
        sr.transform.localScale = new Vector3(3f, 3f, 3f);
    }

    private IEnumerator Stop(Rigidbody rigidbody)
    {
        if (rigidbody == null) yield break;

        NavMeshAgent navMeshAgent = rigidbody.GetComponent<NavMeshAgent>();
        Vector3 tempTargetPoint = navMeshAgent.destination;
        navMeshAgent.enabled = false;
        yield return new WaitForSeconds(1);

        if (rigidbody == null) yield break;

        navMeshAgent.enabled = true;
        navMeshAgent.destination = tempTargetPoint;
        rigidbody.GetComponent<Collider>().isTrigger = true;
        rigidbody.useGravity = false;
    }
}
