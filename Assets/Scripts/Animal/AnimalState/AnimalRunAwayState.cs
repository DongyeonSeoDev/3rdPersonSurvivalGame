using UnityEngine;
using UnityEngine.AI;

public class AnimalRunAwayState : IAnimalState
{
    public void Start(AnimalStateData animalStateData)
    {
        // TODO 동물이 도망가는 코드 작성
    }

    public void Update(AnimalStateData animalStateData)
    {
        if (animalStateData.isDead) // 죽었다면 Dead로
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // 이동을 완료했다면 Idle로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }
}
