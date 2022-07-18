using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public float minMoveDelay; // 움직임후 다음 움직임까지 기다리는 최소 시간
    public float maxMoveDelay; // 움직임후 다음 움직임까지 기다리는 최대 시간
    public float moveRange; // 랜덤으로 움직이는 범위
    public float navMeshMaxDistance; // NavMesh 최대 검색 거리
    public float navMeshMinMoveVelocity; // 이동을 확인할 때 최소 속도
    public float navMeshArrivalDistance; // 도착을 확인하는 거리
    public int navMeshMaxFindPathCount; // NavMesh 길찾기 최대 횟수

    private AnimalStateData stateData = new AnimalStateData(); // State 데이터

    private void Start()
    {
        // State 데이터 설정
        stateData.navMeshAgent = GetComponent<NavMeshAgent>();
        stateData.animalAnimation = GetComponent<AnimalAnimation>();

        stateData.animalTransform = transform;

        stateData.minMoveDelay = minMoveDelay;
        stateData.maxMoveDelay = maxMoveDelay;
        stateData.moveRange = moveRange;
        stateData.navMeshMaxDistance = navMeshMaxDistance;
        stateData.navMeshMinMoveVelocity = navMeshMinMoveVelocity;
        stateData.navMeshArrivalDistance = navMeshArrivalDistance;
        stateData.navMeshMaxFindPathCount = navMeshMaxFindPathCount;

        stateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle)); // 처음에는 Idle로 설정
    }

    private void Update()
    {
        stateData.Process(); // State 패턴 실행
    }
}
