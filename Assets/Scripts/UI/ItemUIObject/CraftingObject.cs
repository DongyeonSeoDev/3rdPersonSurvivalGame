public class CraftingObject : ItemUIObject // ���� UI ������Ʈ
{
    public override void ActiveTrue()
    {
        // �������� ���� ������ ����
        CraftingManager.Instance.ResetCraftingData();

        base.ActiveTrue();
    }
}
