using UnityEngine;
using UnityEngine.AI;

public class AnimalRunAwayState : IAnimalState
{
    public void Start(AnimalStateData animalStateData)
    {
        // TODO ������ �������� �ڵ� �ۼ�
    }

    public void Update(AnimalStateData animalStateData)
    {
        if (animalStateData.isDead) // �׾��ٸ� Dead��
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // �̵��� �Ϸ��ߴٸ� Idle��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }
}
