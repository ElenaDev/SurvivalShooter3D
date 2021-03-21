using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Function1();
       // StopCoroutine("MyFirstCoroutine");
        StartCoroutine("MyFirstCoroutine");
    }
    void Function1()
    {
        Debug.Log("I am a Jedi");
    }
    IEnumerator MyFirstCoroutine()
    {
        Debug.Log("Hola");
        yield return new WaitForSeconds(3);//se espera 3 segundos
        Debug.Log("Soy un Jedi");
    }
}
