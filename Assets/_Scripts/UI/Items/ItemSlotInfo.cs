[System.Serializable]
public class ItemSlotInfo
{
	public Item Item;
	public string Name;
	public int Stacks;
	public float Durability;
	public float MaxDurability;

	public ItemSlotInfo(Item Item, int Stacks, float Durability, float MaxDurability)
	{
		this.Item = Item;
		this.Stacks = Stacks;
		this.Durability = Durability;
		this.MaxDurability = MaxDurability;
	}
}
