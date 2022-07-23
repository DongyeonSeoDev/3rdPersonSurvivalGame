using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollect : MonoBehaviour
{
    public float checkCollectableTime; // 아이템 채집 활성화 시간

    private BoxCollider checkCollectableCollider; // 아이템 채집 콜라이더

    private float currentCheckCollectableTime; // 현재 채집 활성화 시간
    private bool isActive; // 아이템 획득 확인

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
                SetCheckCollider(false);
            }
        }
    }

    // 마우스 좌클릭으로 실행
    public void OnPlayerBehavior(InputAction.CallbackContext context)
    {
        if (context.performed && !ItemUI.isInventoryOpen)
        {
            if (!InventoryManager.Instance.UseMainItem())
            {
                SetCheckCollider(true);
            }
        }
    }

    // 아이템 채집 설정
    private void SetCheckCollider(bool isEnable)
    {
        checkCollectableCollider.enabled = isEnable;
        isActive = !isEnable;
        currentCheckCollectableTime = isEnable ? checkCollectableTime : 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive) // 아이템을 이미 얻었다면 종료
        {
            return;
        }

        // 채집 가능 아이템인지 확인
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();

        if (collectableObject != null)
        {
            // 아이템 획득
            ItemSO item = collectableObject.GetItem();

            if (item != null)
            {
                InventoryManager.Instance.AddItem(item);

                SetCheckCollider(false);
            }
        }

        // 공격이 가능한 아이템을 들고있다면
        ItemSO currentItem = InventoryManager.Instance.CurrentItem();

        if (currentItem != null && currentItem.attackPower > 0)
        {
            Animal animal = other.GetComponent<Animal>();

            if (animal != null)
            {
                animal.GetDamage(currentItem.attackPower);

                SetCheckCollider(false);
            }
        }
    }
}
