using UnityEngine;
using Mirror;

//NetworkBehaivour requires NetWorkIdentity(For remote call privilege and state synchronization)
public class Player : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private GameObject prefab;

    /**Addition to auto syncing, this all of the other's player's character's to synchornzie their health 
    to the corresponding copy of new player right after joining I think**/
    [SyncVar(hook=nameof(OnHealthChange))]
    private int currentHealth;

    private void FixedUpdate() {

        if (isServer) {
            //do some validation here I think
        }
        //Dont wanna do something to your local copy of other player's character by accident lol
        if (isLocalPlayer) {
            //NetworkTransform Component will synch the transform I think
            //this happen immediately in the client!
            float movement = Input.GetAxis("Horizontal");
            GetComponent<Rigidbody2D>().velocity = new Vector2(movement * speed, 0.0f);
            //CmdShoot();
        }

    }

    private void TakeDamage(int amount) 
    {
        //Only let the server handle the game rule
        if (!isServer) {
            return;
        }


        //this might be delayed due to LAG
        //this can create a situation where a player's character is dead in the server's point of view
        //, but not dead in the client's point of view lol

        //there is a secret ClientRpc call here I think, to sync the health. Syncvar makes our lives easier
        //invoke OnHeathChange hook
        currentHealth -= amount;
        //Play hurt animation on the server's local client's instance here
        
        //let the server decides whether if a player's character'death is legitmate
        if (currentHealth <= 0) {
            //let the server decides whether if a player's character shoud be respawned(not from the hook!)
            RpcRespawn();
        }
    }
    /**This method is called when the SyncVar currentHealth(remote clients' local copy) is changed by the server**/
    private void OnHealthChange(int oldHealth, int newHealth) {
        
        if (newHealth <= 0) {
            //Play death animation
            //dont respawn here
        }
        else {
            //Play hurt animation
        }
    }

    /**This method is invoke on the server's local client's instance of the player's instance
    The server replicates the bullet among all clients.
    If the connection is lost, no bullet will show up*/
    [Command]
    void CmdShoot()
    {
        //run on the server!
        //validate the player's character's heath to make sure the player's request is still legitmate
        //the player might doesnt know the character is dead due to lag
        if(currentHealth > 0) 
        {
            GameObject bullet = Instantiate(this.prefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 1f;
            NetworkServer.Spawn(bullet);
            Destroy(bullet, 1.0f);
        }
    }

    /**This method is invoke on ALL of the clients' local instance of the instance which called RpcRespawn previouslyin the server
    The client includes server's local client's instance.**/
    [ClientRpc]
    void RpcRespawn() {
        //Respawn()
        currentHealth = maxHealth;
    }

    [Command]
    void CmdX(string helloMessage) {
        //X(helloMessage) -> this will causes an extra call
        RpcX(helloMessage);
    }

    [ClientRpc]
    void RpcX(string helloMessage) 
    {
        //call X(helloMessage) here
    }
}