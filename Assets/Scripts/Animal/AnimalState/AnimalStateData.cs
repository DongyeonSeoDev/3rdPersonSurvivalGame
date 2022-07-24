using UnityEngine;
using UnityEngine.AI;

public class AnimalStateData
{
    public AnimalAnimation animalAnimation; // 동물 애니메이션 관리
    public NavMeshAgent navMeshAgent; // NavMesh 사용
    public Transform animalTransform; // 동물 위치

    public IAnimalState currentAnimalState; // 현재 State

    public float minMoveDelay; // 움직임후 다음 움직임까지 기다리는 최소 시간
    public float maxMoveDelay; // 움직임후 다음 움직임까지 기다리는 최대 시간
    public float currentTime; // 현재 기다린 시간
    public float delayTime; // 기다리는 시간
    public float navMeshMaxDistance; // NavMesh 최대 검색 거리
    public float navMeshMinMoveVelocity; // 이동을 확인할 때 최소 속도
    public float navMeshArrivalDistance; // 도착을 확인하는 거리
    public float moveSpeed; // 움직일 때 스피드
    public float runAwaySpeed; // 도망갈 때 스피드
    public float minMoveDistance; // 최소 이동거리
    public float maxMoveDistance; // 최대 이동거리
    public float minRunAwayDistance; // 최소 도망거리
    public float maxRunAwayDistance; // 최대 도망거리
    public int navMeshMaxFindPathCount; // NavMesh 길찾기 최대 횟수
    public bool isDamage; // 데미지를 받았는지 확인
    public bool isDead; // 죽었는지 확인
    public ItemSO lootItem; // 적 전리품

    private bool isStart; // 시작 함수를 실행했는지 확인

    public void Process()
    {
        if (currentAnimalState != null)
        {
            if (!isStart)
            {
                // 시작 함수 실행
                isStart = true;
                currentAnimalState.Start(this);
            }
            else
            {
                // 함수 실행
                currentAnimalState.Update(this);
            }
        }
    }

    // State 변경
    public void ChangeState(IAnimalState nextState)
    {
        currentAnimalState = nextState;
        isStart = false;
    }
}