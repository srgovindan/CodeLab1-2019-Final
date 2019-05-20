using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //SCORE & LIVES 
    private const string PLAYER_PREFS_HIGHSCORE = "highScore";

    //INSTANCE & PLAYER REFERENCE
    public static GameManager GM;
    private GameObject _player;

    //MISC: TIMER, LEVEL IND, NUM OF ACTIVE ENEMIES
    private float _timer;
    public GameObject GameOverTextUI;
    public GameObject WaveNumberUI;
    private int highScore;
    public Text HighScoreUI;

    [Header("Spawners")] public GameObject HighSpawnL;

    public GameObject HighSpawnR;
    private int levelInd;
    public int LevelInd
    {
        get
        {
            return levelInd;
            
        }
        set
        {
            levelInd = value;
            StartCoroutine(UpdateWaveNumberUI());
        }
    }

    //SPAWN VARS
    private string levelSpawnLayout = "121"; //inital level spawn layout
    private int lives; //lives property calls game over when it falls to 0 
    public Text LivesTextUI;
    public GameObject LowSpawnL;
    public GameObject LowSpawnR;
    public GameObject MedSpawnL;
    public GameObject MedSpawnR;

    [HideInInspector] public int NumActiveEnemies;

    private int score; //score property sets ui whenever score changes

    //UI ELEMENTS
    public Text ScoreTextUI;
    public List<char> SpawnList;
    public float SpawnTime;

    public int HighScore
    {
        get { return highScore; }
        set
        {
            if (value > highScore) //if the value is greater than the high score
            {
                highScore = value; //make that the new high score
                HighScoreUI.text = "High Score: " + highScore; //update the high score UI
                PlayerPrefs.SetInt(PLAYER_PREFS_HIGHSCORE, HighScore); //save the new high score to player prefs
            }
        }
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreTextUI.text = "Score: " + value;
            HighScore = score;
        }
    }

    public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            //if lives drops below 0, call game over function
            if (lives < 0)
                GameOver();
            //else update the lives ui
            else
                LivesTextUI.text = "Lives: " + value;
        }
    }

    private void Start()
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
        HighScore = PlayerPrefs.GetInt(PLAYER_PREFS_HIGHSCORE, 10); //load the high score from player prefs

        //load the first level
        LoadLevel(LevelInd);
    }

    private void Update()
    {
        //SPAWN TIMER: Spawns an enemy from the Spawn List when the Timer reaches the Spawn Time
        _timer += Time.deltaTime;
        if (_timer > SpawnTime)
            if (SpawnList.Count != 0) //if the spawn list is not empty, spawn a thing                        
            {
                SpawnEnemy(SpawnList[0]); //spawn a thing
                SpawnList.RemoveAt(0); //remove it from the list
                _timer = 0; //reset the timer
            }

        //Debug.Log("Active Enemies: " + NumActiveEnemies);
    }

    IEnumerator UpdateWaveNumberUI()
    {
        WaveNumberUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AudioManager.AM.PlayClipName("WaveComplete");
        WaveNumberUI.GetComponent<Text>().text = "WAVE " + levelInd;
        yield return new WaitForSeconds(2f);
        WaveNumberUI.SetActive(false);
        
    }

    private void LoadSpawnListFromLevelFile(int levelIndex)
    {
        var filePath = Application.dataPath + "/Resources" + "/Levels" + "/level" + levelIndex + ".txt";
        if (!File.Exists(filePath)) File.WriteAllText(filePath, levelSpawnLayout);
        var spawnListText = File.ReadAllText(filePath);
        Debug.Log(spawnListText);

        //save the spawn chars from the text file to the SpawnList list 
        for (var i = 0; i < spawnListText.Length; i++) SpawnList.Add(spawnListText[i]);
    }

    private void SpawnEnemy(char enemyType)
    {
        NumActiveEnemies++; //count the number of active enemies
        GameObject enemy;
        switch (enemyType)
        {
            case '1': //low
                enemy = ObjectPool.Pool.GetWorm();
                if (Random.Range(0, 2) < 1) //random spawn on L or R side
                {
                    enemy.transform.position = LowSpawnL.transform.position;
                }
                else
                {
                    enemy.transform.position = LowSpawnR.transform.position;
                    enemy.transform.eulerAngles += new Vector3(transform.eulerAngles.x, 180);
                }

                break;
            case '2': //med
                enemy = ObjectPool.Pool.GetSkeleton();
                if (Random.Range(0, 2) < 1) //random spawn on L or R side
                {
                    enemy.transform.position = MedSpawnL.transform.position;
                }
                else
                {
                    enemy.transform.position = MedSpawnR.transform.position;
                    enemy.transform.eulerAngles += new Vector3(transform.eulerAngles.x, 180);
                }

                break;
            case '3': //high

                break;
        }
    }

    private void LoadLevel(int i)
    {
        LoadSpawnListFromLevelFile(i);
        //add enemies to the level spawn layout to create a different level set for each computer
        for (var j = 0; j < Mathf.RoundToInt(LevelInd / 2); j++)
            levelSpawnLayout = levelSpawnLayout + Random.Range(0, 4);
    }

    public void LevelCleared()
    {
        StartCoroutine(DelayedLevelLoad(LevelInd, 2f));
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
        if (Input.anyKey)
            ResetGame();
    }

    public void ResetGame()
    {
        //DESTROY ALL ENEMY GAME OBJECTS
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies);
        foreach (var enemy in enemies) Destroy(enemy);
        ObjectPool.Pool.WormPool.Clear();
        ObjectPool.Pool.SkeletonPool.Clear();
        //RESET OR REMOVE
        GameOverTextUI.SetActive(false); //turn off game over ui
        StopAllCoroutines(); //stops all delayed level loads
        NumActiveEnemies = 0;
        _player.GetComponent<Player>().CurrentPlayerState = Player.PlayerState.Idle; //reset player state
        //LOAD STUFF AGAIN
        Enemy.EnemyMoveSpeed = .01f; //reset enemy move speed
        LevelInd = 1; //reset level ind
        Lives = 3;
        Score = 0;
        _timer = 0;
        SpawnList = new List<char>(); //init the list
        LoadLevel(LevelInd); //load first level
    }
}