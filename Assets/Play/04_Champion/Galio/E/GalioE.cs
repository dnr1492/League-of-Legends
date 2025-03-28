using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalioE : MonoBehaviour
{
    private Galio galio;
    private BoxCollider boxCollider;
    private WaitForSeconds waitForSeconds;
    private bool isCollide = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION))
        {
            if (!ChampionController.usingSkill) return;
            isCollide = true;
            StartCoroutine(AirBorn_Up(other.gameObject, galio.AirBornTime));
            boxCollider.enabled = false;
            other.GetComponent<PunchingBagController>().Hit(galio.Damage);
        }
    }

    private void Awake()
    {
        galio = GetComponentInParent<Galio>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        waitForSeconds = new WaitForSeconds(Time.deltaTime * 4);  //로비씬에서부터 비동기 씬 로드로 시작할 경우 * 4
    }

    private void Update()
    {
        if (ChampionController.usingSkill) ChampionController.movePos = ChampionController.Champion.transform.position;

        if (Input.GetKeyDown(KeyCode.E))
        {
            boxCollider.enabled = true;
            Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y = ChampionController.Champion.transform.position.y;
                isCollide = false;
                StartCoroutine(Rush(hitPoint));
            }
        }
    }

    /// <summary>
    /// E스킬 돌진
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    private IEnumerator Rush(Vector3 hitPoint)
    {
        float sumRushDistance = 0;

        ChampionController.usingSkill = true;
        ChampionController.animator.SetBool(ChampionController.aniSpellE, true);
        ChampionController.Champion.transform.LookAt(hitPoint);
        yield return new WaitForSeconds(1);

        while (true)
        {
            if (isCollide)
            {
                ChampionController.animator.SetBool(ChampionController.aniSpellEHit, true);
                yield break;
            }

            if (sumRushDistance >= galio.SkillRushDistance)
            {
                ChampionController.usingSkill = false;
                ChampionController.animator.SetBool(ChampionController.aniSpellE, false);
                yield break;
            }

            ChampionController.Champion.transform.Translate(galio.SkillRushSpeed * Time.deltaTime * Vector3.forward);
            sumRushDistance += Mathf.Abs(galio.SkillRushSpeed * Time.deltaTime * Vector3.forward.z);
            yield return waitForSeconds;
        }
    }

    /// <summary>
    /// E스킬 에어본 업
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
                ChampionController.usingSkill = false;
                StartCoroutine(AirBorn_Down(target, targetOriginPosY, airBornTime));
                yield break;
            }

            target.transform.Translate(25 * airBornTime * Time.deltaTime * Vector3.up / airBornTime);
            yield return waitForSeconds;
        }
    }

    /// <summary>
    /// E스킬 에어본 다운
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

        ChampionController.animator.SetBool(ChampionController.aniSpellE, false);
        ChampionController.animator.SetBool(ChampionController.aniSpellEHit, false);
    }
}
