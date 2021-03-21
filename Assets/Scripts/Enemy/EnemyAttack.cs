using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;//tiempo entre ataques, lo usamos para controlar que el enemigo no ataque de forma continuada
    public int attackDamage = 10;//puntos de vida que le va a quitar al player

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;//para saber si el player está en rango, para atacarle
    float timer;//contador para manejar el tiempo entre ataques

    private void Awake()//las referencias a componentes y gamobjects se suelen hacer en awake
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;//contador de tiempo

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth>=0)
        {
            Attack();
        }
    }
    void Attack()
    {
        timer = 0;

        if(playerHealth.currentHealth >0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
