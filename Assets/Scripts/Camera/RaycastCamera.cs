using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour
{
    public GameObject player;
    public PlayerShooting playerShooting;
    RaycastHit hit;//variable que vamos a usar para hacer referencia al gameobject con el que estamos chocando

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))//botón izquierdo del ratón o dedo sobre móvil
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(camRay.origin, camRay.direction * 10, Color.yellow);
            RaycastHit hit;//variable que usamos para guardarnos la información del choque entre el rayo y el gameobject

            if (Physics.Raycast(camRay, out hit, 100))
            {
                //Tenemos que ver si estamos chocando con el suelo o con une enemigo
                if (hit.collider.gameObject.layer == 8)//floor
                {
                    player.GetComponent<PlayerMovement>().PlayerGO(hit.point);
                    Debug.Log("Floor");
                }
                else if (hit.collider.gameObject.layer == 9)//enemy
                {
                    playerShooting.Shoot();
                    player.GetComponent<PlayerMovement>().Turning(hit.point);
                    Debug.Log("Enemy");
                }
            }
        }     
    }
}
