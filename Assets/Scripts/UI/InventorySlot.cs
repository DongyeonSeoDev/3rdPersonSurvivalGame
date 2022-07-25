using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
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

        // ���� �κ��丮�� ����Ǿ� �ִٸ� ���� �κ��丮�� ������ ����
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

    // ������ ������ ����
    private void SetItemData(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO == null ? null : itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO != null);
    }

    // ���콺�� ��������
    public void OnPointerDown(PointerEventData eventData)
    {
        // Start�� �����͸� ����
        InventoryManager.Instance.moveStartInventorySlot = this;
    }

    // ���콺�� ������
    public void OnPointerUp(PointerEventData eventData)
    {
        // ������ �̵� ����
        InventoryManager.Instance.MoveItem();
    }

    // ���콺�� �����ȿ� ��������
    public void OnPointerEnter(PointerEventData eventData)
    {
        // End�� �����͸� ����
        InventoryManager.Instance.moveEndInventorySlot = this;
    }

    // ���콺�� ���� ������ ��������
    public void OnPointerExit(PointerEventData eventData)
    {
        // End�� �ִ� ���� �ڽ��̶�� End�� null�� �ٲ� 
        if (InventoryManager.Instance.moveEndInventorySlot == this)
        {
            InventoryManager.Instance.moveEndInventorySlot = null;
        }
    }
}
