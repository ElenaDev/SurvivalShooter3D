using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;//daño que hacemos por disparo
    public float timeBetweenBullets = 0.15f;//tiempo entre disparos
    public float range = 100f; //longitud del raycast
    public LayerMask shootableMask;//capa de objetos a la que podemos disparar
    [KLVariable] public string krillVar;

    float timer;//timer para medir los tiempos entre disparos
    Ray shootRay;
    RaycastHit shootHit;//variable que hace referencia a la info de choque entre raycast y el gameobject
    LineRenderer lineRenderer;
    Light gunLight;
    float effectsDisplayTime = 0.2f;//variable que vamos a usar para determinar cuando tiempo van a estar visibles los efectos
    KLAudioSource klAudioSource;
    void Awake()//las referencias de las variables se suelen hacer siempre en el awake
    {
        klAudioSource = GetComponent<KLAudioSource>();
        klAudioSource.SetFloatVar(krillVar, 0);
        lineRenderer = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

       /* if(Input.GetMouseButtonDown(0) && timer >= timeBetweenBullets)
        {
            Shoot();
        }*/
        if(timer>=timeBetweenBullets*effectsDisplayTime)
        {
            lineRenderer.enabled = false;
            gunLight.enabled = false;
        }
        
    }
    public void Shoot()
    {
        timer = 0; //Reseteo el timer

        klAudioSource.Play();

        gunLight.enabled = true;//habilito la luz
        lineRenderer.enabled = true;//habilito el line renderer
        lineRenderer.SetPosition(0, transform.position);

        //Configuración del raycast
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast(shootRay, out shootHit, range, shootableMask))//Lanzamos el raycast
        {
            GameObject enemy = shootHit.collider.gameObject;//estoy cogiendo el gameobject con el que estoy chocando
            if(enemy.GetComponent<EnemyHealth>())//¿este gameobject tiene el componente EnemyHealth?
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damagePerShot, shootHit.point);
                lineRenderer.SetPosition(1, shootHit.point);//shootHit.point es el punto de impacto entre rayo y objeto
            }
        }
        else
        {
            //Segunda posición del lineRenderer cuando no hemos dado al enemigo
            lineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

}
