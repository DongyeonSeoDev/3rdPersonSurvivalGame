using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public CameraMove cameraMove;

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
        characterController.Move(Quaternion.Euler(0f, cameraMove.CurrentCameraAngle.y + 180f, 0f) * targetMovePosition * Time.deltaTime); // ������

        // �ٴڿ� ��������� �߷� ����
        if (characterController.isGrounded)
        {
            targetMovePosition.y = 0;
        }
    }

    // �̵� Ű(W, A, S, D)�� �������� ���� 
    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        //�Է°��� �޾ƿͼ� �̵�
        input = context.ReadValue<Vector2>();

        targetMovePosition.x = input.x * moveSpeed;
        targetMovePosition.z = input.y * moveSpeed;
    }

    // ���� Ű(Space)�� �������� ����
    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        // ��ư�� �������� �ٴڿ� ����ִٸ� ����
        if (context.performed && characterController.isGrounded)
        {
            targetMovePosition.y = jumpSpeed;
        }
    }
}
