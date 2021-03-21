using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Incluir para progamar con elementos de la interfaz
using KrillAudio.Krilloud;

public class PlayerHealth : MonoBehaviour
{
    [Header ("Health Var")]
    public int startingHealth = 100; //vida máxima
    public int currentHealth; //vida actual
    [Header("UI")]
    public Slider slider;//slider de la vida
    public Image damageImage;//imagen que vamos a cambiar el canal alpha cuando el enemigo le quite vida al player

    public float flashSpeed = 5;//velocidad a la que damageImage va a desaparecer
    public Color flashColour = new Color(1, 0, 0, 0.4f);//color rojo que va a aparecer cuando le quiten vida al player

    [KLVariable] public string krillVar;

    Animator anim;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    KLAudioSource klAudioSource;

    private void Awake()
    {
        klAudioSource = GetComponent<KLAudioSource>();

        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;
    }

    void Update()
    {
        if (damaged) damageImage.color = flashColour;
        else damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        damaged = false;
    }

    //Función que vamos a llamar desde el script de ataque del enemigo
    public void TakeDamage(int amount)
    {
        klAudioSource.SetFloatVar(krillVar, 1);
        klAudioSource.Play();
        damaged = true;
        currentHealth -= amount;
        slider.value = currentHealth;

        if(currentHealth <=0 && !isDead)
        {
            Death();
        }
    }
    void Death()
    {
        isDead = true;
        klAudioSource.SetFloatVar(krillVar, 2);
        klAudioSource.Play();
        anim.SetTrigger("Death");

        playerMovement.enabled = false;//deshabilitamos componente para que no se pueda mover
        playerShooting.enabled = false;//deshabilitamos componente para que no pueda disparar
    }
}
