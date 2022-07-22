using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public float minMoveDelay; // �������� ���� �����ӱ��� ��ٸ��� �ּ� �ð�
    public float maxMoveDelay; // �������� ���� �����ӱ��� ��ٸ��� �ִ� �ð�
    public float navMeshMaxDistance; // NavMesh �ִ� �˻� �Ÿ�
    public float navMeshMinMoveVelocity; // �̵��� Ȯ���� �� �ּ� �ӵ�
    public float navMeshArrivalDistance; // ������ Ȯ���ϴ� �Ÿ�
    public float moveSpeed; // ������ �� ���ǵ�
    public float runAwaySpeed; // ������ �� ���ǵ�
    public float minMoveDistance; // �ּ� �̵��Ÿ�
    public float maxMoveDistance; // �ִ� �̵��Ÿ�
    public float minRunAwayDistance; // �ּ� �����Ÿ�
    public float maxRunAwayDistance; // �ִ� �����Ÿ�
    public int navMeshMaxFindPathCount; // NavMesh ��ã�� �ִ� Ƚ��
    public int maxHp; // �ִ� ü��

    private readonly AnimalStateData stateData = new AnimalStateData(); // State ������

    private int currentHp; // ���� ü��

    private void Start()
    {
        // State ������ ����
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

        stateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle)); // ó������ Idle�� ����

        currentHp = maxHp;
    }

    private void Update()
    {
        stateData.Process(); // State ���� ����
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
