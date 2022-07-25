using System.Collections.Generic;
using UnityEngine;

// 채집 가능 오브젝트
public class CollectableObject : MonoBehaviour
{
    public ItemSO item; // 채집하면 주는 아이템
    public ItemSO needItem; // 채집하려면 필요한 아이템

    public ItemSO GetItem() // 채집
    {
        // 필요한 아이템이 없거나, 필요한 아이템을 들고 있다면 실행
        if (needItem == null || InventoryManager.Instance.CurrentItem() == needItem)
        {
            gameObject.SetActive(false);

            if (item.isBuildable) // 건설 가능한 오브젝트라면
            {
                // Key가 없으면 새로 만들고, 사용 가능을 false로 바꿈
                if (!BuildManager.Instance.isUsableBuilding.ContainsKey(item))
                {
                    BuildManager.Instance.isUsableBuilding.Add(item, false);
                }
                else
                {
                    BuildManager.Instance.isUsableBuilding[item] = false;
                }

                // Key가 없으면 새로 만듦
                if (!BuildManager.Instance.poolObjectDictionary.ContainsKey(item))
                {
                    BuildManager.Instance.poolObjectDictionary.Add(item, new Queue<GameObject>());
                }

                // Pool Dictionary에 집어 넣음
                BuildManager.Instance.poolObjectDictionary[item].Enqueue(gameObject);
            }

            return item;
        }

        return null;
    }
}
