using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerStats playerStatus; // �÷��̾� ����
    public Image fillHpBar; // ü�¹� ä��� �̹���
    public Image fillHungerBar; // ����Ĺ� ä��� �̹���

    // �̱��� ����
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
        // �̺�Ʈ ���
        playerStatus.changeHpEvent += SetHpBar;
        playerStatus.changeHungerEvent += SetHungerBar;
    }

    // ü�¹� ä���
    public void SetHpBar(float fillValue)
    {
        fillHpBar.fillAmount = fillValue;
    }

    // ����Ĺ� ä���
    public void SetHungerBar(float fillValue)
    {
        fillHungerBar.fillAmount = fillValue;
    }
}
