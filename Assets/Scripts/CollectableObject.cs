using UnityEngine;

// 채집 가능 오브젝트
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // 채집하면 주는 아이템

    public ItemSO GetItem() // 채집
    {
        gameObject.SetActive(false);

        return item;
    }
}
