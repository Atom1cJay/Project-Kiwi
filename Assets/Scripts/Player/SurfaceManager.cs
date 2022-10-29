using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container of all information related to the surface the player is standing on
/// or in. Relevant for playing sounds, particles, and other stuff like that.
/// </summary>
public class SurfaceManager : MonoBehaviour
{
    public enum GroundSurface
    {
        Sand,
        HardSurface
    }
    public static GroundSurface currentSurface { get; private set; }

    public static void ChangeGroundSurface(GroundSurface surface)
    {
        currentSurface = surface;
    }

    public static bool IsBoopingWater()
    {
        return MovementInfo.instance.BoopingWater();
    }
}
