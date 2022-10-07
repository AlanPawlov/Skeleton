using System;

[Serializable]
public struct DropItemFromStackZone
{
    public DateTime LastDropTime;
    public float DropInterval;
    public float Radius;
    public int Reward;
    public RewardType RewardType;
}
