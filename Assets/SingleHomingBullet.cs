using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHomingBullet : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] int bulletSpeed = 10;
    [SerializeField] int rotationSpeed = 200;
    [SerializeField] float lifeSpan = 2;
    [SerializeField] float freezeTime = 2;
    
    [HideInInspector] public ulong ownerId;
    
    private bool isFreezed = true;
    private GameObject owner;
    private GameObject target;
    private List<GameObject> players = new List<GameObject>();
    private ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();
    void Start()
    {
        PlayerController[] playersTemp;
        playersTemp = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in playersTemp)
        {
            if (player.NetworkObjectId == ownerId)
            {
                owner = player.gameObject;
                continue;
            }
            players.Add(player.gameObject);
        }
        Invoke("DelayedStart", freezeTime);
        Invoke("KillBullet", lifeSpan);
    }
    void FixedUpdate()
    {
        if (!isFreezed && players.Count > 0)
        {
            target = null;
            foreach (GameObject player in players)
            {
                if (player == null)
                {
                    continue;
                }
                Vector3 vectorToPlayer = player.transform.position - transform.position;
                float distanceToPlayer = vectorToPlayer.magnitude;
                Vector3 directionToPlayer = vectorToPlayer / distanceToPlayer;
                if (Vector3.Dot(directionToPlayer, transform.up) < 0)
                {
                    continue;
                }
                if (target == null)
                {
                    target = player;
                    continue;
                }
                Vector3 vectorToTarget = target.transform.position - transform.position;
                float distanceToTarget = vectorToTarget.magnitude;
                Vector3 directionToTarget = vectorToTarget / distanceToTarget;
                if (Vector3.Dot(directionToPlayer, transform.up) > Vector3.Dot(directionToTarget, transform.up))
                {
                    target = player;
                }
            }
            if (target != null)
            {
                Vector3 vectorToTarget1 = target.transform.position - transform.position;
                float distanceToTarget1 = vectorToTarget1.magnitude;
                Vector3 directionToTarget1 = vectorToTarget1 / distanceToTarget1;
                if (Vector3.Dot(directionToTarget1, transform.up)>0.96)
                {
                    return;
                }
                float wedgeProduct = directionToTarget1.x * transform.up.y - directionToTarget1.y * transform.up.x;
                wedgeProduct = Mathf.Sign(wedgeProduct);
                transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime * -wedgeProduct);
                rb.velocity = transform.up * bulletSpeed;
            }
        }
    }

    void DelayedStart()
    {
        rb.velocity = transform.up * bulletSpeed;
        isFreezed = false;
    }
    void KillBullet()
    {
        if (gameObject) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        KillBullet();
    }
}
