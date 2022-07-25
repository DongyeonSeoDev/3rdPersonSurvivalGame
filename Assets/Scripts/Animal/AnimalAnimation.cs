using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalAnimationType // 애니메이션 타입
{ 
    Idle, Walk, Run
}

// 동물 애니메이션 관리
public class AnimalAnimation : MonoBehaviour
{
    // 애니메이션 Hash값 Dictionary
    private readonly Dictionary<AnimalAnimationType, int> hashAnimationDictionary = new Dictionary<AnimalAnimationType, int>();

    private Animator animator;

    private AnimalAnimationType currentAnimalAnimationType; // 현재 애니메이션 타입

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Dictionary에 Hash값 넣음
        foreach (AnimalAnimationType animationType in Enum.GetValues(typeof(AnimalAnimationType)))
        {
            hashAnimationDictionary.Add(animationType, Animator.StringToHash(animationType.ToString()));
        }
    }

    // 애니메이션 실행
    public void SetAnimalAnimation(AnimalAnimationType animationType)
    {
        if (animationType != currentAnimalAnimationType)
        {
            currentAnimalAnimationType = animationType;
            animator.SetTrigger(hashAnimationDictionary[animationType]);
        }
    }
}
