using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalioR : MonoBehaviour
{
    private Galio galio;
    [SerializeField] SphereCollider sphereCollider;
    private List<GameObject> targets = new List<GameObject>();
    private WaitForSeconds waitForSeconds;
    private float playerOriginPosY;
    [SerializeField] Transform tempTr, circleParant;
    [SerializeField] SpriteRenderer centerCircleSR, totalCircleSR;  // *** Circle의 Scale을 에디터상에서 수동으로 구현했으므로 airBornCenterRange, airBornTotalRange를 변경할 때마다 수동으로 수정 필요 ***

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION))
        {
            targets.Add(other.gameObject);
            other.GetComponent<PunchingBagController>().Hit(galio.Damage);
        }
    }

    private void Awake()
    {
        galio = GetComponentInParent<Galio>();
        sphereCollider.enabled = false;
        waitForSeconds = new WaitForSeconds(Time.deltaTime * 5);  //로비씬에서부터 비동기 씬 로드로 시작할 경우 * 5
        circleParant.gameObject.SetActive(false);
    }

    private void Start()
    {
        sphereCollider.radius = galio.AirBornTotalRange;
        playerOriginPosY = ChampionController.Champion.transform.position.y;
    }

    private void Update()
    {
        if (ChampionController.usingSkill) ChampionController.movePos = ChampionController.Champion.transform.position;

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Levitate());
    }

    private IEnumerator Levitate()
    {
        Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layer = LayerMask.GetMask(ChampionController.CHAMPION);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            circleParant.gameObject.SetActive(true);
            circleParant.SetParent(tempTr);
            circleParant.transform.position = hit.collider.transform.position;

            Vector3 hitPoint = hit.collider.transform.position;
            ChampionController.Champion.transform.LookAt(hitPoint);
            ChampionController.usingSkill = true;
            ChampionController.animator.SetBool(ChampionController.aniSpellR, true);
            yield return new WaitForSeconds(1.1f);

            int maxHeight = 8;
            int speed = 80;

            while (true)
            {
                if (ChampionController.Champion.transform.position.y >= maxHeight)
                {
                    StartCoroutine(Rush(hitPoint));
                    yield break;
                }

                ChampionController.Champion.transform.Translate(speed * Time.deltaTime * Vector3.up);
                yield return waitForSeconds;
            }
        }
    }

    private IEnumerator Rush(Vector3 hitPoint)
    {
        int divisionCount = 8;
        float forwardSpeed = 500;

        while (true)
        {
            if ((hitPoint - ChampionController.Champion.transform.position).magnitude <= Mathf.Abs(ChampionController.Champion.transform.position.y + divisionCount))
            {
                StartCoroutine(Glide(hitPoint, divisionCount));
                yield break;
            }
            else ChampionController.Champion.transform.Translate(forwardSpeed * Time.deltaTime * Vector3.forward);
            yield return waitForSeconds;
        }
    }

    private IEnumerator Glide(Vector3 hitPoint, int divisionCount)
    {
        int count = 0;

        Vector3 dir = hitPoint - ChampionController.Champion.transform.position;
        float dis = dir.magnitude;
        Vector3 ratio = dir.normalized * (dis / divisionCount);

        while (true)
        {
            if (count >= divisionCount)
            {
                Vector3 tempPos = ChampionController.Champion.transform.position;
                tempPos.y = playerOriginPosY;
                ChampionController.Champion.transform.position = tempPos;
                break;
            }

            count++;
            ChampionController.Champion.transform.position += ratio;
            sphereCollider.enabled = true;
            yield return waitForSeconds;
        }

        AirBorn();

        yield return new WaitForSeconds(0.05f);
        ChampionController.usingSkill = false;
        ChampionController.animator.SetBool(ChampionController.aniSpellR, false);

        circleParant.gameObject.SetActive(false);
        circleParant.SetParent(transform);
    }

    /// <summary>
    /// R스킬 에어본 범위 Gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        if (ChampionController.Champion == null) return;
        if (galio == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ChampionController.Champion.transform.position, galio.AirBornCenterRange);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(ChampionController.Champion.transform.position, sphereCollider.radius);
    }

    /// <summary>
    /// R스킬 에어본 - 중앙, 그 외
    /// </summary>
    private void AirBorn()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            //중앙 에어본
            if ((targets[i].transform.position - ChampionController.Champion.transform.position).magnitude <= galio.AirBornCenterRange) StartCoroutine(AirBorn_Up(targets[i], 2f));
            //그 외 에어본
            else if ((targets[i].transform.position - ChampionController.Champion.transform.position).magnitude > galio.AirBornCenterRange 
                && (targets[i].transform.position - ChampionController.Champion.transform.position).magnitude <= sphereCollider.radius) StartCoroutine(AirBorn_Up(targets[i], 1f));
        }

        sphereCollider.enabled = false;
        targets.Clear();
    }

    /// <summary>
    /// R스킬 에어본 - 업
    /// </summary>
    /// <param name="target"></param>
    /// <param name="airBornTime"></param>
    /// <returns></returns>
    private IEnumerator AirBorn_Up(GameObject target, float airBornTime)
    {
        float targetOriginPosY = target.transform.position.y;

        while (true)
        {
            if (target.transform.position.y - targetOriginPosY >= airBornTime)
            {
                StartCoroutine(AirBorn_Down(target, targetOriginPosY, airBornTime));
                yield break;
            }

            target.transform.Translate(25 * airBornTime * Time.deltaTime * Vector3.up / airBornTime);
            yield return waitForSeconds;
        }
    }

    /// <summary>
    /// R스킬 에어본 - 다운
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetOriginPosY"></param>
    /// <param name="airBornTime"></param>
    /// <returns></returns>
    private IEnumerator AirBorn_Down(GameObject target, float targetOriginPosY, float airBornTime)
    {
        while (true)
        {
            if (target.transform.position.y <= targetOriginPosY) break;
            target.transform.Translate(15 * airBornTime * Time.deltaTime * Vector3.down / airBornTime);
            yield return waitForSeconds;
        }
    }
}
