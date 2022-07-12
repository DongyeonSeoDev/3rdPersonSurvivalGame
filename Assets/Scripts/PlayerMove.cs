using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed")]
    public float moveSpeed; // 움직임
    public float jumpSpeed; // 점프
    public float gravityScale; // 중력

    private CharacterController characterController;

    private Vector2 input; // 입력
    private Vector3 targetMovePosition; // 목표 위치

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        targetMovePosition.y -= gravityScale * Time.deltaTime; // 중력 적용
        characterController.Move(targetMovePosition * Time.deltaTime); // 움직임

        // 바닥에 닿아있으면 중력 제거
        if (characterController.isGrounded)
        {
            targetMovePosition.y = 0;
        }
    }

    // 이동 키(W, A, S, D)를 눌렀을때 실행 
    public void OnMove(InputAction.CallbackContext context)
    {
        //입력값을 받아와서 이동
        input = context.ReadValue<Vector2>();

        targetMovePosition.x = input.x * moveSpeed;
        targetMovePosition.z = input.y * moveSpeed;
    }

    // 점프 키(Space)를 눌렀을때 실행
    public void OnJump(InputAction.CallbackContext context)
    {
        // 버튼을 눌렀을때 바닥에 닿아있다면 점프
        if (context.performed && characterController.isGrounded)
        {
            targetMovePosition.y = jumpSpeed;
        }
    }
}
