using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public Transform[] inventorySlotParents; // 인벤토리 슬롯 부모
    public Transform mainInventorySlotParent; // 메인 인벤토리 슬롯 부모
    public Transform selectUI; // 선택한 인벤토리를 표시하는 UI
    public Transform mainSelectUI; // 선택한 메인 인벤토리 슬롯을 표시하는 UI
    public GameObject useItemButton; // 아이템 사용 버튼
    public GameObject deleteItemButton; // 아이템 삭제 버튼

    [HideInInspector] public InventorySlot moveStartInventorySlot;
    [HideInInspector] public InventorySlot moveEndInventorySlot;

    private readonly Dictionary<ItemSO, int> itemCountDictionary = new Dictionary<ItemSO, int>();
    private readonly List<InventorySlot> inventorySlots = new List<InventorySlot>(); // 인벤토리 슬롯 리스트
    private readonly List<InventorySlot> mainInventorySlots = new List<InventorySlot>(); // 메인 인벤토리 슬롯 리스트

    private UnityEvent currentUseItemEvent;
    private UnityEvent currentMainInventoryUseItemEvent;
    private InventorySlot currentInventorySlot; // 현재 선택된 인벤토리 슬롯
    private InventorySlot currentMainInventorySlot; // 현재 메인 인벤토리에서 선택된 슬롯

    // 싱글톤 패턴
    private static InventoryManager instance;
    public static InventoryManager Instance
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

    private void Start()
    {
        for (int i = 0; i < inventorySlotParents.Length; i++)
        {
            for (int j = 0; j < inventorySlotParents[i].childCount; j++)
            {
                inventorySlots.Add(inventorySlotParents[i].GetChild(j).GetComponent<InventorySlot>());
            }
        }

        for (int i = 0; i < mainInventorySlotParent.childCount; i++)
        {
            mainInventorySlots.Add(mainInventorySlotParent.GetChild(i).GetComponent<InventorySlot>());
        }

        currentMainInventorySlot = mainInventorySlots[0];
    }

    // 키보드 1~8 키를 눌렀을때 작동
    public void OnInventorySlotChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetMainInventorySelectUI(int.Parse(context.control.name));
        }
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].IsItem())
            {
                inventorySlots[i].SetItem(item);

                CurrentMainInventorySlotCheck(inventorySlots[i]);
                AddItemDictionary(item, 1);

                break;
            }
        }
    }

    public void UseItem(ItemSO item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < inventorySlots.Count; j++)
            {
                if (inventorySlots[j].itemSO == item)
                {
                    inventorySlots[j].SetItem(null);

                    CurrentMainInventorySlotCheck(inventorySlots[j]);
                    AddItemDictionary(item, -1);

                    break;
                }
            }
        }
    }

    // 인벤토리 클릭
    public void ClickInventorySlot(InventorySlot inventorySlot)
    {
        if (inventorySlot != currentInventorySlot)
        {
            // 선택된 UI 표시
            currentInventorySlot = inventorySlot;

            ShowSelectUI(inventorySlot.itemSO != null && inventorySlot.itemSO.isUsable, inventorySlot.itemSO != null);
        }
        else
        {
            // 선택된 UI 표시 제거
            RemoveSelectUI();
        }
    }

    // 아이템 사용 버튼을 클릭했을때 실행
    public void UseItemButton()
    {
        currentUseItemEvent.Invoke();

        DeleteItemButton();
    }

    // 아이템 삭제 버튼을 클릭했을때 실행
    public void DeleteItemButton()
    {
        AddItemDictionary(currentInventorySlot.itemSO, -1);
        currentInventorySlot.SetItem(null);

        CurrentMainInventorySlotCheck(currentInventorySlot);
        RemoveSelectUI();
    }

    public bool UseMainItem()
    {
        if (currentMainInventoryUseItemEvent != null)
        {
            currentMainInventoryUseItemEvent.Invoke();

            currentMainInventoryUseItemEvent = null;
            AddItemDictionary(currentMainInventorySlot.itemSO, -1);
            currentMainInventorySlot.SetItem(null);

            return true;
        }
        else if (currentMainInventorySlot.itemSO != null && currentMainInventorySlot.itemSO.isBuildable)
        {
            BuildManager.Instance.Build();
            AddItemDictionary(currentMainInventorySlot.itemSO, -1);
            currentMainInventorySlot.SetItem(null);

            return true;
        }

        return false;
    }

    public void MoveItem()
    {
        if (moveStartInventorySlot != null && moveEndInventorySlot != null && moveStartInventorySlot != moveEndInventorySlot && moveStartInventorySlot.itemSO != null)
        {
            ItemSO temp = moveStartInventorySlot.itemSO;

            moveStartInventorySlot.SetItem(moveEndInventorySlot.itemSO);
            moveEndInventorySlot.SetItem(temp);

            ClickInventorySlot(moveEndInventorySlot);
            CurrentMainInventorySlotCheck(moveStartInventorySlot);
            CurrentMainInventorySlotCheck(moveEndInventorySlot);
        }

        MoveEnd();
    }

    public ItemSO CurrentItem()
    {
        return currentMainInventorySlot.itemSO;
    }

    public void CloseInventory()
    {
        RemoveSelectUI();
        MoveEnd();
    }

    public void AddItemDictionary(ItemSO item, int addCount)
    {
        if (!itemCountDictionary.ContainsKey(item))
        {
            itemCountDictionary.Add(item, addCount);

            return;
        }

        itemCountDictionary[item] += addCount;
    }

    public int GetItemDictionary(ItemSO item)
    {
        if (!itemCountDictionary.ContainsKey(item))
        {
            return 0;
        }

        return itemCountDictionary[item];
    }

    private void MoveEnd()
    {
        moveStartInventorySlot = null;
        moveEndInventorySlot = null;
    }

    private void SetMainInventorySelectUI(int inventorySlotNumber)
    {
        currentMainInventorySlot = mainInventorySlots[inventorySlotNumber - 1];

        mainSelectUI.SetParent(currentMainInventorySlot.transform, false);

        SetMainInventoryEvent();
    }

    private void CurrentMainInventorySlotCheck(InventorySlot inventorySlot)
    {
        if (inventorySlot.mainInventorySlot != null && currentMainInventorySlot == inventorySlot.mainInventorySlot)
        {
            SetMainInventoryEvent();
        }
    }

    private void SetMainInventoryEvent()
    {
        if (currentMainInventorySlot.itemSO != null)
        {
            if (currentMainInventorySlot.itemSO.isUsable)
            {
                BuildManager.Instance.SetBuildObject(null);
                currentMainInventoryUseItemEvent = currentMainInventorySlot.itemSO.itemUseEvent;

                return;
            }
            else if (currentMainInventorySlot.itemSO.isBuildable)
            {
                BuildManager.Instance.SetBuildObject(currentMainInventorySlot.itemSO);
                currentMainInventoryUseItemEvent = null;

                return;
            }
        }

        BuildManager.Instance.SetBuildObject(null);
        currentMainInventoryUseItemEvent = null;
    }

    // 선택된 아이템 표시 UI를 보여주기
    private void ShowSelectUI(bool isUseable, bool isRemovable)
    {
        if (currentInventorySlot.itemSO != null && currentInventorySlot.itemSO.isUsable)
        {
            currentUseItemEvent = currentInventorySlot.itemSO.itemUseEvent;
        }

        selectUI.SetParent(currentInventorySlot.transform, false);

        selectUI.gameObject.SetActive(true);
        useItemButton.SetActive(isUseable);
        deleteItemButton.SetActive(isRemovable);
    }

    // 선택된 아이템 표시 UI를 지우기
    private void RemoveSelectUI()
    {
        currentUseItemEvent = null;
        currentInventorySlot = null;
        
        selectUI.gameObject.SetActive(false);
        useItemButton.SetActive(false);
        deleteItemButton.SetActive(false);
    }
}
