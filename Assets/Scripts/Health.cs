using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public int maxHealth = 3;
    public int currHealth;
    public bool alive = true;
    private GameObject player;

    Renderer rend;
    Color defaultColor;
    Color hurtColor;
    private bool isTakingDmg = false;
 
    void Start()
    {
        alive = true;
        currHealth = maxHealth;
        player = GameObject.Find("Player");

        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
        hurtColor = Color.black;
    }

    public void TakeDamage(int dmgAmount)
    {

        currHealth -= dmgAmount;
        SetHealthBar();

        if (currHealth <= 0)
        {
            alive = false;
            FindObjectOfType<GameManager>().EndGame();
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTakingDmg && other.tag == "Obstacle")
        {
            StartCoroutine(Blink());
            TakeDamage(1);
        }
    }

    IEnumerator Blink()
    {
        isTakingDmg = true;
        for (int i = 0; i < 20; ++i)
        {
            rend.material.color = hurtColor;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = defaultColor;
            yield return new WaitForSeconds(0.1f);
        }
        isTakingDmg = false;
    }

    // Update is called once per frame
    void SetHealthBar()
    {
        //float myHealth = currHealth / maxHealth;
        Debug.Log("Current Health " + currHealth);
    }
}
