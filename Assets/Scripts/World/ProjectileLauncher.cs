using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] float timeBetweenLaunch;
    [SerializeField] GameObject launchingPoint, projectile;
    [SerializeField] float launchAngle, launchSpeed, range;
    [SerializeField] bool rotateTorwardsPlayer = false;
    [SerializeField] LayerMask validGround;
    [SerializeField] Sound launchSound;


    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("launchProjectile", 0f, timeBetweenLaunch);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (rotateTorwardsPlayer && player != null)
        {
            Vector3 rot = transform.localEulerAngles;
            transform.LookAt(player.transform.position);
            transform.localEulerAngles = new Vector3(rot.x, transform.localEulerAngles.y, rot.z);
        }
    }

    void launchProjectile()
    {
        if (range == 0 || Vector3.Distance(player.transform.position, launchingPoint.transform.position) <= range)
        {

            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, (player.transform.localScale.y / 2f) + 0.05f, validGround))
            {
                GameObject proj = Instantiate(projectile, launchingPoint.transform.position, Quaternion.identity);
                ArcProjectile ap = proj.GetComponentInChildren<ArcProjectile>();

                ap.launchProjectileWithAngle(player.transform.position, launchAngle);
                launchSound.Play(transform);
            }

        }
    }

}
