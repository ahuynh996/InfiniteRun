using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControllerMirror : NetworkBehaviour
{
    [SerializeField]
    private bool facingRight = true;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool alive = true;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float moveInput;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color hurtColor;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Renderer rend;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Transform groundCheck;

    [Header("Game Stats")]
    [SerializeField]
    [SyncVar(hook = nameof(OnHealthChange))]
    private int currentHealth = 10;
    [SerializeField]
    [SyncVar]
    private bool isTakingDmg;

    //void FixedUpdate()
    //{
    //    //if (!isLocalPlayer) {
    //    //    return;
    //    //}

    //    //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

    //    //moveInput = Input.GetAxisRaw("Horizontal");
    //    //rigidBody.velocity = new Vector2(moveInput * speed, rigidBody.velocity.y);

    //    //if (facingRight == false && moveInput > 0) {
    //    //    Flip();
    //    //} else if (facingRight == true && moveInput < 0) {
    //    //    Flip();
    //    //}
    //}

    void Update() {

        if(!isLocalPlayer) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdJump();
        }
    }
    [Command] void CmdJump(){ RpcJump();}
    [ClientRpc] void RpcJump() { Jump();}
    void Jump() { rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);}
    void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnHealthChange(int oldHealth, int newHealth) {
        StartCoroutine(ClientBlink());
        //SetHealthBar(newHealth);
    }

    [ServerCallback]
    public void TakeDamage(int dmgAmount)
    {
        if(isTakingDmg) {
            return;
        }
        isTakingDmg = true;
        currentHealth -= dmgAmount;
        //SetHealthBar(currentHealth);

        if (currentHealth <= 0)
        {
            alive = false;
            //FindObjectOfType<GameManager>().EndGame();
        }
        StartCoroutine(ServerBlink());
    }
    IEnumerator ServerBlink() {
        yield return new WaitForSeconds(4f);
        isTakingDmg = false;
    }

    IEnumerator ClientBlink() {
        for (int i = 0; i < 20; ++i) {
            rend.material.color = Color.black;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
