using UnityEngine;

public class AnimalIdleState : IAnimalState // Idle State
{
    public void Start(AnimalStateData animalStateData)
    {
        // ���ð��� �������� ������
        animalStateData.currentTime = 0f;
        animalStateData.delayTime = Random.Range(animalStateData.minMoveDelay, animalStateData.maxMoveDelay);

        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
    }

    public void Update(AnimalStateData animalStateData)
    {
        animalStateData.currentTime += Time.deltaTime;

        if (animalStateData.isDead)
        {
            // �׾��ٸ� ���� State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Dead));
        }
        else if (animalStateData.isDamage)
        {
            // �������� �Ծ��ٸ� ���� State��
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.RunAway));
        }
        else if (animalStateData.delayTime < animalStateData.currentTime)
        {
            // ���ð��� �����ٸ� �̵�
            animalStateData.ChangeState(AnimalState.Instance.GetAnimalState(AnimalStateType.Move));
        }
    }
}
