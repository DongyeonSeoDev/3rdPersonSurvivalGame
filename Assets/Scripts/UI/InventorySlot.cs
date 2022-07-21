using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage; // ������ �̹���
    public ItemSO itemSO; // ������ ������
    public InventorySlot mainInventorySlot; // ���� �κ��丮 ������ �ִٸ� ���� �۵��ǰ� ��

    // �������� ����ִ��� Ȯ��
    public bool IsItem()
    {
        return itemSO != null;
    }

    // ���Կ� ������ ����
    public void SetItem(ItemSO item)
    {
        SetItemData(item);

        if (mainInventorySlot != null)
        {
            mainInventorySlot.SetItemData(item);
        }
    }

    // Ŭ�� �̺�Ʈ �ý���
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.ClickInventorySlot(this);
    }

    private void SetItemData(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO == null ? null : itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO == null ? false : true);
    }
}
