using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUI : MonoBehaviour
{
    public RectTransform itemUI; // 아이템 UI
    public ItemUIObject startItemUIObject; // 시작 아이템 UI 오브젝트

    // 인벤토리 애니메이션
    public Vector3 openItemUISize; // 열릴 때 크기
    public Vector3 closeItemUISize; // 닫힐 때 크기
    public Vector2 openItemUIPosition; // 열릴 때 위치
    public Vector2 closeItemUIPosition; // 닫힐 때 위치

    public float itemUIAnimationTime; // 아이템 UI 애니메이션 시간

    private ItemUIObject currentItemUIObject;

    private Tween scaleTween; // 크기 조절 트윈
    private Tween positionTween; // 위치 조절 트윈

    public static bool isInventoryOpen; // 인벤토리가 열려있는지 확인

    private void Start()
    {
        currentItemUIObject = startItemUIObject;
    }

    // TAB 버튼을 눌렀을때 작동
    public void OnToggleItemUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleItemUI();
        }
    }

    // 아이템 UI 토글
    public void ToggleItemUI()
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

    public void ChangeUI(ItemUIObject uiObject)
    {
        if (uiObject != currentItemUIObject)
        {
            currentItemUIObject.ActiveFalse();
            uiObject.ActiveTrue();

            currentItemUIObject = uiObject;
        }
    }

    // 아이템 UI가 열릴 때 애니메이션
    private void InventoryOpenAnimation()
    {
        currentItemUIObject.ActiveTrue();
        itemUI.gameObject.SetActive(true);

        // 시작 값 적용
        itemUI.localScale = closeItemUISize;
        itemUI.anchoredPosition = closeItemUIPosition;

        // 애니메이션 작동
        scaleTween = itemUI.DOScale(openItemUISize, itemUIAnimationTime);
        positionTween = itemUI.DOAnchorPos(openItemUIPosition, itemUIAnimationTime).OnComplete(() =>
        {
            GameManager.Instance.ChangeItemUIState(true);
        });
    }

    // 아이템 UI가 닫힐 때 애니메이션
    private void InventoryCloseAnimation()
    {
        InventoryManager.Instance.CloseInventory();
        GameManager.Instance.ChangeItemUIState(false);

        // 시작 값 적용
        itemUI.localScale = openItemUISize;
        itemUI.anchoredPosition = openItemUIPosition;

        // 애니메이션 작동
        scaleTween = itemUI.DOScale(closeItemUISize, itemUIAnimationTime);
        positionTween = itemUI.DOAnchorPos(closeItemUIPosition, itemUIAnimationTime).OnComplete(() =>
        {
            itemUI.gameObject.SetActive(false);
            currentItemUIObject.ActiveFalse();
        });
    }
}
