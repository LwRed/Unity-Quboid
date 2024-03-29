﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe permettant de positionner le Block Tile sur la Grille avec iTween
//Classe permettant de gerer les collisions du Quboid a la Tile
//Classe permettant le mouvement Haut Bas de la Tile
public class Block_Tile : MonoBehaviour
{
    private Vector3 _firstPos;
    private Vector3 _tempPos;
    // Use this for initialization
    private BoxCollider _collider;

    //adjust this to change speed
    [SerializeField]
    float speed = 1f;
    //adjust this to change how high it goes
    [SerializeField]
    float height = 0.1f;
    private float decalage = 0.1f;
    private float _firstdecalage;

    public GameObject _particleEffect;
    private bool _protected = false;

    void Awake()
    {
        _firstPos = this.transform.position;
        _collider = GetComponent<BoxCollider>();
        StartMove();
        decalage = Random.Range(0.1f, 0.3f);
        _firstdecalage = decalage;
    }

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

        //iTween.MoveTo(this.gameObject, _firstPos, moveTime);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", _firstPos, "time", moveTime, "oncomplete", "MoveComplete"));
        //iTween.MoveTo()
    }

    void MoveComplete()
    {
        //Debug.Log("iTween Complete");
        _collider.isTrigger = false;
    }

    void OnCollisionStay (Collision col)
    {

            if (col.gameObject.tag == "Player")
            {
                decalage = 0;
            }
    }
    void OnCollisionEnter (Collision col)
    {
            if (col.gameObject.tag == "Player")
            {
                _particleEffect.SetActive(true);
                Debug.Log("Block_Tile.cs - OnCollisionEnter");
                GameObject.Find("GameManager").GetComponent<GameManager>().tileContact++;
                _protected = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
                StartCoroutine (CoUpdate2());
                StartCoroutine (CoUpdate());
            }
    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            decalage = _firstdecalage;
            Debug.Log("Block_Tile.cs - OnCollisionExit");
            GameObject.Find("GameManager").GetComponent<GameManager>().tileContact--;
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            _protected = true;
            //StartCoroutine (CoUpdate2());
            StartCoroutine (CoUpdate());
        }
    }

    void Update()
    {
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * decalage * speed) * height + _firstPos.y;
        //Debug.Log(Time.time);
        //Debug.Log(decalage);
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
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
        yield return new WaitForSeconds(0.10f); //0.25f sur PC lent, 0.10f sur PC rapide

        //Cuboid en appui sur une seule Tile
        if (GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 1 && GameObject.Find("GameManager").GetComponent<GameManager>().weakContact == 0 && GameObject.FindWithTag("Player").GetComponent<Quboid>()._isTurning == false && GameObject.FindWithTag("Player").GetComponent<Quboid>()._state != CUBESATE.VERTICAL)
            {
                if (_protected == false)
                {
                    Debug.Log("Block_Tile.cs - #1 contact only - Loose");
                    GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                    GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
                    //iTween.ScaleTo(this.gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 0.25f, "onComplete", "nothing"));
                    //Destroy(this.gameObject);
                }
        }
        //Cuboid sans appui (à debugger)
        else if(GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 0 && GameObject.FindWithTag("Player").GetComponent<Quboid>()._isTurning == false)
            {
                Debug.Log("Block_Tile.cs - #0 contact - Loose");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            }
        //Cuboid avec les 2 appuis
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 2 && GameObject.FindWithTag("Player").GetComponent<Quboid>()._isTurning == false)
        {
            Debug.Log("Block_Tile.cs - #2 contact - Continue");
            GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
        }
    }
}
