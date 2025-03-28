using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissFortuneQ : MonoBehaviour
{
    private MissFortune missFortune;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform tempSpellTr;
    private GameObject gizmosBullet;

    private readonly float pahse1Range = 4f;
    private readonly float phase2Range = 5f;
    private readonly float phase2RangeAngle = 30f;
    private readonly float bulletSpeed = 7f;

    private void OnDrawGizmos()
    {
        if (gizmosBullet == null) return;

        Handles.color = Color.red;
        Handles.DrawSolidArc(gizmosBullet.transform.position, Vector3.up, gizmosBullet.transform.forward, -phase2RangeAngle, phase2Range);  //왼쪽
        Handles.DrawSolidArc(gizmosBullet.transform.position, Vector3.up, gizmosBullet.transform.forward, phase2RangeAngle, phase2Range);  //오른쪽
    }

    private void Awake()
    {
        missFortune = GetComponentInParent<MissFortune>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) UseQ();
    }

    private void UseQ()
    {
        Ray ray = ChampionController.quarterViewCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layer = LayerMask.GetMask(ChampionController.CHAMPION, "Red_Minion");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            if (Vector3.Distance(hit.transform.position, transform.position) > pahse1Range) return;

            ChampionController.Champion.LookAt(hit.point);
            ChampionController.movePos = ChampionController.Champion.position;

            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.transform.transform.SetParent(tempSpellTr);
            StartCoroutine(bullet.GetComponent<MissFortune_Bullet>().UseQ(hit.collider.gameObject, phase2Range, bulletSpeed, missFortune.Damage));
        }
    }

    /// <summary>
    /// 부채꼴 모양의 시야 내부에 대상이 존재하는지 판별
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public GameObject CircularSector(GameObject bullet, GameObject[] targets)
    {
        gizmosBullet = bullet;

        List<GameObject> tempTargets = new List<GameObject>();

        for (int i = 0; i < targets.Length; i++)
        {
            //판별 도중 타겟이 사라진 경우
            if (targets[i] == null)
            {
                Destroy(bullet);
                break;
            }

            Vector3 dir = targets[i].transform.position - bullet.transform.position;
            float dot = Vector3.Dot(bullet.transform.forward, dir.normalized);
            float targetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (targetAngle <= phase2RangeAngle)
            {
                Debug.Log("시야 내부 Target : " + targets[i].name);
                tempTargets.Add(targets[i]);
            }
        }

        return Find(bullet, tempTargets.ToArray());
    }

    /// <summary>
    /// 가장 가까운 대상 찾기
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    private GameObject Find(GameObject bullet, GameObject[] targets)
    {
        if (targets.Length == 0) return null;

        GameObject curTarget = targets[0];
        float shortDis = Vector3.Distance(targets[0].transform.position, bullet.transform.position);

        for (int i = 0; i < targets.Length; i++)
        {
            float dis = Vector3.Distance(targets[i].transform.position, bullet.transform.position);
            if (dis < shortDis)
            {
                curTarget = targets[i];
                shortDis = dis;
            }
        }

        Debug.Log("가장 가까운 Target : " + curTarget.name);
        return curTarget;
    }
}
