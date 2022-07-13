using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // 커서 설정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
