using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage; // 아이템 이미지
    public ItemSO itemSO; // 아이템 데이터
    public InventorySlot mainInventorySlot; // 메인 인벤토리 슬롯이 있다면 같이 작동되게 함

    // 아이템이 들어있는지 확인
    public bool IsItem()
    {
        return itemSO != null;
    }

    // 슬롯에 아이템 적용
    public void SetItem(ItemSO item)
    {
        SetItemData(item);

        if (mainInventorySlot != null)
        {
            mainInventorySlot.SetItemData(item);
        }
    }

    // 클릭 이벤트 시스템
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.ClickInventorySlot(this);
    }

    private void SetItemData(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO == null ? null : itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO != null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryManager.Instance.moveStartInventorySlot = this;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InventoryManager.Instance.MoveItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.moveEndInventorySlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryManager.Instance.moveEndInventorySlot == this)
        {
            InventoryManager.Instance.moveEndInventorySlot = null;
        }
    }
}
