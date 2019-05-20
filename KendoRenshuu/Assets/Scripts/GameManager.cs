using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //INSTANCE & PLAYER REFERENCE
    public static GameManager GM;
    private GameObject _player;
    
    //UI ELEMENTS
    public Text ScoreTextUI;
    public Text LivesTextUI;
    public GameObject GameOverTextUI;

    //SCORE & LIVES 
    private int score; //score property sets ui whenever score changes
    public int Score
    {
        get
        {
            return score;
            
        }
        set
        {
            score = value;
            ScoreTextUI.text = "Score: " + value;
        }
    }
    private int lives; //lives property calls game over when it falls to 0 
    public int Lives
    {
        get
        {
            return lives;
            
        }
        set
        {
            lives = value;
            //if lives drops below 0, call game over function
            if (lives<1)
                GameOver();
            //else update the lives ui
            else  
                LivesTextUI.text = "Lives: " + value;
        }
    }

   //MISC: TIMER, LEVEL IND, NUM OF ACTIVE ENEMIES
    private float _timer;
    public int LevelInd;
    [HideInInspector]
    public int NumActiveEnemies;
    
    //SPAWN VARS
    private string levelSpawnLayout = "121"; //inital level spawn layout
    public List<char> SpawnList;
    public float SpawnTime;
    [Header("Spawners")] 
    public GameObject HighSpawnL;
    public GameObject MedSpawnL;
    public GameObject LowSpawnL;
    public GameObject HighSpawnR;
    public GameObject MedSpawnR;
    public GameObject LowSpawnR;

   
    
    void Start()
    {
        //SINGLETON
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(GM);
        }
        else
        {
            Destroy(gameObject);
        }
        
        //get player reference
        _player = GameObject.FindWithTag("Player");
        
        //init stuff
        LevelInd = 1;
        Lives = 3;
        Score = 0;
        _timer = 0;
        SpawnList = new List<char>(); //init the list
        
        //load the first level
        LoadLevel(LevelInd);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > SpawnTime)
        {   
            if (SpawnList.Count != 0) //if the spawn list is not empty, spawn a thing                        
            {
                SpawnEnemy(SpawnList[0]); //spawn a thing
                SpawnList.RemoveAt(0); //remove it from the list
                _timer = 0; //reset the timer
            }
        }
        Debug.Log("Active Enemies: "+NumActiveEnemies);
    }

    void LoadSpawnListFromLevelFile(int levelIndex)
    {
        string filePath = Application.dataPath + "/Resources" + "/Levels" + "/level" + levelIndex + ".txt";
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, levelSpawnLayout);
        }
        string spawnListText = File.ReadAllText(filePath);
        Debug.Log(spawnListText);

        //save the spawn chars from the text file to the SpawnList list 
        for (int i = 0; i < spawnListText.Length; i++)
        {
            SpawnList.Add(spawnListText[i]);
        }
    }
    
    void SpawnEnemy(char enemyType)
    {
        NumActiveEnemies++; //count the number of active enemies
        GameObject enemy;
        switch (enemyType)
        {
            case '1'://low
                enemy = ObjectPool.Pool.GetWorm();
                if (Random.Range(0, 2) < 1) //random spawn on L or R side
                    enemy.transform.position = LowSpawnL.transform.position;
                else
                {
                    enemy.transform.position = LowSpawnR.transform.position;
                    enemy.transform.eulerAngles += new Vector3 (transform.eulerAngles.x, 180);
                }
                break;
            case '2'://med
                enemy = ObjectPool.Pool.GetSkeleton();
                if (Random.Range(0, 2) < 1) //random spawn on L or R side
                    enemy.transform.position = MedSpawnL.transform.position;
                else
                {
                    enemy.transform.position = MedSpawnR.transform.position;
                    enemy.transform.eulerAngles += new Vector3 (transform.eulerAngles.x, 180);
                }
                break;
            case '3'://high
                
                break;

        }
    }

    void LoadLevel(int i)
    {
        LoadSpawnListFromLevelFile(i);
        //add enemies to the level spawn layout
        for (int j = 0; j < Mathf.RoundToInt(LevelInd/2); j++)
        {
            levelSpawnLayout = levelSpawnLayout + Random.Range(0, 4);
        }
    }


    public void LevelCleared()
    {
        StartCoroutine(DelayedLevelLoad(LevelInd,3f));
    }
    
    public IEnumerator DelayedLevelLoad(int i, float t)
    {
     yield return new WaitForSeconds(t);
     LoadLevel(i);
    }

    public void GameOver()
    {
        GameOverTextUI.SetActive(true);
        //change player state to hurt
        _player.GetComponent<Player>().CurrentPlayerState = Player.PlayerState.Hurt;
        
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
    
}
