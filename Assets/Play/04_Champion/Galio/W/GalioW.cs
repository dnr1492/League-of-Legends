using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalioW : MonoBehaviour
{
    private Galio galio;
    private SphereCollider sphereCollider;
    private List<GameObject> targets;
    private bool isInputW = false, isProvoking = false, isAutoProvoking = false;
    [SerializeField] SpriteRenderer circleSR;

    private readonly float circleOriginScale = 0.9f;
    private float curInputTime = 0;
    private readonly float limitMaxInputTime = 1.5f, limitMinInputTime = 0.5f;
    private float increaseSum = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!isInputW) return;
        if (other.gameObject.layer == LayerMask.NameToLayer(ChampionController.CHAMPION)) targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInputW || !isProvoking) return;
        for (int i = 0; i < targets.Count; i++)
        {
            StartCoroutine(UseW_DrawNear(targets[i]));
            if (i == targets.Count - 1) isInputW = false;
            Debug.Log(targets[i].name);
        }
    }

    private void Awake()
    {
        galio = GetComponentInParent<Galio>();
        circleSR.gameObject.SetActive(false);
    }

    private void Start()
    {
        sphereCollider = ChampionController.Champion.transform.Find("W").GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChampionController.animator.SetBool(ChampionController.aniSpellW, true);
            targets = new List<GameObject>();
            sphereCollider.radius = galio.SkillRangeMinW;
            isInputW = true;

            StartCoroutine(LimitMaxInputTime());
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (isAutoProvoking) return;
            if (sphereCollider.radius >= galio.SkillRangeMaxW) sphereCollider.radius = galio.SkillRangeMaxW;
            if (sphereCollider.radius < galio.SkillRangeMinW) sphereCollider.radius = galio.SkillRangeMinW;
            if (increaseSum < (circleOriginScale * 2) - circleOriginScale) CalSamePercentageIncrease();

            sphereCollider.radius += Time.deltaTime;
            isProvoking = false;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            StartCoroutine(LimitMinInputTime());
        }
    }

    /// <summary>
    /// W스킬 도발 (끌어당기기)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator UseW_DrawNear(GameObject target)
    {
        float curDrawNearTime = 0;
        float maxDrawNearTime = 1.5f;
        float moveSpeed = 1;

        while (true)
        {
            if (curDrawNearTime >= maxDrawNearTime) yield break;
            curDrawNearTime += Time.deltaTime;
            target.transform.LookAt(ChampionController.Champion.transform.position);
            target.transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// W스킬 최대 입력 시간 제한
    /// </summary>
    /// <returns></returns>
    private IEnumerator LimitMaxInputTime()
    {
        circleSR.gameObject.SetActive(true);

        isAutoProvoking = false;

        while (true)
        {
            if (curInputTime >= limitMaxInputTime)
            {
                curInputTime = 0;
                ChampionController.animator.SetBool(ChampionController.aniSpellW, false);
                sphereCollider.radius = 0;
                isProvoking = true;
                isAutoProvoking = true;
                circleSR.transform.localScale = new Vector3(circleOriginScale, circleOriginScale, circleOriginScale);
                circleSR.gameObject.SetActive(false);
                yield break;
            }

            curInputTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /// <summary>
    /// W스킬 최소 입력 시간 제한
    /// </summary>
    /// <returns></returns>
    private IEnumerator LimitMinInputTime()
    {
        if (curInputTime < limitMinInputTime) yield return new WaitForSeconds(limitMinInputTime - curInputTime);
        curInputTime = limitMaxInputTime;

        ChampionController.animator.SetBool(ChampionController.aniSpellW, false);
        sphereCollider.radius = 0;
        isProvoking = true;
        circleSR.transform.localScale = new Vector3(circleOriginScale, circleOriginScale, circleOriginScale);
        circleSR.gameObject.SetActive(false);
        increaseSum = 0;
    }

    /// <summary>
    /// 서로 다른 수인 비교값과 변수값이 각각 2배수가 될 때까지 증가할 때 비교값의 증가량과 변수값의 증가량이 일치되도록 계산
    /// </summary>
    private void CalSamePercentageIncrease()
    {
        //변수값의 증가량 == 비교값의 증가량 / (비교값의 2배수 - 비교값) * (변수값의 2배수 - 변수값)
        float increase = Time.deltaTime / (galio.SkillRangeMaxW - galio.SkillRangeMinW) * (circleOriginScale * 2 - circleOriginScale);
        circleSR.transform.localScale = new Vector3(circleSR.transform.localScale.x + increase, circleSR.transform.localScale.y + increase, circleSR.transform.localScale.z + increase);
        increaseSum += increase;
    }
}
