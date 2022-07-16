using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // �κ��丮 �г�
    public Transform inventorySlotParent; // �κ��丮 ���� �θ�
    public Transform selectUI; // ������ �κ��丮�� ǥ���ϴ� UI
    public GameObject useItemButton; // ������ ��� ��ư
    public GameObject deleteItemButton; // ������ ���� ��ư

    // �κ��丮 �ִϸ��̼�
    public Vector3 openInventorySize; // ���� �� ũ��
    public Vector3 closeInventorySize; // ���� �� ũ��
    public Vector2 openInventoryPosition; // ���� �� ��ġ
    public Vector2 closeInventoryPosition; // ���� �� ��ġ
    
    public float inventoryAnimationTime; // �κ��丮 �ִϸ��̼� �ð�

    private InventorySlot[] inventorySlots; // �κ��丮 ���� �迭

    private UnityEvent currentUseItemEvent;
    private InventorySlot currentInventorySlot; // ���� ���õ� �κ��丮 ����
    private Tween scaleTween; // ũ�� ���� Ʈ��
    private Tween positionTween; // ��ġ ���� Ʈ��

    private bool isInventoryOpen; // �κ��丮�� �����ִ��� Ȯ��

    // �̱��� ����
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

    // TAB ��ư�� �������� �۵�
    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    // �κ��丮�� ������ �߰�
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

    // �κ��丮 Ŭ��
    public void ClickInventorySlot(InventorySlot inventorySlot)
    {
        if (inventorySlot != currentInventorySlot)
        {
            // ���õ� UI ǥ��
            currentInventorySlot = inventorySlot;

            ShowSelectUI(inventorySlot.itemSO != null && inventorySlot.itemSO.isUsable, inventorySlot.itemSO != null);
        }
        else
        {
            // ���õ� UI ǥ�� ����
            RemoveSelectUI();
        }
    }

    // ������ ��� ��ư�� Ŭ�������� ����
    public void UseItemButton()
    {
        currentUseItemEvent.Invoke();

        DeleteItemButton();
    }

    // ������ ���� ��ư�� Ŭ�������� ����
    public void DeleteItemButton()
    {
        currentInventorySlot.SetItem(null);
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
        RemoveSelectUI();
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

    // ���õ� ������ ǥ�� UI�� �����ֱ�
    private void ShowSelectUI(bool isUseable, bool isRemovable)
    {
        if (currentInventorySlot.itemSO != null && currentInventorySlot.itemSO.isUsable)
        {
            currentUseItemEvent = currentInventorySlot.itemSO.itemUseEvent;
        }

        selectUI.SetParent(currentInventorySlot.transform);
        selectUI.transform.position = currentInventorySlot.transform.position;

        selectUI.gameObject.SetActive(true);
        useItemButton.SetActive(isUseable);
        deleteItemButton.SetActive(isRemovable);
    }

    // ���õ� ������ ǥ�� UI�� �����
    private void RemoveSelectUI()
    {
        currentInventorySlot = null;
        currentUseItemEvent = null;

        selectUI.gameObject.SetActive(false);
        useItemButton.SetActive(false);
        deleteItemButton.SetActive(false);
    }
}
