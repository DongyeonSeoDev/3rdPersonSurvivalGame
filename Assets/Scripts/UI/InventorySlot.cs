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

        // 메인 인벤토리가 연결되어 있다면 메인 인벤토리도 아이템 적용
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

    // 아이템 데이터 적용
    private void SetItemData(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO == null ? null : itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO != null);
    }

    // 마우스를 눌렀을때
    public void OnPointerDown(PointerEventData eventData)
    {
        // Start에 데이터를 넣음
        InventoryManager.Instance.moveStartInventorySlot = this;
    }

    // 마우스를 땠을때
    public void OnPointerUp(PointerEventData eventData)
    {
        // 아이템 이동 실행
        InventoryManager.Instance.MoveItem();
    }

    // 마우스가 범위안에 들어왔을때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // End에 데이터를 넣음
        InventoryManager.Instance.moveEndInventorySlot = this;
    }

    // 마우스가 범위 밖으로 나갔을때
    public void OnPointerExit(PointerEventData eventData)
    {
        // End에 있는 값이 자신이라면 End를 null로 바꿈 
        if (InventoryManager.Instance.moveEndInventorySlot == this)
        {
            InventoryManager.Instance.moveEndInventorySlot = null;
        }
    }
}
