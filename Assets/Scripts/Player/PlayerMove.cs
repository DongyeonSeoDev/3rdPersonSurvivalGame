using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public CameraMove cameraMove;

    public float moveSpeed; // ������
    public float jumpSpeed; // ����
    public float rotationMaxDegreesDelta; // ȸ��
    public float gravityScale; // �߷�

    private CharacterController characterController;
    private Animator anim;

    private Vector2 input; // �Է�
    private Vector3 targetMovePosition; // ��ǥ ��ġ
    private Vector3 currentMovePosition; // ���� ��ġ
    private Quaternion targetRotation; // ��ǥ ȸ�� ��

    private bool isMove; // �����̰� �ִ��� Ȯ��

    // �ִϸ��̼� Parameters Hash ��
    private readonly int hashIsMove = Animator.StringToHash("isMove");

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // �÷��̾� ����
        GameManager.player = transform;
    }

    private void Update()
    {
        targetMovePosition.y -= gravityScale * Time.deltaTime; // �߷� ����
        // ī�޶� ���� ���ؿ��� ��ǥ �������� ����
        currentMovePosition = Quaternion.Euler(0f, cameraMove.CurrentCameraAngle.y + 180f, 0f) * targetMovePosition * Time.deltaTime;

        characterController.Move(currentMovePosition); // �̵�

        // �÷��̾ �̵��ϴ� �������� ȸ��
        if (isMove)
        {
            targetRotation = Quaternion.Euler(0f, Quaternion.LookRotation(currentMovePosition).eulerAngles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationMaxDegreesDelta);
        }

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

        // ������ ���� ��ȯ
        if (isMove && input == Vector2.zero)
        {
            isMove = false;
            anim.SetBool(hashIsMove, false);
        }
        else if (!isMove && input != Vector2.zero)
        {
            isMove = true;
            anim.SetBool(hashIsMove, true);
        }
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

    // ��� ������ ������Ʈ�� �ִ��� Ȯ��
    private void BuildObjectCheck(BuildObject buildObject, bool isEnter)
    {
        if (buildObject != null)
        {
            // BuildManager�� ��� ������ ������Ʈ�� �߰� �Ǵ� ����
            if (!BuildManager.Instance.isUsableBuilding.ContainsKey(buildObject.buildItem))
            {
                BuildManager.Instance.isUsableBuilding.Add(buildObject.buildItem, isEnter);

                return;
            }

            BuildManager.Instance.isUsableBuilding[buildObject.buildItem] = isEnter;
        }
    }

    // ��� ������ ������Ʈ�� �ִ��� Ȯ��
    private void OnTriggerEnter(Collider other)
    {
        BuildObjectCheck(other.GetComponent<BuildObject>(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        BuildObjectCheck(other.GetComponent<BuildObject>(), false);
    }
}
