using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // �κ��丮 �г�
    public Transform[] inventorySlotParents; // �κ��丮 ���� �θ�
    public Transform mainInventorySlotParent; // ���� �κ��丮 ���� �θ�
    public Transform selectUI; // ������ �κ��丮�� ǥ���ϴ� UI
    public Transform mainSelectUI; // ������ ���� �κ��丮 ������ ǥ���ϴ� UI
    public GameObject useItemButton; // ������ ��� ��ư
    public GameObject deleteItemButton; // ������ ���� ��ư

    // �κ��丮 �ִϸ��̼�
    public Vector3 openInventorySize; // ���� �� ũ��
    public Vector3 closeInventorySize; // ���� �� ũ��
    public Vector2 openInventoryPosition; // ���� �� ��ġ
    public Vector2 closeInventoryPosition; // ���� �� ��ġ
    
    public float inventoryAnimationTime; // �κ��丮 �ִϸ��̼� �ð�

    [HideInInspector] public InventorySlot moveStartInventorySlot;
    [HideInInspector] public InventorySlot moveEndInventorySlot;
    [HideInInspector] public bool isInventoryOpen; // �κ��丮�� �����ִ��� Ȯ��

    private readonly List<InventorySlot> inventorySlots = new List<InventorySlot>(); // �κ��丮 ���� ����Ʈ
    private readonly List<InventorySlot> mainInventorySlots = new List<InventorySlot>(); // ���� �κ��丮 ���� ����Ʈ

    private UnityEvent currentUseItemEvent;
    private UnityEvent currentMainInventoryUseItemEvent;
    private InventorySlot currentInventorySlot; // ���� ���õ� �κ��丮 ����
    private InventorySlot currentMainInventorySlot; // ���� ���� �κ��丮���� ���õ� ����
    private Tween scaleTween; // ũ�� ���� Ʈ��
    private Tween positionTween; // ��ġ ���� Ʈ��

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

    // TAB ��ư�� �������� �۵�
    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    // Ű���� 1~8 Ű�� �������� �۵�
    public void OnInventorySlotChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetMainInventorySelectUI(int.Parse(context.control.name));
        }
    }

    // �κ��丮�� ������ �߰�
    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].IsItem())
            {
                inventorySlots[i].SetItem(item);

                CurrentMainInventorySlotCheck(inventorySlots[i]);

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

        RemoveSelectUI();
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

    public bool UseMainItem()
    {
        if (currentMainInventoryUseItemEvent != null)
        {
            currentMainInventoryUseItemEvent.Invoke();

            currentMainInventoryUseItemEvent = null;
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

    private void MoveEnd()
    {
        moveStartInventorySlot = null;
        moveEndInventorySlot = null;
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
        MoveEnd();
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
        if (currentMainInventorySlot.itemSO != null && currentMainInventorySlot.itemSO.isUsable)
        {
            currentMainInventoryUseItemEvent = currentMainInventorySlot.itemSO.itemUseEvent;
        }
        else
        {
            currentMainInventoryUseItemEvent = null;
        }
    }

    // ���õ� ������ ǥ�� UI�� �����ֱ�
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

    // ���õ� ������ ǥ�� UI�� �����
    private void RemoveSelectUI()
    {
        currentUseItemEvent = null;
        currentInventorySlot = null;
        
        selectUI.gameObject.SetActive(false);
        useItemButton.SetActive(false);
        deleteItemButton.SetActive(false);
    }
}
