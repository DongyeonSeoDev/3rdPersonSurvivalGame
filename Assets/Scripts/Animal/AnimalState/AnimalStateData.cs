using UnityEngine;
using UnityEngine.AI;

public class AnimalStateData
{
    public AnimalAnimation animalAnimation; // ���� �ִϸ��̼� ����
    public NavMeshAgent navMeshAgent; // NavMesh ���
    public Transform animalTransform; // ���� ��ġ

    public IAnimalState currentAnimalState; // ���� State

    public float minMoveDelay; // �������� ���� �����ӱ��� ��ٸ��� �ּ� �ð�
    public float maxMoveDelay; // �������� ���� �����ӱ��� ��ٸ��� �ִ� �ð�
    public float currentTime; // ���� ��ٸ� �ð�
    public float delayTime; // ��ٸ��� �ð�
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
    public bool isDamage; // �������� �޾Ҵ��� Ȯ��
    public bool isDead; // �׾����� Ȯ��
    public ItemSO lootItem; // �� ����ǰ

    private bool isStart; // ���� �Լ��� �����ߴ��� Ȯ��

    public void Process()
    {
        if (currentAnimalState != null)
        {
            if (!isStart)
            {
                // ���� �Լ� ����
                isStart = true;
                currentAnimalState.Start(this);
            }
            else
            {
                // �Լ� ����
                currentAnimalState.Update(this);
            }
        }
    }

    // State ����
    public void ChangeState(IAnimalState nextState)
    {
        currentAnimalState = nextState;
        isStart = false;
    }
}