# NetworkingAssignment
 
## Limited Respawn (11) and Death (9)
[Health.cs](https://github.com/mlynar132/NetworkingAssignment/blob/main/Assets/Scripts/Player/Health.cs)

When player's health reaches zero their health gets reseted and they use a respawn on the server and theirs location get's updated on all the clients and server.
When they run out of respawns they get destroyed I disable the input on the client and the rest is taken care of by NetworkBehaviour.

## Homing Missile (12)
[SingleHomingBullet.cs](https://github.com/mlynar132/NetworkingAssignment/blob/main/Assets/SingleHomingBullet.cs), [PlayerController.cs](https://github.com/mlynar132/NetworkingAssignment/blob/main/Assets/Scripts/Player/PlayerController.cs), [FiringActions.cs](https://github.com/mlynar132/NetworkingAssignment/blob/main/Assets/Scripts/Player/FiringActions.cs)

the SingleHomingBullet is almost like regular bullet besieds the fact that it gets the id of player that spawns it and does some calculations to figure out the non-owner player that it's the closest to facing forward and adjust it self. Aditionally it has a one second delay before it actually starts moving.
