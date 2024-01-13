[System.Serializable]
public class ItemSlotInfo
{
	public Item Item;
	public string Name;
	public int Stacks;

	public ItemSlotInfo(Item newItem, int newStacks)
	{
		Item = newItem;
		Stacks = newStacks;
	}
}
