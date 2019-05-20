using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    
    public Text ScoreTextUI;
    public Text LivesTextUI;

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

    private int lives;

    public int Lives
    {
        get
        {
            return lives;
            
        }
        set
        {
            lives = value;
            LivesTextUI.text = "Lives: " + value;
        }
    }


    private List<char> _spawnList;
    private int _levelInd;
    private float _timer;
    public float SpawnTime;

    public int NumActiveEnemies;

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
        
        //init stuff
        _levelInd = 1;
        Lives = 3;
        Score = 0;

        _timer = 0;
        _spawnList = new List<char>(); //init the list
        
        LoadLevel(_levelInd);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > SpawnTime)
        {
  
            //all the enemies in the level have spawned
            if (_spawnList.Count == 0)
            {
                //TODO: this means all the enemies have spawned
            }
            else
            {
                SpawnEnemy(_spawnList[0]); //spawn a thing 
                NumActiveEnemies++; //count the number of active enemies
                _spawnList.RemoveAt(0); //remove it from the list
                _timer = 0; //reset the timer
            }
        }
       
    }

    void LoadSpawnListFromLevelFile(int levelIndex)
    {
        string filePath = Application.dataPath + "/Resources" + "/Levels" + "/level" + levelIndex + ".txt";
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "123123");
        }
        string spawnListText = File.ReadAllText(filePath);
        Debug.Log(spawnListText);

        //save the spawn chars from the text file to the SpawnList list 
        for (int i = 0; i < spawnListText.Length; i++)
        {
            _spawnList.Add(spawnListText[i]);
        }
    }

    void SpawnEnemy(char enemyType)
    {
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
    }
    
}
