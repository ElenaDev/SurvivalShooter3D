using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//INCLUIR SIEMPRE QUE VAYAMOS A TRABAJAR CON NAVMEHS

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;//variable que va a hacer referencia al componente NavMeshAgent
    Animator anim;
    EnemyHealth enemyHealth;

    void Awake()
    {
        //if(GetComponent<Animator>())
        if (GetComponent<Animator>()!=null)
        {
            anim = GetComponent<Animator>();
        }

        if (GetComponent<NavMeshAgent>())
        {
            agent = GetComponent<NavMeshAgent>();
        }

        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (enemyHealth.isDead) return;

        agent.SetDestination(player.transform.position);

        Animating();
        //El tiempo que pasa entre update no es constante y depende, por ejemplo, de si tu ordenador es polluo o no

        //El update se llama una vez por frame

        //En el update vamos a meter inputs, actualización de variables etc
    }
    void Animating()
    {
        float h = agent.velocity.x;
        float v = agent.velocity.z;
        bool isMoving = h != 0 || v != 0; //isMoving va a ser true si _h es distinto de cero o _v es distinto de cero
        anim.SetBool("IsMoving", isMoving);
    }
    private void FixedUpdate()
    {
        //Por defecto el fixed es llamado cada 0.02s (pero se puede cambiar)

        //SIEMPRE vamos a meter movimiento de físicas

    }
    private void LateUpdate()
    {
        //Lo último que mira en el frame
    }
}
