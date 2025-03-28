using System;

public class Buff
{
    public bool isUsing;
    public float durationTime;
    public Action endAction;

    public Buff(bool isUsing, float durationTime, Action endAction)
    {
        this.isUsing = isUsing;
        this.durationTime = durationTime;
        this.endAction = endAction;
    }
}
