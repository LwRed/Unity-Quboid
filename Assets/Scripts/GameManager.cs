using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

     private ReadLevel scriptLevel;
     public GameObject cameraObject;
     public GameObject GameOverObject;
     private FollowCamera scriptCamera;
     public Text MoveText;
     public Text LevelText;
     public Text LivesText;


    public int AskedFrameRate = 60;
     public int levelNumber;
     public int moves = 0;
     public int lives = 3;
     public int weakContact = 0;
     public int tileContact = 0;
     public bool activeKeyboard = false;
     public bool _gameOver = false;

    private Vector3 offscreen = new Vector3(0.0f,20.0f,0.0f);

    void Awake()
    {
        //Frame Rate
        Application.targetFrameRate = AskedFrameRate;

        //Script Access to current GameObject
        scriptLevel = GetComponent<ReadLevel>();

        //Level Load
        scriptLevel.mapText = Resources.Load("level_" + levelNumber) as TextAsset;
    }
    void Start()
    {
        //Script Access to Camera
        scriptCamera = cameraObject.GetComponent<FollowCamera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Update Canvas Texts
        MoveText.text = "Mouvements : " + moves;
        LevelText.text = "Niveau : " + levelNumber;

        if (GameObject.FindWithTag("Player") != null && scriptCamera.target == null)
        {
        //Assign Player.transform to Camera.target.transform
        scriptCamera.target = GameObject.FindWithTag("Player").transform;
        }
        //Debug Game #1
        if (activeKeyboard == false && _gameOver == false)
            {
                //StartCoroutine (CoUpdate());
               //activeKeyboard = true;
            }

        //Quit ou Redemarrage du jeu
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Nouvelle partie");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKey("escape"))
        {
            Debug.Log("Quitte le jeu");
            Application.Quit();
        }

    }

    public void ResetLevel()
    {
            //Reset Variables
            weakContact = 0;
            tileContact = 0;
            _gameOver = false;
            lives--;
            

            //Destroy Goal Tile
            var goal = GameObject.FindWithTag("TileGoal");
            iTween.ScaleTo(goal, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 0.1f, "onComplete", "nothing"));
            Destroy(goal);
            
            //Destroy Tile Clones
            var tiles = GameObject.FindGameObjectsWithTag ("Tile");
            foreach (var tile in tiles)
            {
            iTween.ScaleTo(tile, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 1.0f, "onComplete", "nothing"));
            Destroy(tile,1.0f);
            }
            //Destroy Weak Tile
            var weaks = GameObject.FindGameObjectsWithTag ("WeakTile");
            foreach (var weak in weaks)
            {
            iTween.ScaleTo(weak, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 1.0f, "onComplete", "nothing"));
            Destroy(weak,1.0f);
            }

            //Check Game Over
            if (lives > -1)
                {
                    //Canvas Update
                    LivesText.text = "Vies : " + lives;
                    this.GetComponent<ReadLevel>().GenerateLevel();

                    //Destroy Player
                    var player = GameObject.FindWithTag("Player");
                    iTween.ScaleTo(player, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 5.5f, "onComplete", "nothing"));
                    Destroy(player,0.5f);
                }
            else
                {   //Canvas Update with GameOver Screen
                    GameOverObject.SetActive(true);
                }
    }

    public void NextLevel()
    {

        //Destroy Game Objects
            //Destroy Goal Tile
            var goal = GameObject.FindWithTag("TileGoal");
            iTween.ScaleTo(goal, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 0.1f, "onComplete", "nothing"));
            Destroy(goal);

            //Destroy Player
            var player = GameObject.FindWithTag("Player");
            iTween.ScaleTo(player, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 0.5f, "onComplete", "nothing"));
            Destroy(player,0.5f);
            //Destroy Tiles Clones
            var tiles = GameObject.FindGameObjectsWithTag ("Tile");
            foreach (var tile in tiles)
            {
            iTween.ScaleTo(tile, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 1.0f, "onComplete", "nothing"));
            Destroy(tile,2.0f);
            }
            //Destroy Weak Tile Clones
            var weaks = GameObject.FindGameObjectsWithTag ("WeakTile");
            foreach (var weak in weaks)
            {
            iTween.ScaleTo(weak, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 1.0f, "onComplete", "nothing"));
            Destroy(weak,1.0f);
            }
            

        //Reset Variables
        moves = 0;
        weakContact = 0;
        tileContact = 0;
        _gameOver = false;

        //Update Level and Load Level
        levelNumber++;
        if (Resources.Load("level_" + levelNumber) != null)
        {
        scriptLevel.mapText = Resources.Load("level_" + levelNumber) as TextAsset;
        this.GetComponent<ReadLevel>().GenerateLevel();
        }
        else
        {
            Debug.Log("Partie terminée");
            levelNumber = 0;
            scriptLevel.mapText = Resources.Load("level_" + levelNumber) as TextAsset;
            this.GetComponent<ReadLevel>().GenerateLevel();
        }
    }

    IEnumerator CoUpdate()
    {
        //Tells Unity to wait 1 second
        yield return new WaitForSeconds(1.0f); //0.25f
        //Reactive le clavier si le jeu a un Bug
        //activeKeyboard = true;
        //ResetLevel();

    }

}
