using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public List<CraftingItem> craftingItemList = new List<CraftingItem>();

    // ΩÃ±€≈Ê ∆–≈œ
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
        for (int i = 0; i < craftingItemList.Count; i++)
        {
            craftingItemList[i].isCrafting = true;

            for (int j = 0; j < craftingItemList[i].needItemTextList.Count; j++)
            {
                craftingItemList[i].currentItemCountList[j] = InventoryManager.Instance.GetItemDictionary(craftingItemList[i].needItemList[j]);

                craftingItemList[i].needItemTextList[j].text = $"{craftingItemList[i].currentItemCountList[j]} / {craftingItemList[i].needItemCountList[j]}";

                if (craftingItemList[i].currentItemCountList[j] < craftingItemList[i].needItemCountList[j])
                {
                    craftingItemList[i].isCrafting = false;
                }
            }

            if (craftingItemList[i].isCrafting)
            {
                craftingItemList[i].buttonImage.color = Color.green;
            }
            else
            {
                craftingItemList[i].buttonImage.color = Color.red;
            }
        }
    }

    public void CraftingItem(CraftingItem item)
    {
        if (item.isCrafting)
        {
            for (int i = 0; i < item.needItemList.Count; i++)
            {
                InventoryManager.Instance.UseItem(item.needItemList[i], item.needItemCountList[i]);
            }

            InventoryManager.Instance.AddItem(item.craftingItem);

            ResetCraftingData();
        }
    }
}
