using UnityEngine;

// ä�� ���� ������Ʈ
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // ä���ϸ� �ִ� ������

    public ItemSO GetItem() // ä��
    {
        gameObject.SetActive(false);

        return item;
    }
}
