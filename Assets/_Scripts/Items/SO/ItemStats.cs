using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Health,
    Durability,
	Power,
    MaxItemStacks,
    MaxUseDistance,
}

[System.Serializable]
public class StatInfo
{
    public Stat StatType;
    public int StatValue;
}

[CreateAssetMenu(fileName = "New ItemStats", menuName = "ItemsStats")]
public class ItemStats : ScriptableObject
{
	[SerializeField] private List<StatInfo> statInfos;
	[SerializeField] private List<StatInfo> instanceStatInfos;

	public Dictionary<Stat, int> Stats = new Dictionary<Stat, int>();
	public Dictionary<Stat, int> InstanceStats = new Dictionary<Stat, int>();

    public void Initialize()
    {
        //add stats from inspector to the accessable dictionary
        foreach (StatInfo statInfo in statInfos)
        {
            Stats.Add(statInfo.StatType, statInfo.StatValue);
        }
        //add instance stats from inspector to the accessable dictionary
        foreach (StatInfo instanceStatInfo in instanceStatInfos)
        {
            InstanceStats.Add(instanceStatInfo.StatType, instanceStatInfo.StatValue);
        }
    }

    public int GetStat(Stat stat)
    {
        if(Stats.TryGetValue(stat, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"No stat value found for {stat} on {this.name}");
            return 0;
        }
    }
}
