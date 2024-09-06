using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float projectileSpeed;
    public float smashSpeed;
    public float jumpSpeed;

    private Rigidbody playerRb;

    private GameObject focalPoint;

    public bool hasPowerup;
    public bool hasGun;
    public bool hasSmash;

    private float powerupStrength = 15f;

    public GameObject powerupIndicator;
    public GameObject gunIndicator;
    public GameObject smashIndicator; 

    public GameObject projectile;

    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 1f;
    private Vector3 explosionOrigin;
    private ParticleSystem explosionFx;

    private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("Focal Point");
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        explosionFx = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float fowardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(fowardInput * speed * focalPoint.transform.forward);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        gunIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        smashIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < 1 && transform.position.y > 0)
        {
            playerRb.AddForce(jumpSpeed * focalPoint.transform.up, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.SetActive(true);
        }

        if (other.CompareTag("Gun"))
        {
            hasGun = true;
            Destroy(other.gameObject);
            StartCoroutine(GunCountdownRoutine());
            gunIndicator.SetActive(true);
            InvokeRepeating("Shoot", 0, 1);
        }

        if (other.CompareTag("Smash"))
        {
            hasSmash = true;
            Destroy(other.gameObject);
            StartCoroutine(SmashCountdownRoutine());
            smashIndicator.SetActive(true);
            InvokeRepeating("Smash", 0, 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void Shoot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (hasGun)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
            GameObject target = enemies[i];
            Vector3 shootDirection = (target.transform.position - transform.position).normalized;
            GameObject instantiatedProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
            Rigidbody projectileRb = instantiatedProjectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
            }
        }
    }

    private void Smash()
    {
        explosionOrigin = transform.position;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (hasSmash)
        {
            playerAnim.SetTrigger("Smash");
            explosionFx.Play();

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject target = enemies[i];
                Rigidbody enemyRb = target.GetComponent<Rigidbody>();
                enemyRb.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    IEnumerator GunCountdownRoutine()
    {
        yield return new WaitForSeconds(3);
        hasGun = false;
        gunIndicator.SetActive(false);
    }

    IEnumerator SmashCountdownRoutine()
    {
        yield return new WaitForSeconds(2);
        hasSmash = false;
        smashIndicator.SetActive(false);
    }
}
