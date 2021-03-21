using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public WaveManager waveManager;
    public GameObject[] enemies;
    public float spawnTime = 3;
    public Transform[] spawnPoints;
    public Transform parentEnemies;
    //coger las posiciones de salida de los enemigos a través del padre (mirando en los hijos)
    Transform[] spawnPointsChildren;

    int enemiesWave;
    int enemiesCreated;

    void Start()
    {
        enemiesWave = waveManager.numWave * 3;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

   void Spawn()
   {
        //si el numero de enemigos que hemos creado en escena es igual que el número de enemigos que tenemos en esta oleada
        if (enemiesCreated == enemiesWave) return;

        //Primero tenemos que comprobar que el player sigue vivo
        //Posición aleatoria
        int positionEnemy = Random.Range(0, spawnPoints.Length);
        //Enemigo aleatorio
        int enemy = Random.Range(0, enemies.Length);
        //Creamos clone del enemigo (prefab)
        GameObject enemyClone = Instantiate(enemies[enemy], spawnPoints[positionEnemy].position, spawnPoints[positionEnemy].rotation);
        enemyClone.transform.SetParent(parentEnemies);//pongo al enemigo que acabo de crear en escena (clone del prefab) como
        //hijo de parentEnemies
        enemiesCreated++;
   }
    public void AmITheLastEnemy()
    {
        if (enemiesCreated != enemiesWave) return;//si no he creado todavía todos los enemigos de la ronda, me salgo de la función
        if(parentEnemies.childCount > 0)//si al menos tiene un hijo
        {
            for (int i = 0; i < parentEnemies.childCount; i++)
            {
                if (!parentEnemies.GetChild(i).GetComponent<EnemyHealth>().isDead) return;
            }
        }
        //HEMOS ACABADO LA RONDA
        waveManager.NextWave();
    }


    //Forma alternativa de coger las posiciones. NO LA ESTAMOS USANDO
    void GetPositions()
    {
        int nChildren = transform.childCount;//me devuelve cuántos hijos tiene el gameobject
        spawnPointsChildren = new Transform[nChildren]; //estamos indicando el tamaño del array
        for (int i = 0; i< nChildren-1;i++)
        {
            spawnPointsChildren[i] = transform.GetChild(i).transform;
        }
    }
}
