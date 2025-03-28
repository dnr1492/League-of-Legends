using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticBezierCurve : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] [Min(2)] int positionCount = 20;
    [SerializeField] Transform target;
    [SerializeField] Transform p0, p1, p2;

    private void Update()
    {
        DrawBezierLineRenderer();
        StartCoroutine(BezierCurve());
    }

    public void DrawBezierLineRenderer()
    {
        lineRenderer.positionCount = positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            float time;
            if (i == 0) time = 0;
            else time = (float)i / (positionCount - 1);

            Vector3 m0 = Vector3.Lerp(p0.position, p1.position, time);
            Vector3 m1 = Vector3.Lerp(p1.position, p2.position, time);
            Vector3 b0 = Vector3.Lerp(m0, m1, time);
            target.position = b0;

            lineRenderer.SetPosition(i, target.position);
        }
    }

    public IEnumerator BezierCurve(float duration = 1.0f)
    {
        float time = 0f;

        while (true)
        {
            if (time > 1f) time = 0;

            Vector3 m0 = Vector3.Lerp(p0.position, p1.position, time);
            Vector3 m1 = Vector3.Lerp(p1.position, p2.position, time);
            Vector3 b0 = Vector3.Lerp(m0, m1, time);
            target.position = b0;

            time += Time.deltaTime / duration;

            yield return null;
        }
    }

    #region ±âÁî¸ð
    public float gizmoDetail;
    private List<Vector3> gizmoPoints = new List<Vector3>();

    private void OnDrawGizmos()
    {
        gizmoPoints.Clear();

        if (p0 != null && p1 != null && p2 != null && gizmoDetail > 0)
        {
            for (int i = 0; i < gizmoDetail; i++)
            {
                float t = (i / gizmoDetail);
                Vector3 m0 = Vector3.Lerp(p0.position, p1.position, t);
                Vector3 m1 = Vector3.Lerp(p1.position, p2.position, t);
                gizmoPoints.Add(Vector3.Lerp(m0, m1, t));
            }
        }

        for (int i = 0; i < gizmoPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(gizmoPoints[i], gizmoPoints[i + 1]);
        }
    }
    #endregion
}
