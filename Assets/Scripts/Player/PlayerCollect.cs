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
        // 입력이 들어왔고, 인벤토리가 열려있지 않다면 실행
        if (context.performed && !ItemUI.isInventoryOpen)
        {
            if (!InventoryManager.Instance.UseMainItem()) // 아이템 사용
            {
                // 아이템을 사용하지 않았다면 실행
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
        if (isActive) // 이미 실행을 했다면 종료
        {
            return;
        }

        // 채집 가능 아이템인지 확인
        CollectableObject collectableObject = other.GetComponent<CollectableObject>();

        if (collectableObject != null)
        {
            ItemSO item = collectableObject.GetItem();

            if (item != null)
            {
                // 아이템이 null이 아니면 아이템 획득
                InventoryManager.Instance.AddItem(item);

                SetCheckCollider(false);
            }
        }

        ItemSO currentItem = InventoryManager.Instance.CurrentItem();

        // 공격이 가능한 아이템을 들고있다면
        if (currentItem != null && currentItem.attackPower > 0)
        {
            // 동물이 있는지 확인
            Animal animal = other.GetComponent<Animal>();

            if (animal != null)
            {
                // 동물이 있다면 공격
                animal.GetDamage(currentItem.attackPower);

                SetCheckCollider(false);
            }
        }
    }
}
