using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour // ���� Manager
{
    // ���� ������ ����Ʈ
    public List<CraftingItem> craftingItemList = new List<CraftingItem>();

    // �̱��� ����
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
        // �������� ������ �� �ִ��� ������ Ȯ��

        for (int i = 0; i < craftingItemList.Count; i++)
        {
            craftingItemList[i].isCrafting = true;

            // �ʿ��� ������
            for (int j = 0; j < craftingItemList[i].needItemTextList.Count; j++)
            {
                // �κ��丮�� ��� �ִ��� Ȯ��
                craftingItemList[i].currentItemCountList[j] = InventoryManager.Instance.GetItemDictionary(craftingItemList[i].needItemList[j]);
                // �ؽ�Ʈ �ٲ�
                craftingItemList[i].needItemTextList[j].text = $"{craftingItemList[i].currentItemCountList[j]} / {craftingItemList[i].needItemCountList[j]}";
                // ���� ������ �ִ� �������� �ʿ��� �����ۺ��� ���ٸ�
                if (craftingItemList[i].currentItemCountList[j] < craftingItemList[i].needItemCountList[j])
                {
                    // ���� �Ұ���
                    craftingItemList[i].isCrafting = false;
                }
                else if (craftingItemList[i].needBuilding != null) // �ʿ��� ���๰�� �ִٸ�
                {
                    // ���๰ Ȯ��
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
                // ������ �����ϸ� �ʷϻ�
                craftingItemList[i].buttonImage.color = Color.green;
            }
            else
            {
                // ������ �Ұ����ϸ� ������
                craftingItemList[i].buttonImage.color = Color.red;
            }
        }
    }

    // ������ ����
    public void CraftingItem(CraftingItem item)
    {
        if (item.isCrafting) // ������ �����ϸ�
        {
            // �ʿ��� ������ ���
            for (int i = 0; i < item.needItemList.Count; i++)
            {
                InventoryManager.Instance.UseItem(item.needItemList[i], item.needItemCountList[i]);
            }

            // ���۵� ������ �߰�
            InventoryManager.Instance.AddItem(item.craftingItem);

            // ������ ���� ������ ����
            ResetCraftingData();
        }
    }
}
