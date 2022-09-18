using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/AircraftConfig", fileName = "Config")]
public class AircraftConfig : ScriptableObject
{
    public float EnginePower;
    public float PitchPower;
    public float RollPower;
}
