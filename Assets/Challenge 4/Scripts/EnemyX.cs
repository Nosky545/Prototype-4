using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float enemySpeed = 50f;
    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private ParticleSystem Frustration;

    // Start is called before the first frame update
    void Start()
    {
        Frustration = GameObject.Find("Frustration").GetComponent<ParticleSystem>();
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(enemySpeed * Time.deltaTime * lookDirection);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.CompareTag("Enemy Goal"))
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.CompareTag("Player Goal"))
        {
            Destroy(gameObject);
            Frustration.Play();
        }

    }

}
