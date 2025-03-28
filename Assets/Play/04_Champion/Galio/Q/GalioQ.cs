using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalioQ : MonoBehaviour
{
    private Galio galio;
    [SerializeField] Transform galioTr, tempTr, qTr;
    [SerializeField] GameObject galio_Q_Prefab;
    private GalioQ_Prefab galio_Q;
    [SerializeField] Sprite sprCircle;
    private GameObject phase2_Q_Go;
    private SpriteRenderer phase2_Q_SR;
    private Vector3 hitPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION))
        {
            other.GetComponent<PunchingBagController>().Hit(galio.Damage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Red_Minion"))
        {
            other.GetComponent<RedMinion>().Hit(galio.Damage, true);
        }
    }

    private void Awake()
    {
        galio = GetComponentInParent<Galio>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                hitPoint = hit.point;
                hitPoint.y = 0;

                Set_Q_SkillRange();
                StartCoroutine(UseQ_1Phase());

                Indicate_Q_1Phase();
                StartCoroutine(Indicate_Q_2Phase());
            }
        }
    }

    /// <summary>
    /// Q스킬 1페이즈 사용
    /// </summary>
    /// <returns></returns>
    private IEnumerator UseQ_1Phase()
    {
        ChampionController.Champion.LookAt(hitPoint);

        galio_Q = Instantiate(galio_Q_Prefab, qTr).GetComponent<GalioQ_Prefab>();
        qTr.gameObject.transform.SetParent(tempTr);

        ChampionController.animator.SetBool(ChampionController.aniSpellQ, true);
        ChampionController.movePos = ChampionController.Champion.position;

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(galio_Q.BezierCurve_Left(hitPoint));
        StartCoroutine(galio_Q.BezierCurve_Right(hitPoint));

        yield return new WaitForSeconds(0.6f);
        ChampionController.animator.SetBool(ChampionController.aniSpellQ, false);

        yield return new WaitForSeconds(0.6f);
        StartCoroutine(UseQ_2Phase());

        yield return new WaitForSeconds(Time.deltaTime);
        Destroy(galio_Q.gameObject);
    }

    /// <summary>
    /// Q스킬 2페이즈 사용
    /// </summary>
    /// <returns></returns>
    private IEnumerator UseQ_2Phase()
    {
        GameObject phase2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        phase2.name = "2Phase";
        phase2.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        phase2.transform.position = galio_Q.GetPhase2Pos();
        phase2.transform.SetParent(qTr);

        SphereCollider sphereCollider = phase2.GetComponent<SphereCollider>();
        sphereCollider.radius = 0.5f;

        if (sphereCollider != null) sphereCollider.enabled = false;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = false;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = false;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        if (sphereCollider != null) sphereCollider.enabled = false;
        Destroy(phase2);
        qTr.gameObject.transform.SetParent(galioTr);
        qTr.transform.localPosition = Vector3.zero;
        qTr.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    /// <summary>
    /// Q스킬 1페이즈 범위 표시
    /// </summary>
    private void Indicate_Q_1Phase()
    {
        galio_Q.DrawBezierLineRenderer_Left(hitPoint);
        galio_Q.DrawBezierLineRenderer_Right(hitPoint);
    }

    /// <summary>
    /// Q스킬 2페이즈 범위 표시
    /// </summary>
    /// <returns></returns>
    private IEnumerator Indicate_Q_2Phase()
    {
        if (phase2_Q_Go == null) phase2_Q_Go = new GameObject();
        phase2_Q_Go.SetActive(true);
        if (phase2_Q_SR == null) phase2_Q_SR = phase2_Q_Go.AddComponent<SpriteRenderer>();
        phase2_Q_SR.name = "Circle";
        phase2_Q_SR.sprite = sprCircle;
        phase2_Q_SR.transform.eulerAngles = new Vector3(90, 45, 0);
        phase2_Q_SR.transform.position = hitPoint;
        phase2_Q_SR.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(Time.deltaTime);
        phase2_Q_Go.SetActive(false);
    }

    /// <summary>
    /// Q스킬 최소 및 최대 사거리 설정
    /// </summary>
    private void Set_Q_SkillRange()
    {
        int skillRangeMinQ = galio.SkillRangeMinQ;
        int skillRangeMaxQ = galio.SkillRangeMaxQ;
        Vector3 championPos = ChampionController.Champion.position;
        Vector3 dir = hitPoint - championPos;
        float dis = dir.magnitude;
        if (dis < skillRangeMinQ) dis = skillRangeMinQ;
        if (dis > skillRangeMaxQ) dis = skillRangeMaxQ;
        hitPoint = championPos + dir.normalized * dis;
    }
}
