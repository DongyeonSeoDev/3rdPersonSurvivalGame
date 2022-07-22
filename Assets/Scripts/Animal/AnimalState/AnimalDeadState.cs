public class AnimalDeadState : IAnimalState // Á×¾úÀ»¶§ State
{
    // Á×À½ Ã³¸®
    public void Start(AnimalStateData animalStateData)
    {
        animalStateData.animalTransform.gameObject.SetActive(false);
        animalStateData.animalAnimation.SetAnimalAnimation(AnimalAnimationType.Idle);
        animalStateData.ChangeState(null);
    }

    public void Update(AnimalStateData animalStateData) { }
}
