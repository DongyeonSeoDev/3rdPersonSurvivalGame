using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public Transform player;

    public float limitMinCameraRotationX; // �ּ� ȸ�� X��
    public float limitMaxCameraRotationX; // �ִ� ȸ�� X��

    public float cameraRotationSpeedX; // ī�޶� ȸ�� X �ӵ�
    public float cameraRotationSpeedY; // ī�޶� ȸ�� Y �ӵ�

    public float cameraDistance; // ī�޶�� �÷��̾� �Ÿ�

    // Cinemachine Camera ���
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineTransposer cinemachineTransposer;

    private Vector2 moveMouseDirection; // ���콺 ������
    private Vector3 currentCameraAngle; // ���� ī�޶� ����
    public Vector3 CurrentCameraAngle
    {
        get { return currentCameraAngle; }
    }

    private void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        // ī�޶��� ��ġ�� ���� �� ī�޶� ������
        cinemachineTransposer.m_FollowOffset = Quaternion.Euler(0f, -player.rotation.eulerAngles.y, 0f) * Quaternion.Euler(currentCameraAngle) * Vector3.forward * cameraDistance;
    }

    // ���콺�� ���������� ����
    public void OnCameraRotation(InputAction.CallbackContext context)
    {
        // ���콺�� ������ ������ ������
        moveMouseDirection = context.ReadValue<Vector2>();

        // ī�޶� ������ ���콺�� ������ ���� �߰�
        currentCameraAngle.x += moveMouseDirection.y * cameraRotationSpeedX * Time.deltaTime;
        currentCameraAngle.y += moveMouseDirection.x * cameraRotationSpeedY * Time.deltaTime;

        // ī�޶� ���� ����
        currentCameraAngle.x = Mathf.Clamp(currentCameraAngle.x, limitMinCameraRotationX, limitMaxCameraRotationX);
        currentCameraAngle.y = cameraRotationSpeedY < 0 ? 360 - currentCameraAngle.y : currentCameraAngle.y;
        currentCameraAngle.y = cameraRotationSpeedY > 360 ? currentCameraAngle.y - 360 : currentCameraAngle.y;
    }
}