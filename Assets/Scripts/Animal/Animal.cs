using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public float minMoveDelay; // 움직임후 다음 움직임까지 기다리는 최소 시간
    public float maxMoveDelay; // 움직임후 다음 움직임까지 기다리는 최대 시간
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
    public int maxHp; // 최대 체력

    private readonly AnimalStateData stateData = new AnimalStateData(); // State 데이터

    private int currentHp; // 현재 체력

    private void Start()
    {
        // State 데이터 설정
        stateData.navMeshAgent = GetComponent<NavMeshAgent>();
        stateData.animalAnimation = GetComponent<AnimalAnimation>();

        stateData.animalTransform = transform;

        stateData.minMoveDelay = minMoveDelay;
        stateData.maxMoveDelay = maxMoveDelay;
        stateData.navMeshMaxDistance = navMeshMaxDistance;
        stateData.navMeshMinMoveVelocity = navMeshMinMoveVelocity;
        stateData.navMeshArrivalDistance = navMeshArrivalDistance;
        stateData.moveSpeed = moveSpeed;
        stateData.runAwaySpeed = runAwaySpeed;
        stateData.minMoveDistance = minMoveDistance;
        stateData.maxMoveDistance = maxMoveDistance;
        stateData.minRunAwayDistance = minRunAwayDistance;
        stateData.maxRunAwayDistance = maxRunAwayDistance;
        stateData.navMeshMaxFindPathCount = navMeshMaxFindPathCount;

        stateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle)); // 처음에는 Idle로 설정

        currentHp = maxHp;
    }

    private void Update()
    {
        stateData.Process(); // State 패턴 실행
    }

    public void GetDamage(int damage)
    {
        if (!stateData.isDead)
        {
            stateData.isDamage = true;

            currentHp -= damage;

            if (currentHp <= 0)
            {
                stateData.isDead = true;
            }
        }
    }
}
