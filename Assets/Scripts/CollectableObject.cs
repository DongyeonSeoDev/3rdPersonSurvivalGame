using UnityEngine;

// 채집 가능 오브젝트
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // 채집하면 주는 아이템
    public ItemSO needItem; // 채집하려면 필요한 아이템

    public ItemSO GetItem() // 채집
    {
        if (needItem == null || InventoryManager.Instance.CurrentItem() == needItem)
        {
            gameObject.SetActive(false);

            return item;
        }

        return null;
    }
}
