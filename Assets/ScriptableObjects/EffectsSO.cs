using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsSO", menuName = "Scriptable Objects/EffectsSO")]
public class EffectsSO : ScriptableObject
{
    [SerializeField] public List<StatusEffect> effects;
}
