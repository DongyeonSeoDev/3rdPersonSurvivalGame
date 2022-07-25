public class InventoryObject : ItemUIObject // 인벤토리 UI 오브젝트
{
    public override void ActiveFalse()
    {
        // 꺼졌을때 인벤토리 닫기 실행
        InventoryManager.Instance.CloseInventory();

        base.ActiveFalse();
    }
}
