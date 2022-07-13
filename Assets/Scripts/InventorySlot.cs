using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Image itemImage; // 아이템 이미지

    // 아이템 적용
    public void SetItem(Sprite image)
    {
        itemImage.sprite = image;
        itemImage.gameObject.SetActive(image == null ? false : true);
    }
}
