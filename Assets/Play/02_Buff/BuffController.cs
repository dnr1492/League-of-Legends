using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [SerializeField] GameObject[] arrBuffPrefab;

    public enum BuffType { IncreDamage, IncreDeffence, IncreSpeed }

    private readonly Dictionary<int, Buff> dicBuff = new();
    private readonly Dictionary<int, GameObject> dicBuffEffect = new();
    
    public void Update_Buff()
    {
        foreach (int key in dicBuff.Keys.ToList())
        {
            Buff buffData = dicBuff[key];

            if (!buffData.isUsing) buffData.durationTime = 0.0f;
            buffData.durationTime -= Time.deltaTime;

            if (buffData.durationTime <= 0) Delete_Buff(key);
            else dicBuff[key] = buffData;
        }
    }

    public void Add_Buff(bool isUsing, float durationTime, Action startAction, Action endAction, int id, BuffType buffType)
    {
        if (dicBuff.ContainsKey(id))
        {
            return;
        }
        else
        {
            AttachEffect(id, buffType);
            dicBuff.Add(id, new Buff(isUsing, durationTime, endAction));
            startAction();
        }
    }

    public void Add_Buff_Refresh(bool isUsing, float durationTime, Action startAction, Action endAction, int id, BuffType buffType)
    {
        if (dicBuff.ContainsKey(id))
        {
            dicBuff[id] = new Buff(isUsing, durationTime, endAction);
        }
        else
        {
            AttachEffect(id, buffType);
            dicBuff.Add(id, new Buff(isUsing, durationTime, endAction));
            startAction();
        }
    }

    public List<int> GetAll_Buff()
    {
        return dicBuff.Keys.ToList();
    }

    public void Delete_Buff(int id)
    {
        if (!dicBuff.ContainsKey(id)) return;

        if (dicBuffEffect.ContainsKey(id))
        {
            if (dicBuffEffect[id] != null) Destroy(dicBuffEffect[id]);
            dicBuffEffect.Remove(id);
        }

        Action endAction = dicBuff[id].endAction;
        dicBuff.Remove(id);
        endAction();
    }

    public void DeleteAll_Buff()
    {
        foreach (var key in dicBuff.Keys.ToList())
        {
            Delete_Buff(key);
        }
    }

    private void AttachEffect(int id, BuffType buffType)
    {
        if (!dicBuffEffect.ContainsKey(id))
        {
            dicBuffEffect.Add(id, Instantiate(arrBuffPrefab[(int)buffType], ChampionController.Champion));
        }
    }
}