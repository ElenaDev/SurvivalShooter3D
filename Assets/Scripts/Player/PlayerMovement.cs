using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//incluir cuando queremos cargar escenas, ver cual es la escena actual...etc

public class PlayerMovement : MonoBehaviour
{
    public int speed;
    public float smoothing;//variable de velocidad para movimiento con transfom
    public LayerMask layerFloor;

    Animator anim;
    Rigidbody rb;

    Vector3 movement;

    void Start()
    {
        //Nos guardamos las referencias a los componentes en las variables, de esta forma es mucho más óptimo que usar el GetComponent cada
        //vez que queramos acceder al componente
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //GetAxis devuelve valores entre -1 y 1 (usando valores intermedios)
        //GetAxisRaw devuelve 3 valores: -1, 0 y 1
        // float h = Input.GetAxisRaw("Horizontal"); // teclas AD
        //float v = Input.GetAxisRaw("Vertical"); // teclas WS
        //Move(h, v);//le estoy pasando a la función Move los valores de h y v
        // Turning();//Llamada a la función que hace el giro del personaje
       
    }
    void Move(float _h, float _v)
    {
        movement.Set(_h, 0, _v);
        movement.Normalize();//normalizamos el vector para que su módulo valga 1 y siempre tenga la misma velocidad de movimiento
        rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));

        bool isMoving = _h != 0 || _v != 0; //isMoving va a ser true si _h es distinto de cero o _v es distinto de cero

        anim.SetBool("IsMoving", isMoving);

        /*
        if(_h!=0 || _v!=0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        */
    }
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(camRay.origin, camRay.direction * 10, Color.yellow);
        RaycastHit hit;//variable que usamos para guardarnos la información del choque entre el rayo y el gameobject

        if(Physics.Raycast(camRay, out hit, 100, layerFloor))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0;//para asegurarme que no hay movimiento ni cosas raras a lo largo del eje y
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRotation);
        }
    }
    public void PlayerGO(Vector3 point)
    {
        Turning(point);
        StopCoroutine("MovementPlayer");
        StartCoroutine("MovementPlayer", point);
    }
    IEnumerator MovementPlayer(Vector3 target)
    {
        while(Vector3.Distance(transform.position, target) > 0.05f)
        {
            //NOS ESTAMOS MOVIENDO
            anim.SetBool("IsMoving", true);
            transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
            yield return null;
        }
        //NOS HEMOS DEJADO DE MOVER PORQUE HEMOS LLEGADO A NUESTRO DESTINO
        anim.SetBool("IsMoving", false);
    }
    public void Turning(Vector3 point)
    {
        Vector3 playerToMouse = point - transform.position;
        playerToMouse.y = 0;//para asegurarme que no hay movimiento ni cosas raras a lo largo del eje y
        Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
        rb.MoveRotation(newRotation);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");//para cargar una escena
    }
}
