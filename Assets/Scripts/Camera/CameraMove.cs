using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public Transform player;

    public float limitMinCameraRotationX; // 최소 회전 X값
    public float limitMaxCameraRotationX; // 최대 회전 X값

    public float cameraRotationSpeedX; // 카메라 회전 X 속도
    public float cameraRotationSpeedY; // 카메라 회전 Y 속도

    public float cameraDistance; // 카메라와 플레이어 거리

    // Cinemachine Camera 사용
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineTransposer cinemachineTransposer;

    private Vector2 moveMouseDirection; // 마우스 움직임
    private Vector3 currentCameraAngle; // 현재 카메라 각도
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
        // 카메라의 위치를 정한 후 카메라를 움직임
        cinemachineTransposer.m_FollowOffset = Quaternion.Euler(0f, -player.rotation.eulerAngles.y, 0f) * Quaternion.Euler(currentCameraAngle) * Vector3.forward * cameraDistance;
    }

    // 마우스를 움직였을때 실행
    public void OnCameraRotation(InputAction.CallbackContext context)
    {
        // 마우스를 움직인 방향을 가져옴
        moveMouseDirection = context.ReadValue<Vector2>();

        // 카메라 각도에 마우스를 움직인 방향 추가
        currentCameraAngle.x += moveMouseDirection.y * cameraRotationSpeedX * Time.deltaTime;
        currentCameraAngle.y += moveMouseDirection.x * cameraRotationSpeedY * Time.deltaTime;

        // 카메라 각도 제한
        currentCameraAngle.x = Mathf.Clamp(currentCameraAngle.x, limitMinCameraRotationX, limitMaxCameraRotationX);
        currentCameraAngle.y = cameraRotationSpeedY < 0 ? 360 - currentCameraAngle.y : currentCameraAngle.y;
        currentCameraAngle.y = cameraRotationSpeedY > 360 ? currentCameraAngle.y - 360 : currentCameraAngle.y;
    }
}