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

        // 플레이어 반대 방향 가져오기
        direction = (animalStateData.animalTransform.position - GameManager.Instance.player.position).normalized;
        direction.y = 0f;

        // 플레이어 반대 방향에 길이 있는지 확인
        targetPosition = animalStateData.animalTransform.position + direction * Random.Range(animalStateData.minRunAwayDistance, animalStateData.maxRunAwayDistance);

        if (NavMesh.SamplePosition(targetPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
        {
            // 길이 있다면 이동
            SetRunAway(animalStateData, navMeshHit.position);
            return;
        }

        // 길이 없다면 랜덤으로 이동
        for (int i = 0; i < animalStateData.navMeshMaxFindPathCount; i++)
        {
            targetPosition = animalStateData.animalTransform.position + GameManager.Instance.RandomDirection() * Random.Range(animalStateData.minRunAwayDistance, animalStateData.maxRunAwayDistance);

            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, animalStateData.navMeshMaxDistance, NavMesh.AllAreas))
            {
                SetRunAway(animalStateData, navMeshHit.position);
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
        else if (animalStateData.navMeshAgent.velocity.sqrMagnitude > animalStateData.navMeshMinMoveVelocity && animalStateData.navMeshAgent.remainingDistance <= animalStateData.navMeshArrivalDistance)
        {
            // 이동을 완료했다면 Idle State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Idle));
        }
    }

    // 동물이 도망가는 위치 설정
    private void SetRunAway(AnimalStateData animalStateData, Vector3 position)
    {
        animalStateData.navMeshAgent.speed = animalStateData.runAwaySpeed;
        animalStateData.navMeshAgent.SetDestination(position);
        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Run);
    }
}
