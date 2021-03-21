using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject waveText;
    public int numWave = 1;
    public bool newGame;

    public EnemyManager enemyManager;
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;

    private void Awake()
    {
        if(newGame) PlayerPrefs.SetInt("NumWave", 1);
        else LoadData();
    }
    IEnumerator Start()
    {
        waveText.GetComponent<Text>().text = "Wave " + numWave;
        yield return new WaitForSeconds(2);
        waveText.SetActive(false);
        enemyManager.enabled = true;
        playerMovement.enabled = true;
        playerShooting.enabled = true;
    }

    void Update()
    {
        
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("NumWave", numWave);
    }
    public void LoadData()
    {
        if(PlayerPrefs.HasKey("NumWave"))//Si anteriormente he guardado este dato, lo cargo
        {
            numWave = PlayerPrefs.GetInt("NumWave");
            numWave++;
        }
    }
    public void NextWave()
    {
        StartCoroutine("WaveCompleted");
    }
    IEnumerator WaveCompleted()
    {
        waveText.SetActive(true);
        waveText.GetComponent<Text>().text = "Wave Completed";
        yield return new WaitForSeconds(2);
        SaveData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
