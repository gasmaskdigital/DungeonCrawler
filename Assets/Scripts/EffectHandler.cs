using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StatusEffect 
{
    public string name;
    public int intensity;
    public float duration; //How long the effect lasts in seconds. Make 0 or less for instant effects
    public float timeRemaining;

    public StatusEffect(string name, int intensity, float duration) 
    {
        this.name = name;
        this.intensity = intensity;
        this.duration = duration;
        this.timeRemaining = duration;
    }

    public StatusEffect(string name,int intensity, float duration, float timeRemaining)
    {
        this.name = name;
        this.intensity = intensity;
        this.duration = duration;
        this.timeRemaining = timeRemaining;
    }
}

public class EffectHandler : MonoBehaviour
{
    [SerializeField] public List<StatusEffect> activeEffects;
    private static List<StatusEffect> playerEffects;

    private void Start()
    {
        if (gameObject.CompareTag("Player") && playerEffects != null) activeEffects = playerEffects;
        else activeEffects = new();
    }
    private void Update()
    {
        if (activeEffects.Count >= 1)
        {
            List<StatusEffect> currentEffects = new();
            copyList(activeEffects, currentEffects);
            for (int i = 0; i < currentEffects.Count; i++)
            {
                StatusEffect effect = activeEffects[i];
                effect.timeRemaining = activeEffects[i].timeRemaining - Time.deltaTime;
                if (effect.timeRemaining <= 0) removeEffect(activeEffects[i]);
                else activeEffects[i] = effect;
            }
        }
    }

    public void addEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void removeEffect(StatusEffect effect)
    {
        activeEffects.Remove(effect);
    }


    public void copyList<T>(List<T> source, List<T> target)
    {
        target.Clear();
        foreach (var item in source) target.Add(item);
    }

    private void OnDestroy()
    {
        if(gameObject.CompareTag("Player")) playerEffects = activeEffects;
    }
}
