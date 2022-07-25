using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour // 제작 Manager
{
    // 제작 아이템 리스트
    public List<CraftingItem> craftingItemList = new List<CraftingItem>();

    // 싱글톤 패턴
    private static CraftingManager instance;
    public static CraftingManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);

            return;
        }

        instance = this;
    }

    public void ResetCraftingData()
    {
        // 아이템을 제작할 수 있는지 없는지 확인

        for (int i = 0; i < craftingItemList.Count; i++)
        {
            craftingItemList[i].isCrafting = true;

            // 필요한 아이템
            for (int j = 0; j < craftingItemList[i].needItemTextList.Count; j++)
            {
                // 인벤토리에 몇개가 있는지 확인
                craftingItemList[i].currentItemCountList[j] = InventoryManager.Instance.GetItemDictionary(craftingItemList[i].needItemList[j]);
                // 텍스트 바꿈
                craftingItemList[i].needItemTextList[j].text = $"{craftingItemList[i].currentItemCountList[j]} / {craftingItemList[i].needItemCountList[j]}";
                // 현재 가지고 있는 아이템이 필요한 아이템보다 적다면
                if (craftingItemList[i].currentItemCountList[j] < craftingItemList[i].needItemCountList[j])
                {
                    // 제작 불가능
                    craftingItemList[i].isCrafting = false;
                }
                else if (craftingItemList[i].needBuilding != null) // 필요한 건축물이 있다면
                {
                    // 건축물 확인
                    if (BuildManager.Instance.isUsableBuilding.TryGetValue(craftingItemList[i].needBuilding, out bool value))
                    {
                        craftingItemList[i].isCrafting = value;
                    }
                    else
                    {
                        craftingItemList[i].isCrafting = false;
                    }
                }
            }

            if (craftingItemList[i].isCrafting)
            {
                // 제작이 가능하면 초록색
                craftingItemList[i].buttonImage.color = Color.green;
            }
            else
            {
                // 제작이 불가능하면 빨간색
                craftingItemList[i].buttonImage.color = Color.red;
            }
        }
    }

    // 아이템 제작
    public void CraftingItem(CraftingItem item)
    {
        if (item.isCrafting) // 제작이 가능하면
        {
            // 필요한 아이템 사용
            for (int i = 0; i < item.needItemList.Count; i++)
            {
                InventoryManager.Instance.UseItem(item.needItemList[i], item.needItemCountList[i]);
            }

            // 제작된 아이템 추가
            InventoryManager.Instance.AddItem(item.craftingItem);

            // 아이템 제작 데이터 리셋
            ResetCraftingData();
        }
    }
}
