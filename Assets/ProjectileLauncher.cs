using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] float timeBetweenLaunch;
    [SerializeField] GameObject projectile;
    [SerializeField] float launchAngle, launchSpeed;


    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("launchProjectile", 0f, timeBetweenLaunch);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void launchProjectile()
    {
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        ArcProjectile ap = proj.GetComponentInChildren<ArcProjectile>();

        ap.launchProjectileWithAngle(player.transform.position, launchAngle);
    }

}
