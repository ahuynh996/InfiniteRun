using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

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
    private int maxHealth = 3;
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

    [Header("HUD")]
    [SerializeField]
    private Image[] hearts;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    [Header("Game Stats")]
    [SerializeField]
    [SyncVar(hook = nameof(OnHealthChange))]
    private int currentHealth = 3;
    [SerializeField]
    [SyncVar]
    private bool isTakingDmg;

    public override void OnStartLocalPlayer()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        canvas.enabled = true;
        hearts = new Image[maxHealth];
        hearts[0] = GameObject.FindGameObjectWithTag("Heart0").GetComponent<Image>();
        hearts[1] = GameObject.FindGameObjectWithTag("Heart1").GetComponent<Image>();
        hearts[2] = GameObject.FindGameObjectWithTag("Heart2").GetComponent<Image>();
    }
    #region Unused
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

    //void Flip() {
    //    facingRight = !facingRight;
    //    Vector3 Scaler = transform.localScale;
    //    Scaler.x *= -1;
    //    transform.localScale = Scaler;
    //}
    #endregion

    void Update()
    {
        if(!isLocalPlayer) { return;}
        
        if (Input.GetKeyDown(KeyCode.Space)) { CmdJump();}

        for (int i = 0; i < hearts.Length; i++)
        {
            if(hearts[i]!=null)
            {
                if (i < currentHealth) {
                    hearts[i].sprite = fullHeart;
                } else {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < maxHealth) {
                    hearts[i].enabled = true;
                } else {
                    hearts[i].enabled = false;
                }
            }
        }
    }

    [Command] void CmdJump() { RpcJump(); }
    [ClientRpc] void RpcJump() { Jump(); }
    void Jump() { rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce); }

    [ServerCallback]
    public void TakeDamage(int dmgAmount)
    {
        if(isTakingDmg) { return;}
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

    void OnHealthChange(int oldHealth, int newHealth)
    {
        StartCoroutine(ClientBlink());
        //SetHealthBar(newHealth);
    }

    IEnumerator ServerBlink()
    {
        yield return new WaitForSeconds(4f);
        isTakingDmg = false;
    }

    IEnumerator ClientBlink()
    {
        for (int i = 0; i < 20; ++i) {
            rend.material.color = Color.black;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
