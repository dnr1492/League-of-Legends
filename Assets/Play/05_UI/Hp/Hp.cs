using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] Slider sliderHp, sliderDamaged, sliderShield;
    [SerializeField] Image separator;

    private readonly int floatSteps = Shader.PropertyToID("_Steps");
    private readonly int floatHSRatio = Shader.PropertyToID("_HSRatio");
    private readonly int floatWidth = Shader.PropertyToID("_Width");
    private readonly int floatThickness = Shader.PropertyToID("_Thickness");

    private readonly int averageSeparation = 10;
    private readonly float damagedSpeed = 3;

    private void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0, -45, 0);
    }

    public void SetHpAndShield(float curHp, float maxHp, float curSp = 0)
    {
        float step, hpShieldRatio;

        //���尡 ������ ��
        if (curSp > 0)
        {
            //���� ü�� + ���� > �ִ� ü��
            if (curHp + curSp > maxHp)
            {
                //����
                hpShieldRatio = curHp / (curHp + curSp);
                sliderShield.value = 1f;

                //ü��
                step = curHp / averageSeparation;
                sliderHp.value = curHp / (curHp + curSp);
            }
            else
            {
                //����
                hpShieldRatio = curHp / maxHp;
                sliderShield.value = (curHp + curSp) / maxHp;

                //ü��
                step = curHp / averageSeparation;
                sliderHp.value = curHp / maxHp;
            }
        }
        else
        {
            //����
            hpShieldRatio = 1f;
            sliderShield.value = 0;

            //ü��
            step = maxHp / averageSeparation;
            sliderHp.value = curHp / maxHp;
        }

        StopCoroutine(UpdateDamageImpact());
        StartCoroutine(UpdateDamageImpact());

        separator.material.SetFloat(floatSteps, step);
        separator.material.SetFloat(floatHSRatio, hpShieldRatio);
        separator.material.SetFloat(floatWidth, 100);
        separator.material.SetFloat(floatThickness, 1);
    }

    private IEnumerator UpdateDamageImpact()
    {
        while (true)
        {
            if (sliderDamaged.value.ToString("F4") == sliderHp.value.ToString("F4")) yield break;

            yield return new WaitForSeconds(Time.deltaTime * damagedSpeed);
            sliderDamaged.value = Mathf.Lerp(sliderDamaged.value, sliderHp.value, Time.deltaTime * damagedSpeed);
        }
    }
}
