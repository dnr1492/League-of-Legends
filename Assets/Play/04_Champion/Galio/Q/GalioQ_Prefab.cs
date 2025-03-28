using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalioQ_Prefab : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer_left, lineRenderer_right;
    [SerializeField] [Min(2)] int positionCount = 20;
    [SerializeField] Transform target_left, target_right;
    [SerializeField] Transform p0_left, p1_left, p2_left;
    [SerializeField] Transform p0_right, p1_right, p2_right;
    private readonly float goalTime = 1;

    private void Awake()
    {
        lineRenderer_left.enabled = false;
        lineRenderer_right.enabled = false;
        target_left.gameObject.SetActive(false);
        target_right.gameObject.SetActive(false);
    }

    #region Q스킬을 표시하는 용도
    public void DrawBezierLineRenderer_Left(Vector3 hitPoint)
    {
        p2_left.position = hitPoint;

        lineRenderer_left.enabled = true;
        lineRenderer_left.positionCount = positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            float time;
            if (i == 0) time = 0;
            else time = (float)i / (positionCount - 1);

            Vector3 m0 = Vector3.Lerp(p0_left.position, p1_left.position, time);
            Vector3 m1 = Vector3.Lerp(p1_left.position, p2_left.position, time);
            Vector3 b0 = Vector3.Lerp(m0, m1, time);

            lineRenderer_left.SetPosition(i, b0);
        }
    }

    public void DrawBezierLineRenderer_Right(Vector3 hitPoint)
    {
        p2_right.position = hitPoint;

        lineRenderer_right.enabled = true;
        lineRenderer_right.positionCount = positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            float time;
            if (i == 0) time = 0;
            else time = (float)i / (positionCount - 1);

            Vector3 m0 = Vector3.Lerp(p0_right.position, p1_right.position, time);
            Vector3 m1 = Vector3.Lerp(p1_right.position, p2_right.position, time);
            Vector3 b0 = Vector3.Lerp(m0, m1, time);

            lineRenderer_right.SetPosition(i, b0);
        }
    }
    #endregion

    #region Q스킬을 사용하는 용도
    public IEnumerator BezierCurve_Left(Vector3 hitPoint, float duration = 1.0f)
    {
        p2_left.position = hitPoint;

        float time = 0f;
        target_left.gameObject.SetActive(true);

        while (true)
        {
            if (time > goalTime)
            {
                target_left.gameObject.SetActive(false);
                yield break;
            }

            Vector3 m0 = Vector3.Lerp(p0_left.position, p1_left.position, time);
            Vector3 m1 = Vector3.Lerp(p1_left.position, p2_left.position, time);
            target_left.position = Vector3.Lerp(m0, m1, time);

            time += Time.deltaTime / duration;

            yield return null;
        }
    }

    public IEnumerator BezierCurve_Right(Vector3 hitPoint, float duration = 1.0f)
    {
        p2_right.position = hitPoint;

        float time = 0f;
        target_right.gameObject.SetActive(true);

        while (true)
        {
            if (time > goalTime)
            {
                target_right.gameObject.SetActive(false);
                yield break;
            }

            Vector3 m0 = Vector3.Lerp(p0_right.position, p1_right.position, time);
            Vector3 m1 = Vector3.Lerp(p1_right.position, p2_right.position, time);
            target_right.position = Vector3.Lerp(m0, m1, time);

            time += Time.deltaTime / duration;

            yield return null;
        }
    }
    #endregion

    public Vector3 GetPhase2Pos()
    {
        return (p2_left.position + p2_right.position) / 2;
    }

    #region 기즈모
    public float gizmoDetail;
    private List<Vector3> gizmoLeftPoints = new List<Vector3>();
    private List<Vector3> gizmoRightPoints = new List<Vector3>();

    private void OnDrawGizmos()
    {
        GizmoLeft();
        GizmoRight();
    }

    private void GizmoLeft()
    {
        gizmoLeftPoints.Clear();

        if (p0_left != null && p1_left != null && p2_left != null && gizmoDetail > 0)
        {
            for (int i = 0; i < gizmoDetail; i++)
            {
                float t = (i / gizmoDetail);
                Vector3 m0 = Vector3.Lerp(p0_left.position, p1_left.position, t);
                Vector3 m1 = Vector3.Lerp(p1_left.position, p2_left.position, t);
                gizmoLeftPoints.Add(Vector3.Lerp(m0, m1, t));
            }
        }

        for (int i = 0; i < gizmoLeftPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(gizmoLeftPoints[i], gizmoLeftPoints[i + 1]);
        }
    }

    private void GizmoRight()
    {
        gizmoRightPoints.Clear();

        if (p0_right != null && p1_right != null && p2_right != null && gizmoDetail > 0)
        {
            for (int i = 0; i < gizmoDetail; i++)
            {
                float t = (i / gizmoDetail);
                Vector3 m0 = Vector3.Lerp(p0_right.position, p1_right.position, t);
                Vector3 m1 = Vector3.Lerp(p1_right.position, p2_right.position, t);
                gizmoRightPoints.Add(Vector3.Lerp(m0, m1, t));
            }
        }

        for (int i = 0; i < gizmoRightPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(gizmoRightPoints[i], gizmoRightPoints[i + 1]);
        }
    }
    #endregion
}
