using UnityEngine;

public class AnimalIdleState : IAnimalState // Idle State
{
    // ���ð��� �������� ������
    public void Start(AnimalStateData animalStateData)
    {
        animalStateData.currentTime = 0f;
        animalStateData.delayTime = Random.Range(animalStateData.minMoveDelay, animalStateData.maxMoveDelay);

        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
    }

    // ���ð���ŭ ��⸦ �� �� ������
    public void Update(AnimalStateData animalStateData)
    {
        animalStateData.currentTime += Time.deltaTime;

        if (animalStateData.isDead) // �׾��ٸ� ���� State��
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.isDamage) // �������� �Ծ��ٸ� ���� State��
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.delayTime < animalStateData.currentTime)
        {
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Move));
        }
    }
}
