using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe permettant de positionner le Block Start sur la Grille avec iTween
//Classe permettant de gerer les collisions du Quboid a la Tile Start
//Classe permettant le mouvement Haut Bas de la Tile Start
public class Block_Start : MonoBehaviour
{
    private Vector3 _firstPos;
    private Vector3 _tempPos;
    // Use this for initialization
    private BoxCollider _collider;
    //Declaration Object Particles
    public GameObject _particleEffect;
    
    //Son
    public AudioSource audioSource;
    private bool _audioPlayBool = true;

    void Awake()
    {
        _firstPos = this.transform.position;
        _collider = GetComponent<BoxCollider>();
        StartMove();

    }

//DEBUT ITWEEN

    int RandomMark()
    {
        int mark = Random.Range(0, 2);
        if (mark == 0) mark--;
        return mark;
    }
	
    public void StartMove(float BlockMoveTime = 0.0f)
    {
        _collider.isTrigger = true;

        float moveTime;

        if(BlockMoveTime == 0.0f)
        {
            moveTime = 3.0f;
        }
        else
        {
            moveTime = BlockMoveTime;
        }
        int mark = Random.Range(0, 2);

        if (mark == 0) mark--;

        this.transform.position = _tempPos = new Vector3(Random.Range(0, 30) * RandomMark(), _firstPos.y + Random.Range(0, 10) * RandomMark(), Random.Range(0, 30) * RandomMark());

        iTween.MoveTo(this.gameObject, iTween.Hash("position", _firstPos, "time", moveTime, "oncomplete", "MoveComplete"));

    }

    void MoveComplete()
    {
        //Debug.Log("iTween Complete");
        _collider.isTrigger = false;
    }

//FIN ITWEEN

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            _particleEffect.SetActive(true);
            Debug.Log("Block_Start.cs - OnCollisionEnter");
            GameObject.Find("GameManager").GetComponent<GameManager>().tileContact++;
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
            StartCoroutine (CoUpdate2());
            StartCoroutine (CoUpdate());
            if (_audioPlayBool == true)
            {
                //Play Audio
                audioSource.Play();
                _audioPlayBool = false;
            }
           
        }
    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Block_Start.cs - OnCollisionExit");
            GameObject.Find("GameManager").GetComponent<GameManager>().tileContact--;
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            //StartCoroutine (CoUpdate2());
            StartCoroutine (CoUpdate());
        }
    }

    IEnumerator CoUpdate()
    {
        //Tells Unity to wait 1 second
        yield return new WaitForSeconds(1.5f);
        _particleEffect.SetActive(false);
    }
    IEnumerator CoUpdate2()
    {
        //Tells Unity to wait
        yield return new WaitForSeconds(0.15f); //0.25f sur PC lent, 0.10f sur PC rapide
        //Cuboid en appui sur une seule Tile
        if (GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 1 && GameObject.FindWithTag("Player").GetComponent<Quboid>()._state != CUBESATE.VERTICAL)
            {
                Debug.Log("Block_Start.cs - #1 contact only - Loose");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            }
        //Cuboid sans appui
        else if(GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 0)
            {
                Debug.Log("Block_Start.cs - #0 contact - Loose");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            }
    }
}
