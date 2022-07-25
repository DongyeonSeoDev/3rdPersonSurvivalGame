using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollect : MonoBehaviour
{
    public float checkCollectableTime; // ������ ä�� Ȱ��ȭ �ð�

    private BoxCollider checkCollectableCollider; // ������ ä�� �ݶ��̴�

    private float currentCheckCollectableTime; // ���� ä�� Ȱ��ȭ �ð�
    private bool isActive; // ������ ȹ�� Ȯ��

    private void Awake()
    {
        checkCollectableCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // ������ ä�� Ȱ��ȭ �ð��� ������ �ݶ��̴� ��Ȱ��ȭ
        if (currentCheckCollectableTime > 0)
        {
            currentCheckCollectableTime -= Time.deltaTime;

            if (currentCheckCollectableTime <= 0)
            {
                SetCheckCollider(false);
            }
        }
    }

    // ���콺 ��Ŭ������ ����
    public void OnPlayerBehavior(InputAction.CallbackContext context)
    {
        // �Է��� ���԰�, �κ��丮�� �������� �ʴٸ� ����
        if (context.performed && !ItemUI.isInventoryOpen)
        {
            if (!InventoryManager.Instance.UseMainItem()) // ������ ���
            {
                // �������� ������� �ʾҴٸ� ����
                SetCheckCollider(true);
            }
        }
    }

    // ������ ä�� ����
    private void SetCheckCollider(bool isEnable)
    {
        checkCollectableCollider.enabled = isEnable;
        isActive = !isEnable;
        currentCheckCollectableTime = isEnable ? checkCollectableTime : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive) // �̹� ������ �ߴٸ� ����
        {
            return;
        }

        // ä�� ���� ���������� Ȯ��
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();

        if (collectableObject != null)
        {
            ItemSO item = collectableObject.GetItem();

            if (item != null)
            {
                // �������� null�� �ƴϸ� ������ ȹ��
                InventoryManager.Instance.AddItem(item);

                SetCheckCollider(false);
            }
        }

        ItemSO currentItem = InventoryManager.Instance.CurrentItem();

        // ������ ������ �������� ����ִٸ�
        if (currentItem != null && currentItem.attackPower > 0)
        {
            // ������ �ִ��� Ȯ��
            Animal animal = other.GetComponent<Animal>();

            if (animal != null)
            {
                // ������ �ִٸ� ����
                animal.GetDamage(currentItem.attackPower);

                SetCheckCollider(false);
            }
        }
    }
}
