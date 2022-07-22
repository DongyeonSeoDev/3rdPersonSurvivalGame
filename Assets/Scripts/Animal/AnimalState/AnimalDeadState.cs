public class AnimalDeadState : IAnimalState // �׾����� State
{
    // ���� ó��
    public void Start(AnimalStateData animalStateData)
    {
        animalStateData.animalTransform.gameObject.SetActive(false);
        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
        animalStateData.ChangeState(null);
    }

    public void Update(AnimalStateData animalStateData) { }
}
