using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image[] itemImage; // ������ �̹���
    public ItemSO itemSO; // ������ ������

    // �������� ����ִ��� Ȯ��
    public bool IsItem()
    {
        return itemSO != null;
    }

    // ���Կ� ������ ����
    public void SetItem(ItemSO item)
    {
        itemSO = item;

        for (int i = 0; i < itemImage.Length; i++)
        {
            itemImage[i].sprite = itemSO == null ? null : itemSO.itemSprite;
            itemImage[i].gameObject.SetActive(itemSO == null ? false : true);
        }
    }

    // Ŭ�� �̺�Ʈ �ý���
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.ClickInventorySlot(this);
    }
}
