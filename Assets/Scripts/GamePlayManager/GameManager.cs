using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private Transform playerStartPos;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject enemy;

    private GameObject[] enemyPositions;


    //private GameObject[] mapBounds;

    private int minNumOfMobs, maxNumOfMobs;

    private float spawnDuration;

    private float numOfEnemyToSpawn;
    public float numOfEnemyInStage = 0;

    private EnumStages enumStages;

    public GameObject levelCompleteImg;
    public Text levelCompleteText;
    public Text numOfEnemiesText;

    public bool startStage = true;
    public bool waitingForNewStage = false;

    private void Awake()
    {
        Instantiate(player, playerStartPos.position, Quaternion.identity);
        minNumOfMobs = 1;
        maxNumOfMobs = 2;
        enemyPositions = GameObject.FindGameObjectsWithTag("EnemyPos");
        numOfEnemyToSpawn = Random.Range(minNumOfMobs, maxNumOfMobs);
        numOfEnemyInStage = numOfEnemyToSpawn;
        spawnDuration = Random.Range(3, 5);
        StartCoroutine(SpawnEnemys());
        startStage = true;
        enumStages = EnumStages.Stage1;
        numOfEnemiesText.text = "" + numOfEnemyInStage;
        //Debug.Log(numOfEnemyToSpawn.ToString());
    }


    private void LateUpdate()
    {
        if (numOfEnemyInStage == 0 && !waitingForNewStage)
        {
            StartCoroutine(StartNewStage());
            waitingForNewStage = true;
            numOfEnemiesText.text = "" + numOfEnemyInStage;
        }
    }


    public void EnemyDeathCound()
    {
        numOfEnemyInStage--;
        numOfEnemiesText.text = "" + numOfEnemyInStage;
    }


    public IEnumerator SpawnEnemys()
    {
        while (true)
        {

                yield return new WaitForSeconds(spawnDuration);

                if (enumStages == EnumStages.Stage1)
                {
                    if (numOfEnemyToSpawn > 0)
                    {


                        Instantiate(enemy, enemyPositions[Random.Range(0, 2)].transform.position, Quaternion.identity);
                        numOfEnemyToSpawn--;
                        minNumOfMobs++;
                    }

                }
                else if (enumStages == EnumStages.Stage2)
                {
                    if (numOfEnemyToSpawn > 0)
                    {
                        Instantiate(enemy, enemyPositions[Random.Range(0, 2)].transform.position, Quaternion.identity);
                        numOfEnemyToSpawn--;
                        maxNumOfMobs++;
                    }
                }
                else if (enumStages == EnumStages.Stage3)
                {
                    if (numOfEnemyToSpawn > 0)
                    {
                        Instantiate(enemy, enemyPositions[Random.Range(0, 2)].transform.position, Quaternion.identity);
                        numOfEnemyToSpawn--;
                        minNumOfMobs++;
                    }
                }
                else
                {
                StartCoroutine(FinishAllStages());
                }
        }
    }


    public IEnumerator FinishAllStages()
    {
        levelCompleteImg.SetActive(true);
        levelCompleteText.text = "Finished Stages Good Job!";
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }



    public IEnumerator StartNewStage()
    {
        levelCompleteImg.SetActive(true);
        startStage = false;
        levelCompleteText.text = enumStages.ToString() + " Clear";

        enumStages += 1;
        numOfEnemyToSpawn = Random.Range(minNumOfMobs, maxNumOfMobs);
        numOfEnemyInStage = numOfEnemyToSpawn;

        yield return new WaitForSeconds(3f);
        levelCompleteImg.SetActive(false);
        waitingForNewStage = false;

    }
}
