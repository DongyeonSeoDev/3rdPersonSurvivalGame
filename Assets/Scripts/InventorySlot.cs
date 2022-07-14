using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // ������ �̹���

    private ItemSO itemSO; // ������ ������

    // �������� ����ִ��� Ȯ��
    public bool IsItem()
    {
        return itemSO != null;
    }

    // ���Կ� ������ ����
    public void SetItem(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO == null ? false : true);
    }
}
