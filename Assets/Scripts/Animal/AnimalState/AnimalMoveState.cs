using UnityEngine;
using UnityEngine.AI;

public class AnimalMoveState : IAnimalState // ������ State
{
    public void Start(AnimalStateData animalStateData)
    {
        // ���� ��ġ�� �����ͼ� �� �� �ִ� ��ġ�� �̵�
        Vector3 randomPosition;
        NavMeshHit navMeshHit;

        for (int i = 0; i < animalStateData.navMeshMaxFindPathCount; i++)
        {
            randomPosition = animalStateData.animalTransform.position + Random.insideUnitSphere * animalStateData.moveRange;

            if (NavMesh.SamplePosition(randomPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
            {
                animalStateData.navMeshAgent.SetDestination(navMeshHit.position);
                animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Walk);

                return;
            }
        }

        // �ִ� ��ã�� Ƚ���� �Ѿ�� Idle��
        animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
    }

    public void Update(AnimalStateData animalStateData)
    {
        if (animalStateData.isDead) // �׾��ٸ� ���� State��
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.isDamage) // �������� �Ծ��ٸ� ���� State��
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // �̵��� �Ϸ��ߴٸ� Idle��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }
}