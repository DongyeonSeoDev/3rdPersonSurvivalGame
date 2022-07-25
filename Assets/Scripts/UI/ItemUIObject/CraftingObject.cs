public class CraftingObject : ItemUIObject // 제작 UI 오브젝트
{
    public override void ActiveTrue()
    {
        // 켜졌을때 제작 데이터 리셋
        CraftingManager.Instance.ResetCraftingData();

        base.ActiveTrue();
    }
}
