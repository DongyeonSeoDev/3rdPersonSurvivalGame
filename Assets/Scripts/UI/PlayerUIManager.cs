using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerStats playerStatus; // 플레이어 상태
    public Image fillHpBar; // 체력바 채우는 이미지
    public Image fillHungerBar; // 배고픔바 채우는 이미지

    // 싱글톤 패턴
    private static PlayerUIManager instance;
    public static PlayerUIManager Instance
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
        // 이벤트 등록
        playerStatus.changeHpEvent += SetHpBar;
        playerStatus.changeHungerEvent += SetHungerBar;
    }

    // 체력바 채우기
    public void SetHpBar(float fillValue)
    {
        fillHpBar.fillAmount = fillValue;
    }

    // 배고픔바 채우기
    public void SetHungerBar(float fillValue)
    {
        fillHungerBar.fillAmount = fillValue;
    }
}
