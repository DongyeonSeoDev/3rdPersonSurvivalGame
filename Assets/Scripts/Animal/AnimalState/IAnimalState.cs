public interface IAnimalState // 동물 State 인터페이스
{
    public void Start(AnimalStateData animalStateData); // 시작할 때 한번 실행하는 함수
    public void Update(AnimalStateData animalStateData); // 계속 실행하는 함수
}