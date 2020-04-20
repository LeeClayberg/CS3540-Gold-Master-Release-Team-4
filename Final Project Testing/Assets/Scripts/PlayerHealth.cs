using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 1000;
    public int currentHealth;

    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    private void Update()
    {
        healthSlider.value = currentHealth;
    }

    //Decreases the health of the player
    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }
        if (currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    //Player Dies
    void PlayerDies()
    {
        if (!GameController.isGameOver)
        {
            healthSlider.value = 0;
            NewCharacterController gm = GameObject.FindGameObjectWithTag("Player").GetComponent<NewCharacterController>();
            gm.PlayerDied();
        }
    }
}
