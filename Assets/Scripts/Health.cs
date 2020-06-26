using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 3f;
    public float currHealth = 0f;
    public bool alive = true;
    private GameObject player;
    private Material white;
    private Material default;

    void Start()
    {
        alive = true;
        currHealth = maxHealth;
        player = GameObject.Find("Player");

    }


    public void TakeDamage(float dmgAmount)
    {
        if(!alive)
        {
            return;
        }

        if(currHealth <= 0)
        {
            currHealth = 0;
            alive = false;
            FindObjectOfType<GameManager>().EndGame();
        }
        else
        {
            currHealth -= dmgAmount;
            SetHealthBar();
        }

        currHealth -= dmgAmount;
        SetHealthBar();
        Invoke("playerBlinkWhenDamaged", .1f);
        Debug.Log("Current Health " + currHealth);
    }

    void playerBlinkWhenDamaged()
    {
        if (player.active)
        {
            player.SetActive(false);
        }
        else
        {
            player.SetActive(true);
        }

    }

    // Update is called once per frame
    void SetHealthBar()
    {
        float myHealth = currHealth / maxHealth;
    }
}
