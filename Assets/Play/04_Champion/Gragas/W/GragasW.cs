using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GragasW : MonoBehaviour
{
    //private Gragas gragas;
    //private float curDamage;

    private readonly int buffDamage = 5;
    private readonly float durationTime = 5;
    private readonly int id = 0;

    //private void Awake()
    //{
    //    gragas = GetComponentInParent<Gragas>();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) UseW();

        ChampionController.buffController.Update_Buff();
    }

    private void UseW()
    {
        ChampionController.buffController.Add_Buff(true, durationTime, StartAction, EndAction, id, BuffController.BuffType.IncreDamage);
    }

    private void StartAction()
    {
        ChampionController.Champion.GetComponent<ChampionController>().Damage += buffDamage;
        StartCoroutine(AnimationTimer());
        Debug.Log("W 사용 후 데미지 : " + ChampionController.Champion.GetComponent<ChampionController>().Damage);
    }

    private void EndAction()
    {
        ChampionController.Champion.GetComponent<ChampionController>().Damage -= buffDamage;
        Debug.Log("W 사용 전 데미지 : " + ChampionController.Champion.GetComponent<ChampionController>().Damage);
    }

    private IEnumerator AnimationTimer()
    {
        // *** 걸으면서 W스킬을 사용하는 애니매이션의 부재 - 아바타가 없어서 아바타 마스크를 활용한 방법도 불가 *** 
        ChampionController.animator.SetBool(ChampionController.aniSpellW, true);
        yield return new WaitForSeconds(0.3f);
        ChampionController.animator.SetBool(ChampionController.aniSpellW, false);
    }
}