using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> currentRespawns = new NetworkVariable<int>();
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        currentHealth.Value = 100;
        currentRespawns.Value = 2;
    }
    public void TakeDamage(int damage)
    {
        if (!IsServer)
        {
            return;
        }
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;
        if (currentHealth.Value <= 0)
        {
            if (currentRespawns.Value <= 0)
            {
                Death();
            }
            currentHealth.Value = 100;
            currentRespawns.Value--;
            Respawn();
        }
    }
    [ClientRpc]
    private void Respawn()
    {
        transform.position = Vector3.zero;
    }
    private void Death()
    {
        Destroy(gameObject);
    }
}
