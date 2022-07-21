using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHp; // �ִ� ü��
    public float maxHunger; // �ִ� �����
    public float decreaseHp; // ü�� ���ҷ�
    public float decreaseHunger; // ����� ���ҷ�
    public float decreaseTime; // ���� �ð�

    // ������ ����
    public event Action<float> ChangeHpEvent;
    public event Action<float> ChangeHungerEvent;

    private WaitForSeconds waitDecreaseTime; // ���� �ð�

    private float currentHp; // ���� ü��
    private float currentHunger; // ���� �����

    // �̱��� ����
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);

            return;
        }

        instance = this;
    }

    private void Start()
    {
        currentHp = maxHp;
        currentHunger = maxHunger;

        waitDecreaseTime = new WaitForSeconds(decreaseTime);

        StartCoroutine(DecreaseHunger());
    }

    // ü�� ����
    public void AddHp(float addValue)
    {
        currentHp = Mathf.Clamp(currentHp + addValue, 0, maxHp);

        ChangeHpEvent(currentHp / maxHp);
    }

    // ����� ����
    public void AddHunger(float addValue)
    {
        currentHunger = Mathf.Clamp(currentHunger + addValue, 0, maxHunger);

        ChangeHungerEvent(currentHunger / maxHunger);
    }

    // �ð��� ������ ����� �Ǵ� ü�� ����
    private IEnumerator DecreaseHunger()
    {
        while (true)
        {
            yield return waitDecreaseTime;

            if (currentHunger == 0)
            {
                AddHp(-decreaseHp);
            }
            else
            {
                AddHunger(-decreaseHunger);
            }
        }
    }
}
