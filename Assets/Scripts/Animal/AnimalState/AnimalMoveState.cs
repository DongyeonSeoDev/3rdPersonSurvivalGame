using UnityEngine;
using UnityEngine.AI;

public class AnimalMoveState : IAnimalState // ������ State
{
    public void Start(AnimalStateData animalStateData)
    {
        Vector3 targetPosition;
        NavMeshHit navMeshHit;

        // ���� ��ġ�� �����ͼ� �� �� �ִ� ��ġ�� �̵�
        for (int i = 0; i < animalStateData.navMeshMaxFindPathCount; i++)
        {
            targetPosition = animalStateData.animalTransform.position + GameManager.Instance.RandomDirection() * Random.Range(animalStateData.minMoveDistance, animalStateData.maxMoveDistance);

            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
            {
                animalStateData.navMeshAgent.speed = animalStateData.moveSpeed;
                animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Walk);
                animalStateData.navMeshAgent.SetDestination(navMeshHit.position);

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
        else if (animalStateData.isDamage)
        {
            // �������� �Ծ��ٸ� RunAway State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // �̵��� �Ϸ��ߴٸ� Idle State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }
}