using UnityEngine;

public class AnimalIdleState : IAnimalState // Idle State
{
    public void Start(AnimalStateData animalStateData)
    {
        // 대기시간을 랜덤으로 가져옴
        animalStateData.currentTime = 0f;
        animalStateData.delayTime = Random.Range(animalStateData.minMoveDelay, animalStateData.maxMoveDelay);

        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
    }

    public void Update(AnimalStateData animalStateData)
    {
        animalStateData.currentTime += Time.deltaTime;

        if (animalStateData.isDead)
        {
            // 죽었다면 죽음 State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.isDamage)
        {
            // 데미지를 입었다면 도망 State로
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.delayTime < animalStateData.currentTime)
        {
            // 대기시간이 지났다면 이동
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Move));
        }
    }
}
