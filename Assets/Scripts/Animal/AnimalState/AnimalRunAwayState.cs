using UnityEngine;
using UnityEngine.AI;

public class AnimalRunAwayState : IAnimalState
{
    public void Start(AnimalStateData animalStateData)
    {
        animalStateData.isDamage = false;

        Vector3 targetPosition;
        Vector3 direction;
        NavMeshHit navMeshHit;

        // �÷��̾� �ݴ� ���� ��������
        direction = (animalStateData.animalTransform.position - GameManager.Instance.player.position).normalized;
        direction.y = 0f;

        // �÷��̾� �ݴ� ���⿡ ���� �ִ��� Ȯ��
        targetPosition = animalStateData.animalTransform.position + direction * Random.Range(animalStateData.minRunAwayDistance, animalStateData.maxRunAwayDistance);

        if (NavMesh.SamplePosition(targetPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
        {
            // ���� �ִٸ� �̵�
            SetRunAway(animalStateData, navMeshHit.position);
            return;
        }

        // ���� ���ٸ� �������� �̵�
        for (int i = 0; i < animalStateData.navMeshMaxFindPathCount; i++)
        {
            targetPosition = animalStateData.animalTransform.position + GameManager.Instance.RandomDirection() * Random.Range(animalStateData.minRunAwayDistance, animalStateData.maxRunAwayDistance);

            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
            {
                SetRunAway(animalStateData, navMeshHit.position);
                return;
            }
        }

        // �ִ� ��ã�� Ƚ���� �Ѿ�� Idle��
        animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
    }

    public void Update(AnimalStateData animalStateData)
    {
        if (animalStateData.isDead)
        {
            // �׾��ٸ� Dead State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // �̵��� �Ϸ��ߴٸ� Idle State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }

    // ������ �������� ��ġ ����
    private void SetRunAway(AnimalStateData animalStateData, Vector3 position)
    {
        animalStateData.navMeshAgent.speed = animalStateData.runAwaySpeed;
        animalStateData.navMeshAgent.SetDestination(position);
        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Run);
    }
}
