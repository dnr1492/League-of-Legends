using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AurelionSolW_Prefab : MonoBehaviour
{
    [SerializeField] List<GameObject> objs = new List<GameObject>();  //개수
    [SerializeField] GameObject center;
    private float deg;
    private float objSpeed;  //속도
    private float circleRadiusSize;  //반지름 크기
    private bool lookAtCenter = false;

    private void Update()
    {
        deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                float degAverage = deg + (i * (360 / objs.Count));
                float rad = Mathf.Deg2Rad * degAverage;
                float x = circleRadiusSize * Mathf.Sin(rad);
                float z = circleRadiusSize * Mathf.Cos(rad);
                objs[i].transform.position = center.transform.position + new Vector3(x, 0.5f, z);
                if (lookAtCenter) objs[i].transform.rotation = Quaternion.Euler(0, degAverage, 0);  //Center를 바라보도록 하기
            }
        }
        else deg = 0;
    }

    public void Init(float objSpeed, float circleRadiusSize, bool lookAtCenter)
    {
        this.objSpeed = objSpeed;
        this.circleRadiusSize = circleRadiusSize;
        this.lookAtCenter = lookAtCenter;
    }
}
