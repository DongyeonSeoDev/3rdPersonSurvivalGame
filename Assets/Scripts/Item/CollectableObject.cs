using System.Collections.Generic;
using UnityEngine;

// ä�� ���� ������Ʈ
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // ä���ϸ� �ִ� ������
    public ItemSO needItem; // ä���Ϸ��� �ʿ��� ������

    public ItemSO GetItem() // ä��
    {
        // �ʿ��� �������� ���ų�, �ʿ��� �������� ��� �ִٸ� ����
        if (needItem == null || InventoryManager.Instance.CurrentItem() == needItem)
        {
            gameObject.SetActive(false);

            if (item.isBuildable) // �Ǽ� ������ ������Ʈ���
            {
                // Key�� ������ ���� �����, ��� ������ false�� �ٲ�
                if (!BuildManager.Instance.isUsableBuilding.ContainsKey(item))
                {
                    BuildManager.Instance.isUsableBuilding.Add(item, false);
                }
                else
                {
                    BuildManager.Instance.isUsableBuilding[item] = false;
                }

                // Key�� ������ ���� ����
                if (!BuildManager.Instance.poolObjectDictionary.ContainsKey(item))
                {
                    BuildManager.Instance.poolObjectDictionary.Add(item, new Queue<GameObject>());
                }

                // Pool Dictionary�� ���� ����
                BuildManager.Instance.poolObjectDictionary[item].Enqueue(gameObject);
            }

            return item;
        }

        return null;
    }
}
