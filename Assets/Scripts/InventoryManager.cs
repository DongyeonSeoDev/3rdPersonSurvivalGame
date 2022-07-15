using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // 인벤토리 패널
    public Transform inventorySlotParent; // 인벤토리 슬롯 부모
    public Transform selectUI; // 선택한 인벤토리를 표시하는 UI
    public Button useItemButton; // 아이템 사용 버튼
    public Button deleteItemButton; // 아이템 삭제 버튼

    // 인벤토리 애니메이션
    public Vector3 openInventorySize; // 열릴 때 크기
    public Vector3 closeInventorySize; // 닫힐 때 크기
    public Vector2 openInventoryPosition; // 열릴 때 위치
    public Vector2 closeInventoryPosition; // 닫힐 때 위치
    
    public float inventoryAnimationTime; // 인벤토리 애니메이션 시간

    private InventorySlot[] inventorySlots; // 인벤토리 슬롯 배열

    private UnityEvent currentUseItemEvent;
    private InventorySlot currentInventorySlot; // 현재 선택된 인벤토리 슬롯
    private Tween scaleTween; // 크기 조절 트윈
    private Tween positionTween; // 위치 조절 트윈

    private bool isInventoryOpen; // 인벤토리가 열려있는지 확인

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
        inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();
    }

    // TAB 버튼을 눌렀을때 작동
    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!inventorySlots[i].IsItem())
            {
                inventorySlots[i].SetItem(item);
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

    public void UseItemButton()
    {
        currentUseItemEvent.Invoke();

        DeleteItemButton();
    }

    public void DeleteItemButton()
    {
        currentInventorySlot.SetItem(null);
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

    private void ShowSelectUI(bool isUseable, bool isRemovable)
    {
        if (currentInventorySlot.itemSO.isUsable)
        {
            currentUseItemEvent = currentInventorySlot.itemSO.itemUseEvent;
        }

        selectUI.SetParent(currentInventorySlot.transform);
        selectUI.transform.position = currentInventorySlot.transform.position;

        selectUI.gameObject.SetActive(true);
        useItemButton.gameObject.SetActive(isUseable);
        deleteItemButton.gameObject.SetActive(isRemovable);
    }

    private void RemoveSelectUI()
    {
        currentInventorySlot = null;
        currentUseItemEvent = null;

        selectUI.gameObject.SetActive(false);
        useItemButton.gameObject.SetActive(false);
        deleteItemButton.gameObject.SetActive(false);
    }
}
