using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementParticleInfo : ScriptableObject
{
    [SerializeField] MovementParticles walking;
    [SerializeField] MovementParticles vertBoost;
    [SerializeField] MovementParticles horizBoost;
    [SerializeField] MovementParticles landing;
    [SerializeField] MovementParticles landingImpact;
    [SerializeField] MovementParticles accel;
    [SerializeField] MovementParticles sliding;
    [SerializeField] MovementParticles slidingBoost;
    [SerializeField] MovementParticles slidingTracks;
    [SerializeField] MovementParticles splash;
    [SerializeField] MovementParticles swimCircles;
    [SerializeField] MovementParticles swimCirclesEdges;
    [SerializeField] MovementParticles stars;
    [SerializeField] MovementParticles walkingSmoke;
    [SerializeField] MovementParticles waterWalking;

    public MovementParticles Walking { get { return walking; } }
    public MovementParticles VertBoost { get { return vertBoost; } }
    public MovementParticles HorizBoost { get { return horizBoost; } }
    public MovementParticles Landing { get { return landing; } }
    public MovementParticles LandingImpact { get { return landingImpact; } }
    public MovementParticles Accel { get { return accel; } }
    public MovementParticles Sliding { get { return sliding; } }
    public MovementParticles SlidingBoost { get { return slidingBoost; } }
    public MovementParticles SlidingTracks { get { return slidingTracks; } }
    public MovementParticles Splash { get { return splash; } }
    public MovementParticles SwimCircles { get { return swimCircles; } }
    public MovementParticles SwimCirclesEdges { get { return swimCirclesEdges; } }
    public MovementParticles Stars { get { return stars; } }
    public MovementParticles WalkingSmoke { get { return walkingSmoke; } }
    public MovementParticles WaterWalking { get { return waterWalking; } }

    // Singleton Handling
    static MovementParticleInfo _instance;
    public static MovementParticleInfo Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<MovementParticleInfo>("MovementParticleInfo");
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class MovementParticles
    {
        public GameObject particles;
        public bool staysWithPlayer;
        public float maxTimeBeforeDestroy; // 0 (or negative) if no timer for destruction
        [Header("Use Sparingly (Is Slow)")]
        public bool haltedByNewMove; // Particles stop spawning on new move
        public float timeAfterHaltToDestroy; // If halt takes place, how long after should destroy happen (negative = never).
    }
}
