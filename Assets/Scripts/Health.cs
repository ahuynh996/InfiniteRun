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


    void Start()
    {
        alive = true;
        currHealth = maxHealth;

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

        currHealth -= dmgAmount;
        SetHealthBar();
        Debug.Log("Current Health " + currHealth);
    }
    // Update is called once per frame
    void SetHealthBar()
    {
        float myHealth = currHealth / maxHealth;
    }
}
