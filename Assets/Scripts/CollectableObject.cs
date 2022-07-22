using UnityEngine;

// ä�� ���� ������Ʈ
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // ä���ϸ� �ִ� ������
    public ItemSO needItem; // ä���Ϸ��� �ʿ��� ������

    public ItemSO GetItem() // ä��
    {
        if (needItem == null || InventoryManager.Instance.CurrentItem() == needItem)
        {
            gameObject.SetActive(false);

            return item;
        }

        return null;
    }
}
