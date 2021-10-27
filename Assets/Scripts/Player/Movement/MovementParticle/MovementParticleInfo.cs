using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementParticleInfo : ScriptableObject
{
    [SerializeField] MovementParticles vertBoost;
    [SerializeField] MovementParticles horizBoost;
    [SerializeField] MovementParticles landing;
    [SerializeField] MovementParticles accel;
    [SerializeField] MovementParticles sliding;
    [SerializeField] MovementParticles splash;

    public MovementParticles VertBoost { get { return vertBoost; } }
    public MovementParticles HorizBoost { get { return horizBoost; } }
    public MovementParticles Landing { get { return landing; } }
    public MovementParticles Accel { get { return accel; } }
    public MovementParticles Sliding { get { return sliding; } }
    public MovementParticles Splash { get { return splash; } }

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
        public float maxTimeBeforeDestroy; // 0 if no timer for destruction
        public bool destroyedByNewMove; // Particles disappear on new move
        [Header("Use Sparingly (Is Slow)")]
        public bool haltedByNewMove; // Particles stop spawning on new move
    }
}
