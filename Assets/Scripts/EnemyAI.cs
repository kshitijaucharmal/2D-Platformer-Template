using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float followRange = 5f;
    [SerializeField] private float stopFollowRange = 7f;

    private Transform player;
    private Vector3 startPosition;
    private bool isFollowingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < followRange)
        {
            isFollowingPlayer = true;
        }
        else if (distanceToPlayer > stopFollowRange)
        {
            isFollowingPlayer = false;
        }

        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            ReturnToStart();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * speed * Time.deltaTime;
    }

    void ReturnToStart()
    {
        if (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
