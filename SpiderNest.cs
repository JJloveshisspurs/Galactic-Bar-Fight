using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderNest : MonoBehaviour
{

    public float spawnInterval;
    private float spawnTimer;

    public GameObject spiderInstance;


    public List<GameObject> spawnedSpiders = new List<GameObject>();


    public bool spawnerActive = false;

    public float minimalSpanwtime = 9f;
    public float maximumSpawnTime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        spawnTimer += Time.deltaTime;


        if (spawnTimer >= spawnInterval)
        {
            if (GameController.instance.currenGameState != GameController.gameState.gameOver)
            {
                spawnTimer = 0f;
                spawnInterval = Random.Range(minimalSpanwtime, maximumSpawnTime);
                SpawnNewSpider();
            }

        }

    }

    public void SpawnNewSpider()
    {
        //Debug.Log("Spawning new spider!");
        if (GameController.instance.currenGameState == GameController.gameState.gameplay)
        {
            if (spawnerActive == true && GameController.instance.CheckIfEnemySpiderSpawnIsPossible())
            {
                GameController.instance.IncrementSpiderCount();

                GameObject oSpider = Instantiate(spiderInstance, this.gameObject.transform);
                oSpider.transform.localPosition = Vector3.zero;
                oSpider.transform.parent = null;

                spawnedSpiders.Add(oSpider);

            }
        }
    }


    public void ClearSpawnedSpiders()
    {
        Debug.Log("Clearing Spawned Spiders!!!!");

        for (int i = 0; i < spawnedSpiders.Count; i++)
        {
            if (spawnedSpiders[i] != null)
                Destroy(spawnedSpiders[i]);


        }


        spawnedSpiders.Clear();

    }

    public void ActivateSpawner()
    {

        spawnerActive = true;
    }

    public void DeactivateSpawner()
    {
        spawnerActive = false;

        ClearSpawnedSpiders();
    }



}
