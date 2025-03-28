using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBullet : MonoBehaviour
{
    private GameObject curTarget;
    private Vector3 curTargetFinalPos;

    private readonly float bulletDamage = 10;
    private readonly float bulletSpeed = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Red_Minion_Bullet")
            && other.gameObject.layer == LayerMask.NameToLayer("Blue_Minion"))
        {
            other.GetComponentInParent<BlueMinion>().Hit(bulletDamage);
            Destroy(gameObject);
        }

        if (gameObject.layer == LayerMask.NameToLayer("Blue_Minion_Bullet")
            && other.gameObject.layer == LayerMask.NameToLayer("Red_Minion"))
        {
            other.GetComponentInParent<RedMinion>().Hit(bulletDamage);
            Destroy(gameObject);
        }

        if (gameObject.layer == LayerMask.NameToLayer("Red_Minion_Bullet")
            && other.gameObject.layer == LayerMask.NameToLayer("Blue_Tower"))
        {
            other.GetComponentInParent<BlueTower>().Hit(bulletDamage);
            Destroy(gameObject);
        }

        if (gameObject.layer == LayerMask.NameToLayer("Blue_Minion_Bullet")
            && other.gameObject.layer == LayerMask.NameToLayer("Red_Tower"))
        {
            other.GetComponentInParent<RedTower>().Hit(bulletDamage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (curTarget != null)
        {
            transform.LookAt(curTarget.transform);
            curTargetFinalPos = curTarget.transform.position;
        } 
        if (transform.position.ToString("F1") == curTargetFinalPos.ToString("F1")) Destroy(gameObject);
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    public void Init(GameObject target, int layer)
    {
        curTarget = target;
        gameObject.layer = layer;
    }
}