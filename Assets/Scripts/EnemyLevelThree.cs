using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelThree : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    public float speed;

    public float projectileSpeed;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        InvokeRepeating("ShootAtPlayer", 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void ShootAtPlayer()
    {
        Vector3 shootDirection = (player.transform.position - transform.position).normalized;
        GameObject instantiatedProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        Rigidbody projectileRb = instantiatedProjectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
    }
}
