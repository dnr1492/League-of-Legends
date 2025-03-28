using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionGenerator : MonoBehaviour
{
    [SerializeField] GameObject red_MinionPrefab, blue_MinionPrefab;
    [SerializeField] Transform[] red_Barracks, blue_Barracks;
    [SerializeField] Transform red_Nexus, blue_Nexus;

    private readonly int minionCount = 3;
    private float curTime = 15;
    private readonly float limitTime = 15;
    private bool isCreating = false;

    [SerializeField] GameObject minionBulletPrefab;
    [SerializeField] Transform tempSpellTr;

    private void Update()
    {
        Create();
    }

    public bool Check()
    {
        if (isCreating) return false;

        if (curTime >= limitTime)
        {
            curTime = 0;
            isCreating = true;
            return true;
        }

        curTime += Time.deltaTime;
        return false;
    }

    public void Create()
    {
        if (!Check()) return;
        for (int i = 0; i < red_Barracks.Length; i++) StartCoroutine(Create_Red(i));
        for (int i = 0; i < blue_Barracks.Length; i++) StartCoroutine(Create_Blue(i));
    }

    private IEnumerator Create_Red(int index)
    {
        for (int i = 0; i < minionCount; i++)
        {
            RedMinion redMinion = Instantiate(red_MinionPrefab, red_Barracks[index]).GetComponent<RedMinion>();
            redMinion.name = LayerMask.LayerToName(redMinion.gameObject.layer) + " " + i;
            redMinion.Init(blue_Nexus.position, minionBulletPrefab, tempSpellTr);

            if (index == 0) redMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Top");
            else if (index == 1) redMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Mid");
            else if (index == 2) redMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Bottom");

            yield return new WaitForSeconds(0.5f);
        }

        isCreating = false;
    }

    private IEnumerator Create_Blue(int index)
    {
        for (int i = 0; i < minionCount; i++)
        {
            BlueMinion blueMinion = Instantiate(blue_MinionPrefab, blue_Barracks[index]).GetComponent<BlueMinion>();
            blueMinion.name = LayerMask.LayerToName(blueMinion.gameObject.layer) + " " + i;
            blueMinion.Init(red_Nexus.position, minionBulletPrefab, tempSpellTr);

            if (index == 0) blueMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Top");
            else if (index == 1) blueMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Mid");
            else if (index == 2) blueMinion.GetComponent<NavMeshAgent>().areaMask += 1 << NavMesh.GetAreaFromName("Bottom");

            yield return new WaitForSeconds(0.5f);
        }

        isCreating = false;
    }
}