using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ

    // �̱��� ����
    private static GameManager instance;
    public static GameManager Instance
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
        SetCursorStates(true);
    }

    // ������ UI�� �����ų� ���� �� �۵�
    public void ChangeItemUIState(bool isOpen)
    {
        SetCursorStates(!isOpen);
        SetTime(isOpen);
    }

    // Ŀ�� ����
    private void SetCursorStates(bool isCursorLock)
    {
        Cursor.visible = !isCursorLock;
        Cursor.lockState = isCursorLock ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // TimeScale ����
    private void SetTime(bool isTimeStop)
    {
        Time.timeScale = isTimeStop ? 0f : 1f;
    }

    // ���� ������ �������� �Լ�
    public Vector3 RandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);

        return new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0f, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}
