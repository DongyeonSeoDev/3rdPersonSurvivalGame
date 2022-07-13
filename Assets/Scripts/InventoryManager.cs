using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // �κ��丮 �г�
    public Transform inventorySlotParent; // �κ��丮 ���� �θ�

    // �κ��丮 �ִϸ��̼�
    public Vector3 openInventorySize; // ���� �� ũ��
    public Vector3 closeInventorySize; // ���� �� ũ��
    public Vector2 openInventoryPosition; // ���� �� ��ġ
    public Vector2 closeInventoryPosition; // ���� �� ��ġ
    
    public float inventoryAnimationTime; // �κ��丮 �ִϸ��̼� �ð�

    private InventorySlot[] inventorySlots; // �κ��丮 ���� �迭

    private Tween scaleTween; // ũ�� ���� Ʈ��
    private Tween positionTween; // ��ġ ���� Ʈ��

    private bool isInventoryOpen; // �κ��丮�� �����ִ��� Ȯ��

    private void Start()
    {
        inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();
    }

    // TAB ��ư�� �������� �۵�
    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    // �κ��丮 ���
    public void ToggleInventory()
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

    // �κ��丮�� ���� �� �ִϸ��̼�
    private void InventoryOpenAnimation()
    {
        inventoryPanel.gameObject.SetActive(true);

        // ���� �� ����
        inventoryPanel.localScale = closeInventorySize;
        inventoryPanel.anchoredPosition = closeInventoryPosition;

        // �ִϸ��̼� �۵�
        scaleTween = inventoryPanel.DOScale(openInventorySize, inventoryAnimationTime);
        positionTween = inventoryPanel.DOAnchorPos(openInventorySize, inventoryAnimationTime).OnComplete(() =>
        {
            GameManager.Instance.ChangeInventoryState(true);
        });
    }

    // �κ��丮�� ���� �� �ִϸ��̼�
    private void InventoryCloseAnimation()
    {
        GameManager.Instance.ChangeInventoryState(false);

        // ���� �� ����
        inventoryPanel.localScale = openInventorySize;
        inventoryPanel.anchoredPosition = openInventoryPosition;

        // �ִϸ��̼� �۵�
        scaleTween = inventoryPanel.DOScale(closeInventorySize, inventoryAnimationTime);
        positionTween = inventoryPanel.DOAnchorPos(closeInventoryPosition, inventoryAnimationTime).OnComplete(() =>
        {
            inventoryPanel.gameObject.SetActive(false);
        });
    }
}
