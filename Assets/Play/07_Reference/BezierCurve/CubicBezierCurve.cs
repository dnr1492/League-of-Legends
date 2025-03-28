using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] [Min(2)] int positionCount = 20;
    [SerializeField] GameObject p0, p1, p2, p3;

    private void Update()
    {
        DrawBezierLineRenderer();
    }

    private void DrawBezierLineRenderer()
    {
        lineRenderer.positionCount = positionCount;

        for (int i = 0; i < positionCount; i++)
        {
            float time;
            if (i == 0) time = 0;
            else time = (float)i / (positionCount - 1);

            Vector3 m0 = Vector3.Lerp(p0.transform.position, p1.transform.position, time);
            Vector3 m1 = Vector3.Lerp(p1.transform.position, p2.transform.position, time);
            Vector3 m2 = Vector3.Lerp(p2.transform.position, p3.transform.position, time);

            Vector3 b0 = Vector3.Lerp(m0, m1, time);
            Vector3 b1 = Vector3.Lerp(m1, m2, time);
            Vector3 bezier = Vector3.Lerp(b0, b1, time);

            lineRenderer.SetPosition(i, bezier);
        }
    }
}