using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUI : MonoBehaviour
{
    public RectTransform itemUI; // ������ UI
    public ItemUIObject startItemUIObject; // ���� ������ UI ������Ʈ

    // �κ��丮 �ִϸ��̼�
    public Vector3 openItemUISize; // ���� �� ũ��
    public Vector3 closeItemUISize; // ���� �� ũ��
    public Vector2 openItemUIPosition; // ���� �� ��ġ
    public Vector2 closeItemUIPosition; // ���� �� ��ġ

    public float itemUIAnimationTime; // ������ UI �ִϸ��̼� �ð�

    private ItemUIObject currentItemUIObject;

    private Tween scaleTween; // ũ�� ���� Ʈ��
    private Tween positionTween; // ��ġ ���� Ʈ��

    public static bool isInventoryOpen; // �κ��丮�� �����ִ��� Ȯ��

    private void Start()
    {
        currentItemUIObject = startItemUIObject;
    }

    // TAB ��ư�� �������� �۵�
    public void OnToggleItemUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleItemUI();
        }
    }

    // ������ UI ���
    public void ToggleItemUI()
    {
        isInventoryOpen = !isInventoryOpen;

        // ���� �۵��ǰ� �ִ� �ִϸ��̼��� �ִٸ� ����
        if (scaleTween.IsActive())
        {
            scaleTween.Complete();
        }

        if (positionTween.IsActive())
        {
            positionTween.Complete();
        }

        // �ִϸ��̼� ����
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

    // ������ UI�� ���� �� �ִϸ��̼�
    private void InventoryOpenAnimation()
    {
        currentItemUIObject.ActiveTrue();
        itemUI.gameObject.SetActive(true);

        // ���� �� ����
        itemUI.localScale = closeItemUISize;
        itemUI.anchoredPosition = closeItemUIPosition;

        // �ִϸ��̼� �۵�
        scaleTween = itemUI.DOScale(openItemUISize, itemUIAnimationTime);
        positionTween = itemUI.DOAnchorPos(openItemUIPosition, itemUIAnimationTime).OnComplete(() =>
        {
            GameManager.Instance.ChangeItemUIState(true);
        });
    }

    // ������ UI�� ���� �� �ִϸ��̼�
    private void InventoryCloseAnimation()
    {
        InventoryManager.Instance.CloseInventory();
        GameManager.Instance.ChangeItemUIState(false);

        // ���� �� ����
        itemUI.localScale = openItemUISize;
        itemUI.anchoredPosition = openItemUIPosition;

        // �ִϸ��̼� �۵�
        scaleTween = itemUI.DOScale(closeItemUISize, itemUIAnimationTime);
        positionTween = itemUI.DOAnchorPos(closeItemUIPosition, itemUIAnimationTime).OnComplete(() =>
        {
            itemUI.gameObject.SetActive(false);
            currentItemUIObject.ActiveFalse();
        });
    }
}
