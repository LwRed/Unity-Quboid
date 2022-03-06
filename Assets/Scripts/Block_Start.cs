using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        _firstPos = this.transform.position;
        _collider = GetComponent<BoxCollider>();
        StartMove();
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
    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            _particleEffect.SetActive(true);
                Debug.Log("Start Contact On");
                GameObject.Find("GameManager").GetComponent<GameManager>().tileContact++;
                //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
                StartCoroutine (CoUpdate2());
                StartCoroutine (CoUpdate());
            //Play Audio
            audioSource.Play();
        }
    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Start Contact Off");
            GameObject.Find("GameManager").GetComponent<GameManager>().tileContact--;
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
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
        //Tells Unity to wait 1 second
        yield return new WaitForSeconds(0.25f); //0.15f
        //Cuboid en appui sur une seule Tile
        if (GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 1 && GameObject.FindWithTag("Player").GetComponent<Quboid>()._state != CUBESATE.VERTICAL)
            {
                Debug.Log("StartTile #1");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            }
        //Cuboid sans appui
        else if(GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 0)
            {
                Debug.Log("StartTile #0");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            }
        //Autre Cas
        else
            {
                Debug.Log("StartTile #Other");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;

            }
    }
}
