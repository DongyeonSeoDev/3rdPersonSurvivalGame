using System.Collections.Generic;

public enum AnimalStateType // 동물 State 타입
{
    Idle, Move, RunAway, Dead
}

public class AnimalState
{
    // 싱글톤 패턴
    private static AnimalState instance;
    public static AnimalState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AnimalState();
            }

            return instance;
        }
    }

    // 동물 State Dictionary
    private readonly Dictionary<AnimalStateType, IAnimalState> animalStateDictionary = new Dictionary<AnimalStateType, IAnimalState>();

    // Dictionary에 데이터 추가
    private AnimalState()
    {
        animalStateDictionary.Add(AnimalStateType.Idle, new AnimalIdleState());
        animalStateDictionary.Add(AnimalStateType.Move, new AnimalMoveState());
        animalStateDictionary.Add(AnimalStateType.RunAway, new AnimalRunAwayState());
        animalStateDictionary.Add(AnimalStateType.Dead, new AnimalDeadState());
    }

    // State를 가져옴
    public IAnimalState GetAnimalState(AnimalStateType stateType)
    {
        return animalStateDictionary[stateType];
    }
}
