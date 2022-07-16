using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHp; // 최대 체력
    public float maxHunger; // 최대 배고픔
    public float decreaseHp; // 체력 감소량
    public float decreaseHunger; // 배고픔 감소량
    public float decreaseTime; // 감소 시간

    // 옵저버 패턴
    public event Action<float> changeHpEvent;
    public event Action<float> changeHungerEvent;

    private WaitForSeconds waitDecreaseTime; // 감소 시간

    private float currentHp; // 현재 체력
    private float currentHunger; // 현재 배고픔

    // 싱글톤 패턴
    private static PlayerStatus instance;
    public static PlayerStatus Instance
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

    // 체력 감소
    public void AddHp(float addValue)
    {
        currentHp = Mathf.Clamp(currentHp + addValue, 0, maxHp);

        changeHpEvent(currentHp / maxHp);
    }

    // 배고픔 감소
    public void AddHunger(float addValue)
    {
        currentHunger = Mathf.Clamp(currentHunger + addValue, 0, maxHunger);

        changeHungerEvent(currentHunger / maxHunger);
    }

    // 시간이 지나면 배고픔 또는 체력 감소
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
