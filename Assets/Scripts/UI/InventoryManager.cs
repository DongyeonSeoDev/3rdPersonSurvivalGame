using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // 인벤토리 패널
    public Transform[] inventorySlotParents; // 인벤토리 슬롯 부모
    public Transform mainInventorySlotParent; // 메인 인벤토리 슬롯 부모
    public Transform selectUI; // 선택한 인벤토리를 표시하는 UI
    public Transform mainSelectUI; // 선택한 메인 인벤토리 슬롯을 표시하는 UI
    public GameObject useItemButton; // 아이템 사용 버튼
    public GameObject deleteItemButton; // 아이템 삭제 버튼

    // 인벤토리 애니메이션
    public Vector3 openInventorySize; // 열릴 때 크기
    public Vector3 closeInventorySize; // 닫힐 때 크기
    public Vector2 openInventoryPosition; // 열릴 때 위치
    public Vector2 closeInventoryPosition; // 닫힐 때 위치
    
    public float inventoryAnimationTime; // 인벤토리 애니메이션 시간

    [HideInInspector]
    public UnityEvent currentMainInventoryUseItemEvent;
    [HideInInspector]
    public bool isEvent;
    [HideInInspector]
    public bool isInventoryOpen; // 인벤토리가 열려있는지 확인

    private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // 인벤토리 슬롯 리스트
    private List<InventorySlot> mainInventorySlots = new List<InventorySlot>(); // 메인 인벤토리 슬롯 리스트

    private UnityEvent currentUseItemEvent;
    private InventorySlot currentInventorySlot; // 현재 선택된 인벤토리 슬롯
    private InventorySlot currentMainInventorySlot; // 현재 메인 인벤토리에서 선택된 슬롯
    private Tween scaleTween; // 크기 조절 트윈
    private Tween positionTween; // 위치 조절 트윈

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
    }

    // TAB 버튼을 눌렀을때 작동
    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
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

                if (currentMainInventorySlot == inventorySlots[i])
                {
                    SetMainInventoryEvent();
                }

                break;
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
        currentInventorySlot.SetItem(null);

        RemoveSelectUI();
    }

    // 인벤토리 토글
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        // 현재 작동되고 있는 애니메이션이 있다면 종료
        if (scaleTween.IsActive())
        {
            scaleTween.Complete();
        }

        if (positionTween.IsActive())
        {
            positionTween.Complete();
        }

        // 애니메이션 실행
        if (isInventoryOpen)
        {
            InventoryOpenAnimation();
        }
        else
        {
            InventoryCloseAnimation();
        }
    }

    // 인벤토리가 열릴 때 애니메이션
    private void InventoryOpenAnimation()
    {
        inventoryPanel.gameObject.SetActive(true);

        // 시작 값 적용
        inventoryPanel.localScale = closeInventorySize;
        inventoryPanel.anchoredPosition = closeInventoryPosition;

        // 애니메이션 작동
        scaleTween = inventoryPanel.DOScale(openInventorySize, inventoryAnimationTime);
        positionTween = inventoryPanel.DOAnchorPos(openInventorySize, inventoryAnimationTime).OnComplete(() =>
        {
            GameManager.Instance.ChangeInventoryState(true);
        });
    }

    // 인벤토리가 닫힐 때 애니메이션
    private void InventoryCloseAnimation()
    {
        RemoveSelectUI();
        GameManager.Instance.ChangeInventoryState(false);

        // 시작 값 적용
        inventoryPanel.localScale = openInventorySize;
        inventoryPanel.anchoredPosition = openInventoryPosition;

        // 애니메이션 작동
        scaleTween = inventoryPanel.DOScale(closeInventorySize, inventoryAnimationTime);
        positionTween = inventoryPanel.DOAnchorPos(closeInventoryPosition, inventoryAnimationTime).OnComplete(() =>
        {
            inventoryPanel.gameObject.SetActive(false);
        });
    }

    private void SetMainInventorySelectUI(int inventorySlotNumber)
    {
        currentMainInventorySlot = mainInventorySlots[inventorySlotNumber - 1];

        mainSelectUI.SetParent(currentMainInventorySlot.transform, false);

        SetMainInventoryEvent();
    }

    private void SetMainInventoryEvent()
    {
        if (currentMainInventorySlot.itemSO != null && currentMainInventorySlot.itemSO.isUsable)
        {
            currentMainInventoryUseItemEvent = currentMainInventorySlot.itemSO.itemUseEvent;
            isEvent = true;
        }
        else
        {
            currentMainInventoryUseItemEvent = null;
            isEvent = false;
        }
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
        currentInventorySlot = null;
        currentUseItemEvent = null;

        selectUI.gameObject.SetActive(false);
        useItemButton.SetActive(false);
        deleteItemButton.SetActive(false);
    }
}
