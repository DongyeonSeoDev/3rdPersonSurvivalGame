using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player; // 플레이어 위치

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

    // 아이템 UI가 열리거나 닫힐 때 작동
    public void ChangeItemUIState(bool isOpen)
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

    // 랜덤 방향을 가져오는 함수
    public Vector3 RandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);

        return new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0f, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}
