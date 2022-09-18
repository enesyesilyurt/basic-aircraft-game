using UnityEngine;

public class Aircraft
{
    public float EnginePower;
    public float PitchPower;
    public float RollPower;

    public Aircraft(AircraftConfig config)
    {
        EnginePower = config.EnginePower;
        PitchPower = config.PitchPower;
        RollPower = config.RollPower;
    }
}
