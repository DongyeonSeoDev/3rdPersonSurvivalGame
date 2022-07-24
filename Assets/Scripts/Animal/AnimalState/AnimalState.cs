using System.Collections.Generic;

public enum AnimalStateType // ���� State Ÿ��
{
    Idle, Move, RunAway, Dead
}

public class AnimalState
{
    // �̱��� ����
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

    // ���� State Dictionary
    private readonly Dictionary<AnimalStateType, IAnimalState> animalStateDictionary = new Dictionary<AnimalStateType, IAnimalState>();

    // Dictionary�� ������ �߰�
    private AnimalState()
    {
        animalStateDictionary.Add(AnimalStateType.Idle, new AnimalIdleState());
        animalStateDictionary.Add(AnimalStateType.Move, new AnimalMoveState());
        animalStateDictionary.Add(AnimalStateType.RunAway, new AnimalRunAwayState());
        animalStateDictionary.Add(AnimalStateType.Dead, new AnimalDeadState());
    }

    // State�� ������
    public IAnimalState GetAnimalState(AnimalStateType stateType)
    {
        return animalStateDictionary[stateType];
    }
}
