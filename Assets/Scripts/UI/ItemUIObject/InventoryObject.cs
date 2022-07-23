public class InventoryObject : ItemUIObject
{
    public override void ActiveFalse()
    {
        InventoryManager.Instance.CloseInventory();

        base.ActiveFalse();
    }
}
