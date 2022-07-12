using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed")]
    public float moveSpeed; // ������
    public float jumpSpeed; // ����
    public float gravityScale; // �߷�

    private CharacterController characterController;

    private Vector2 input; // �Է�
    private Vector3 targetMovePosition; // ��ǥ ��ġ

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        targetMovePosition.y -= gravityScale * Time.deltaTime; // �߷� ����
        characterController.Move(targetMovePosition * Time.deltaTime); // ������

        // �ٴڿ� ��������� �߷� ����
        if (characterController.isGrounded)
        {
            targetMovePosition.y = 0;
        }
    }

    // �̵� Ű(W, A, S, D)�� �������� ���� 
    public void OnMove(InputAction.CallbackContext context)
    {
        //�Է°��� �޾ƿͼ� �̵�
        input = context.ReadValue<Vector2>();

        targetMovePosition.x = input.x * moveSpeed;
        targetMovePosition.z = input.y * moveSpeed;
    }

    // ���� Ű(Space)�� �������� ����
    public void OnJump(InputAction.CallbackContext context)
    {
        // ��ư�� �������� �ٴڿ� ����ִٸ� ����
        if (context.performed && characterController.isGrounded)
        {
            targetMovePosition.y = jumpSpeed;
        }
    }
}
