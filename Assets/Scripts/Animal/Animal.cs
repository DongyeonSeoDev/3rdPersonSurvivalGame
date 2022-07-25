using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public Renderer animalRenderer; // ���� Renderer
    public float damageTime = 0f; // Damage ����Ʈ �ð�

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
    public ItemSO lootItem; // �� ����ǰ

    private readonly AnimalStateData stateData = new AnimalStateData(); // State ������

    private Material animalMaterial; // ���� Material

    private int currentHp; // ���� ü��
    private float currentDamageTime; // ���� Damage ����Ʈ �ð� 

    private void Start()
    {
        // Material�� ������
        animalMaterial = animalRenderer.material;

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
        stateData.lootItem = lootItem;

        // ó������ Idle�� ����
        stateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));

        // ü�� ����
        currentHp = maxHp;
    }

    private void Update()
    {
        stateData.Process(); // State ���� ����

        if (currentDamageTime > 0f) // ���ظ� �Ծ����� ����
        {
            currentDamageTime -= Time.deltaTime;

            if (currentDamageTime <= 0f) // ���� ����Ʈ �ð��� �����ٸ�
            {
                animalMaterial.SetInt("_ChangeColor", 0); // ���� ������� ����
            }
        }
    }

    public void GetDamage(int damage)
    {
        if (!stateData.isDead && currentDamageTime <= 0f) // ���� �ʾҰ� Damage ����Ʈ ȿ���� ���ٸ�
        {
            stateData.isDamage = true; // State ����

            animalMaterial.SetInt("_ChangeColor", 1); // ���� ����
            currentDamageTime = damageTime; // ����Ʈ ����

            // ü���� ���ְ�, �׾����� Ȯ��
            currentHp -= damage;

            if (currentHp <= 0)
            {
                stateData.isDead = true;
            }
        }
    }
}
