using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Image itemImage; // ������ �̹���

    // ������ ����
    public void SetItem(Sprite image)
    {
        itemImage.sprite = image;
        itemImage.gameObject.SetActive(image == null ? false : true);
    }
}
