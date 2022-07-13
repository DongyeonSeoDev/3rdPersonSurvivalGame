using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // 인벤토리 패널
    public Transform inventorySlotParent; // 인벤토리 슬롯 부모

    // 인벤토리 애니메이션
    public Vector3 openInventorySize; // 열릴 때 크기
    public Vector3 closeInventorySize; // 닫힐 때 크기
    public Vector2 openInventoryPosition; // 열릴 때 위치
    public Vector2 closeInventoryPosition; // 닫힐 때 위치
    
    public float inventoryAnimationTime; // 인벤토리 애니메이션 시간

    private InventorySlot[] inventorySlots; // 인벤토리 슬롯 배열

    private Tween scaleTween; // 크기 조절 트윈
    private Tween positionTween; // 위치 조절 트윈

    private bool isInventoryOpen; // 인벤토리가 열려있는지 확인

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
}
