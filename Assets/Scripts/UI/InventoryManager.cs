using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour // 인벤토리 Manager
{
    public Transform[] inventorySlotParents; // 인벤토리 슬롯 부모
    public Transform mainInventorySlotParent; // 메인 인벤토리 슬롯 부모
    public Transform selectUI; // 선택한 인벤토리를 표시하는 UI
    public Transform mainSelectUI; // 선택한 메인 인벤토리 슬롯을 표시하는 UI
    public GameObject useItemButton; // 아이템 사용 버튼
    public GameObject deleteItemButton; // 아이템 삭제 버튼

    [HideInInspector] public InventorySlot moveStartInventorySlot; // 움직임 시작 슬롯
    [HideInInspector] public InventorySlot moveEndInventorySlot; // 움직임 종료 슬롯

    private readonly Dictionary<ItemSO, int> itemCountDictionary = new Dictionary<ItemSO, int>(); // 아이템 개수 Dictionary
    private readonly List<InventorySlot> inventorySlots = new List<InventorySlot>(); // 인벤토리 슬롯 리스트
    private readonly List<InventorySlot> mainInventorySlots = new List<InventorySlot>(); // 메인 인벤토리 슬롯 리스트

    private UnityEvent currentUseItemEvent; // 아이템 사용 이벤트
    private UnityEvent currentMainInventoryUseItemEvent; // 메인 인벤토리 아이템 사용 이벤트
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
        // 인벤토리 슬롯 추가
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
            if (!inventorySlots[i].IsItem()) // 인벤토리가 비어있는지 확인
            {
                inventorySlots[i].SetItem(item); // 아이템 설정

                CurrentMainInventorySlotCheck(inventorySlots[i]); // 메인 인벤토리인지 확인
                AddItemDictionary(item, 1); // 아이템 추가

                break;
            }
        }
    }

    // 아이템 사용
    public void UseItem(ItemSO item, int count)
    {
        for (int i = 0; i < count; i++) // 사용한 아이템 개수만큼 실행
        {
            for (int j = 0; j < inventorySlots.Count; j++)
            {
                if (inventorySlots[j].itemSO == item) // 아이템을 찾았다면
                {
                    // 아이템 제거
                    inventorySlots[j].SetItem(null); // 아이템 제거
                    // 메인 인벤토리 확인
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
        if (inventorySlot != currentInventorySlot) // 이미 선택된 인벤토리인지 확인
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
        // 아이템 이벤트 실행
        currentUseItemEvent.Invoke();

        DeleteItemButton();
    }

    // 아이템 삭제 버튼을 클릭했을때 실행
    public void DeleteItemButton()
    {
        // 아이템 제거
        AddItemDictionary(currentInventorySlot.itemSO, -1);
        currentInventorySlot.SetItem(null);

        CurrentMainInventorySlotCheck(currentInventorySlot); // 메인 인벤토리 확인
        RemoveSelectUI(); // 선택 UI 제거
    }

    // 메인 아이템 사용
    public bool UseMainItem()
    {
        if (currentMainInventoryUseItemEvent != null)
        {
            // 아이템 사용 이벤트 실행
            currentMainInventoryUseItemEvent.Invoke();

            // 이벤트와 아이템 제거
            currentMainInventoryUseItemEvent = null;
            AddItemDictionary(currentMainInventorySlot.itemSO, -1);
            currentMainInventorySlot.SetItem(null);

            return true;
        }
        else if (currentMainInventorySlot.itemSO != null && currentMainInventorySlot.itemSO.isBuildable)
        {
            // 건축 아이템 이면 건축 실행
            BuildManager.Instance.Build();

            // 아이템 제거
            AddItemDictionary(currentMainInventorySlot.itemSO, -1);
            currentMainInventorySlot.SetItem(null);

            return true;
        }

        return false;
    }

    public void MoveItem() // 아이템 이동
    {
        // Start와 End가 Null이 아니고, Start와 End가 같지 않고, Start에 아이템이 있다면 실행
        if (moveStartInventorySlot != null && moveEndInventorySlot != null && moveStartInventorySlot != moveEndInventorySlot && moveStartInventorySlot.itemSO != null)
        {
            ItemSO temp = moveStartInventorySlot.itemSO;

            // Start와 End의 아이템 교환
            moveStartInventorySlot.SetItem(moveEndInventorySlot.itemSO);
            moveEndInventorySlot.SetItem(temp);

            ClickInventorySlot(moveEndInventorySlot); // 인벤토리 슬롯 확인

            // 메인 인벤토리 슬롯 확인
            CurrentMainInventorySlotCheck(moveStartInventorySlot); 
            CurrentMainInventorySlotCheck(moveEndInventorySlot);
        }

        MoveEnd(); // 움직임 종료
    }

    public ItemSO CurrentItem() // 현재 들고있는 아이템
    {
        return currentMainInventorySlot.itemSO;
    }

    public void CloseInventory() // 인벤토리를 닫았을때
    {
        RemoveSelectUI(); // 선택 UI 제거
        MoveEnd(); // 움직임 종료
    }

    public void AddItemDictionary(ItemSO item, int addCount) // 아이템 Dictionary에 추가
    {
        if (!itemCountDictionary.ContainsKey(item))
        {
            // 키가 없다면 추가
            itemCountDictionary.Add(item, addCount);

            return;
        }

        itemCountDictionary[item] += addCount; // 아이템 개수 설정
    }

    public int GetItemDictionary(ItemSO item) // 아이템 개수 가져오기
    {
        if (!itemCountDictionary.ContainsKey(item))
        {
            // 키가 없다면 0
            return 0;
        }

        return itemCountDictionary[item];
    }

    private void MoveEnd()
    {
        // 아이템 이동 종료시 Start와 End 값을 null로 바꿈
        moveStartInventorySlot = null;
        moveEndInventorySlot = null;
    }

    private void SetMainInventorySelectUI(int inventorySlotNumber)
    {
        // 현재 메인 인벤토리 슬롯을 선택한 번호의 인벤토리 슬롯으로 변경
        currentMainInventorySlot = mainInventorySlots[inventorySlotNumber - 1];

        mainSelectUI.SetParent(currentMainInventorySlot.transform, false);

        SetMainInventoryEvent(); // 이벤트 설정
    }

    private void CurrentMainInventorySlotCheck(InventorySlot inventorySlot)
    {
        // 메인 인벤토리 슬롯의 아이템이 바뀌었다면 메인 인벤토리 이벤트 변경
        if (inventorySlot.mainInventorySlot != null && currentMainInventorySlot == inventorySlot.mainInventorySlot)
        {
            SetMainInventoryEvent();
        }
    }

    private void SetMainInventoryEvent() // 메인 인벤토리 이벤트 설정
    {
        if (currentMainInventorySlot.itemSO != null) // 아이템이 있다면
        {
            if (currentMainInventorySlot.itemSO.isUsable) // 사용가능 아이템
            {
                BuildManager.Instance.SetBuildObject(null); // 건설 오브젝트를 null로 변경
                currentMainInventoryUseItemEvent = currentMainInventorySlot.itemSO.itemUseEvent; // 이벤트에 아이템 사용 이벤트를 넣음

                return;
            }
            else if (currentMainInventorySlot.itemSO.isBuildable) // 건설가능 아이템
            {
                BuildManager.Instance.SetBuildObject(currentMainInventorySlot.itemSO); // 건설 오브젝트에 넣음
                currentMainInventoryUseItemEvent = null; // 이벤트를 null로 변경

                return;
            }
        }

        // 둘다 null로 변경
        BuildManager.Instance.SetBuildObject(null);
        currentMainInventoryUseItemEvent = null;
    }

    // 선택된 아이템 표시 UI를 보여주기
    private void ShowSelectUI(bool isUseable, bool isRemovable)
    {
        if (currentInventorySlot.itemSO != null && currentInventorySlot.itemSO.isUsable)
        {
            // 아이템 사용이 가능하다면 이벤트에 넣음
            currentUseItemEvent = currentInventorySlot.itemSO.itemUseEvent;
        }

        // 부모 설정
        selectUI.SetParent(currentInventorySlot.transform, false);

        // UI와 버튼 활성화
        selectUI.gameObject.SetActive(true);
        useItemButton.SetActive(isUseable);
        deleteItemButton.SetActive(isRemovable);
    }

    // 선택된 아이템 표시 UI를 지우기
    private void RemoveSelectUI()
    {
        // 이벤트와 아이템 슬롯 null로 변경
        currentUseItemEvent = null;
        currentInventorySlot = null;
        
        // UI와 버튼 비활성화
        selectUI.gameObject.SetActive(false);
        useItemButton.SetActive(false);
        deleteItemButton.SetActive(false);
    }
}
