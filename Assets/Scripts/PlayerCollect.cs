using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollect : MonoBehaviour
{
    public float checkCollectableTime; // 아이템 채집 활성화 시간

    private BoxCollider checkCollectableCollider; // 아이템 채집 콜라이더

    private float currentCheckCollectableTime; // 현재 채집 활성화 시간
    private bool isGetItem; // 아이템 획득 확인

    private void Awake()
    {
        checkCollectableCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // 아이템 채집 활성화 시간이 끝나면 콜라이더 비활성화
        if (currentCheckCollectableTime > 0)
        {
            currentCheckCollectableTime -= Time.deltaTime;

            if (currentCheckCollectableTime <= 0)
            {
                DisableCheckCollider();
            }
        }
    }

    // 마우스 좌클릭으로 실행
    public void OnPlayerBehavior(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EnableCheckCollider();
        }
    }

    // 아이템 채집 활성화
    private void EnableCheckCollider()
    {
        checkCollectableCollider.enabled = true;
        isGetItem = false;
        currentCheckCollectableTime = checkCollectableTime;
    }

    // 아이템 채집 비활성화
    private void DisableCheckCollider()
    {
        checkCollectableCollider.enabled = false;
        currentCheckCollectableTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGetItem) // 아이템을 이미 얻었다면 종료
        {
            return;
        }

        // 채집 가능 아이템인지 확인
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();

        if (collectableObject != null)
        {
            // 아이템 획득
            ItemSO item = collectableObject.GetItem();

            InventoryManager.Instance.AddItem(item);
            isGetItem = true;

            DisableCheckCollider();
        }
    }
}
