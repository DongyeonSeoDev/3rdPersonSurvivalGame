public class InventoryObject : ItemUIObject // �κ��丮 UI ������Ʈ
{
    public override void ActiveFalse()
    {
        // �������� �κ��丮 �ݱ� ����
        InventoryManager.Instance.CloseInventory();

        base.ActiveFalse();
    }
}
