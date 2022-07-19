using UnityEngine;
using UnityEngine.AI;

public class AnimalMoveState : IAnimalState // 움직임 State
{
    public void Start(AnimalStateData animalStateData)
    {
        Vector3 targetPosition;
        NavMeshHit navMeshHit;

        // 랜덤 위치를 가져와서 갈 수 있는 위치면 이동
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

        // 최대 길찾기 횟수를 넘어가면 Idle로
        animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
    }

    public void Update(AnimalStateData animalStateData)
    {
        if (animalStateData.isDead)
        {
            // 죽었다면 Dead State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.isDamage)
        {
            // 데미지를 입었다면 RunAway State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // 이동을 완료했다면 Idle State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }
}