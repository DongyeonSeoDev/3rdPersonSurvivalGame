using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴
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

    // 인벤토리가 열리거나 닫힐 때 작동
    public void ChangeInventoryState(bool isOpen)
    {
        SetCursorStates(!isOpen);
        SetTime(isOpen);
    }

    // 커서 설정
    private void SetCursorStates(bool isCursorLock)
    {
        Cursor.visible = !isCursorLock;
        Cursor.lockState = isCursorLock ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // TimeScale 설정
    private void SetTime(bool isTimeStop)
    {
        Time.timeScale = isTimeStop ? 0f : 1f;
    }
}
