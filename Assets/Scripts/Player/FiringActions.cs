using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringActions : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] GameObject clientSingleHomingBulletPrefab;
    [SerializeField] GameObject serverSingleHomingBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;


    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
        playerController.onFireHomingEvent += FireHoming;
    }

    private void Fire(bool isShooting)
    {

        if (isShooting)
        {
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }

    private void FireHoming(bool isShooting, ulong ownerId)
    {

        if (isShooting)
        {
            ShootLocalHomingBullet(ownerId);
        }
    }

    [ServerRpc]
    private void ShootHomingBulletServerRpc(ulong ownerId)
    {
        GameObject HomingBullet = Instantiate(serverSingleHomingBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        HomingBullet.GetComponent<SingleHomingBullet>().ownerId = ownerId;
        Physics2D.IgnoreCollision(HomingBullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootHomingBulletClientRpc(ownerId);
    }

    [ClientRpc]
    private void ShootHomingBulletClientRpc(ulong ownerId)
    {
        if (IsOwner) return;
        GameObject HomingBullet = Instantiate(clientSingleHomingBulletPrefab,bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        HomingBullet.GetComponent<SingleHomingBullet>().ownerId = ownerId;
        Physics2D.IgnoreCollision(HomingBullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    private void ShootLocalHomingBullet(ulong ownerId)
    {
        GameObject HomingBullet = Instantiate(clientSingleHomingBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        HomingBullet.GetComponent<SingleHomingBullet>().ownerId = ownerId;
        Physics2D.IgnoreCollision(HomingBullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootHomingBulletServerRpc(ownerId);
    }
}
