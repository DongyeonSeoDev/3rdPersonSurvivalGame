using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalAnimationType // �ִϸ��̼� Ÿ��
{ 
    Idle, Walk, Run
}

// ���� �ִϸ��̼� ����
public class AnimalAnimation : MonoBehaviour
{
    // �ִϸ��̼� Hash�� Dictionary
    private readonly Dictionary<AnimalAnimationType, int> hashAnimationDictionary = new Dictionary<AnimalAnimationType, int>();

    private Animator animator;

    private AnimalAnimationType currentAnimalAnimationType; // ���� �ִϸ��̼� Ÿ��

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Dictionary�� Hash�� ����
        foreach (AnimalAnimationType animationType in Enum.GetValues(typeof(AnimalAnimationType)))
        {
            hashAnimationDictionary.Add(animationType, Animator.StringToHash(animationType.ToString()));
        }
    }

    // �ִϸ��̼� ����
    public void SetAnimalAnimation(AnimalAnimationType animationType)
    {
        if (animationType != currentAnimalAnimationType)
        {
            currentAnimalAnimationType = animationType;
            animator.SetTrigger(hashAnimationDictionary[animationType]);
        }
    }
}
