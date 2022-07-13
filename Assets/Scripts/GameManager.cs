using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    // �κ��丮�� �����ų� ���� �� �۵�
    public void ChangeInventoryState(bool isOpen)
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
}
