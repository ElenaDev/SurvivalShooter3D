using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;
public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int startingHealth = 100;//vida máxima
    public int currentHealth;//vida actual
    public float sinkSpeed;//velocidad a la que se va a "hundir" el enemigo cuando muera
    public int scoreValue = 10;//puntos que va a dar el enemigo al jugador cuando muera
    public bool isDead;//me dice si el enemigo está muerto
    public GameObject hitParticles;//Sistema de partículas que vamos a habilitar cuando reciba disparo del player
    [KLVariable] public string klVar;

    KLAudioSource klAudioSource;
    Animator anim;
    ScoreManager scoreManager;
   // CapsuleCollider capsuleCollider;
    bool isSinking;//me dice si el enemigo se está hundiendo
    EnemyManager enemyManager;

    void Awake()
    {
        klAudioSource = GetComponent<KLAudioSource>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        anim = GetComponent<Animator>();
       //capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        if(isSinking == true)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        //if (isDead == true)
        //{
        //return
        //}
        if (isDead) return;//return hace que salgamos de la función y no sigamos mirando el resto de líneas de código

        //curentHealth = currentHealth - amount;

        klAudioSource.SetFloatVar(klVar, 0);
        klAudioSource.Play();

        ShowHitParticles();

        currentHealth -= amount;

        //Habilitar partículas

        if(currentHealth <=0)
        {
            Death();
        }
    }
    void ShowHitParticles()
    {
        for(int i=0;i<hitParticles.transform.childCount;i++)
        {
            hitParticles.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
    }
    void Death()
    {
        klAudioSource.SetFloatVar(klVar, 1);
        klAudioSource.Play();
        scoreManager.UpdateScore(scoreValue);//llamamos a la función UpdateScore de la clase ScoreManager para actualizar puntos
        isDead = true;
        enemyManager.AmITheLastEnemy();//para comprobar si es el último enemigo
      //  capsuleCollider.isTrigger = true;//ponemos el collider modo trigger porque quiero que el enemigo atraviese el suelo
        anim.SetTrigger("Death");
    }

    //Función pública porque la vamos a llamar desde evento en la animación
    public void StartSkinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        isSinking = true;
        //GetComponent<Rigidbody>().isKinematic = true;
        //SUBIR PUNTUACIÓN
        Destroy(gameObject, 2);
    }
}
