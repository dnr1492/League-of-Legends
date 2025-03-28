using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField] Text txtDamage, txtSpeed;
    private float preDamage, preSpeed;

    public void SetStatus(float damage, float speed)
    {
        if (preDamage != damage)
        {
            txtDamage.text = "���ݷ� : " + damage.ToString();
            preDamage = damage;
        }

        if (preSpeed != speed)
        {
            txtSpeed.text = "�ӵ� : " + speed.ToString();
            preSpeed = speed;
        }
    }
}