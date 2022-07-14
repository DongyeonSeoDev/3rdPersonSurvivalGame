using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지

    private ItemSO itemSO; // 아이템 데이터

    // 아이템이 들어있는지 확인
    public bool IsItem()
    {
        return itemSO != null;
    }

    // 슬롯에 아이템 적용
    public void SetItem(ItemSO item)
    {
        itemSO = item;
        itemImage.sprite = itemSO.itemSprite;
        itemImage.gameObject.SetActive(itemSO == null ? false : true);
    }
}
