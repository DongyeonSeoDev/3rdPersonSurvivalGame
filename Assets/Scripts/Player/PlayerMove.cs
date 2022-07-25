using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public CameraMove cameraMove;

    public float moveSpeed; // 움직임
    public float jumpSpeed; // 점프
    public float rotationMaxDegreesDelta; // 회전
    public float gravityScale; // 중력

    private CharacterController characterController;
    private Animator anim;

    private Vector2 input; // 입력
    private Vector3 targetMovePosition; // 목표 위치
    private Vector3 currentMovePosition; // 현재 위치
    private Quaternion targetRotation; // 목표 회전 값

    private bool isMove; // 움직이고 있는지 확인

    // 애니메이션 Parameters Hash 값
    private readonly int hashIsMove = Animator.StringToHash("isMove");

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // 플레이어 설정
        GameManager.player = transform;
    }

    private void Update()
    {
        targetMovePosition.y -= gravityScale * Time.deltaTime; // 중력 적용
        // 카메라 방향 기준에서 목표 방향으로 설정
        currentMovePosition = Quaternion.Euler(0f, cameraMove.CurrentCameraAngle.y + 180f, 0f) * targetMovePosition * Time.deltaTime;

        characterController.Move(currentMovePosition); // 이동

        // 플레이어가 이동하는 방향으로 회전
        if (isMove)
        {
            targetRotation = Quaternion.Euler(0f, Quaternion.LookRotation(currentMovePosition).eulerAngles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationMaxDegreesDelta);
        }

        // 바닥에 닿아있으면 중력 제거
        if (characterController.isGrounded)
        {
            targetMovePosition.y = 0;
        }
    }

    // 이동 키(W, A, S, D)를 눌렀을때 실행 
    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        //입력값을 받아와서 이동
        input = context.ReadValue<Vector2>();

        targetMovePosition.x = input.x * moveSpeed;
        targetMovePosition.z = input.y * moveSpeed;

        // 움직임 상태 전환
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

    // 점프 키(Space)를 눌렀을때 실행
    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        // 버튼을 눌렀을때 바닥에 닿아있다면 점프
        if (context.performed && characterController.isGrounded)
        {
            targetMovePosition.y = jumpSpeed;
        }
    }

    // 사용 가능한 오브젝트가 있는지 확인
    private void BuildObjectCheck(BuildObject buildObject, bool isEnter)
    {
        if (buildObject != null)
        {
            // BuildManager에 사용 가능한 오브젝트에 추가 또는 삭제
            if (!BuildManager.Instance.isUsableBuilding.ContainsKey(buildObject.buildItem))
            {
                BuildManager.Instance.isUsableBuilding.Add(buildObject.buildItem, isEnter);

                return;
            }

            BuildManager.Instance.isUsableBuilding[buildObject.buildItem] = isEnter;
        }
    }

    // 사용 가능한 오브젝트가 있는지 확인
    private void OnTriggerEnter(Collider other)
    {
        BuildObjectCheck(other.GetComponent<BuildObject>(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        BuildObjectCheck(other.GetComponent<BuildObject>(), false);
    }
}
