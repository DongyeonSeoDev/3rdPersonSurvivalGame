using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollect : MonoBehaviour
{
    public float checkCollectableTime; // ������ ä�� Ȱ��ȭ �ð�

    private BoxCollider checkCollectableCollider; // ������ ä�� �ݶ��̴�

    private float currentCheckCollectableTime; // ���� ä�� Ȱ��ȭ �ð�
    private bool isGetItem; // ������ ȹ�� Ȯ��

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
                DisableCheckCollider();
            }
        }
    }

    // ���콺 ��Ŭ������ ����
    public void OnPlayerBehavior(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EnableCheckCollider();
        }
    }

    // ������ ä�� Ȱ��ȭ
    private void EnableCheckCollider()
    {
        checkCollectableCollider.enabled = true;
        isGetItem = false;
        currentCheckCollectableTime = checkCollectableTime;
    }

    // ������ ä�� ��Ȱ��ȭ
    private void DisableCheckCollider()
    {
        checkCollectableCollider.enabled = false;
        currentCheckCollectableTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGetItem) // �������� �̹� ����ٸ� ����
        {
            return;
        }

        // ä�� ���� ���������� Ȯ��
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();

        if (collectableObject != null)
        {
            // ������ ȹ��
            ItemSO item = collectableObject.GetItem();

            InventoryManager.Instance.AddItem(item);
            isGetItem = true;

            DisableCheckCollider();
        }
    }
}
