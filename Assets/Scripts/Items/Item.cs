using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public virtual string GetName()
    {
        return "NoName";
    }

    public virtual int MaxStacks()
    {
        return 20;
    }

    public virtual Sprite GetItemImage()
    {
        return Resources.Load<Sprite>("UI/ItemIcons/CrowBarItemIcon");
    }
}
