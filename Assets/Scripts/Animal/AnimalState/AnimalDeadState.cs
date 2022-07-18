public class AnimalDeadState : IAnimalState // �׾����� State
{
    // ���� ó��
    public void Start(AnimalStateData animalStateData)
    {
        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
        animalStateData.ChangeState(null);
    }

    public void Update(AnimalStateData animalStateData) { }
}
