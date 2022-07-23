public class CraftingObject : ItemUIObject
{
    public override void ActiveTrue()
    {
        CraftingManager.Instance.ResetCraftingData();

        base.ActiveTrue();
    }
}
