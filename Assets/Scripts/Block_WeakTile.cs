using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_WeakTile : MonoBehaviour
{
    private Vector3 _firstPos;
    private Vector3 _tempPos;
    // Use this for initialization
    private BoxCollider _collider;
    private Rigidbody _rb;

    //adjust this to change speed
    [SerializeField]
    float speed = 1f;
    //adjust this to change how high it goes
    [SerializeField]
    float height = 0.1f;
    private float decalage = 0.1f;
    private float _firstdecalage;
    //public GameObject _particleEffect;
    private bool _protected = false;

    //Son
    public AudioSource audioWeakSimple;
    public AudioSource audioWeakDouble;
    public AudioSource audioWeakCrack;

    void Awake()
    {
        _firstPos = this.transform.position;
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
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
        //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
    }

    void OnCollisionStay (Collision col)
    {

            if (col.gameObject.tag == "Player")
            {
                decalage = 0;
            }
            else
            {
                decalage = _firstdecalage;
            }

    }
    void OnCollisionEnter (Collision col)
    {

            if (col.gameObject.tag == "Player")
            {
                
                //_particleEffect.SetActive(true);
                Debug.Log("Weak Contact On");
                GameObject.Find("GameManager").GetComponent<GameManager>().weakContact++;
                //Game Engine for Weak Test
                _protected = false;
                //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
                StartCoroutine (CoUpdate());
            }

    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {   
            Debug.Log("Weak Contact Off");
            GameObject.Find("GameManager").GetComponent<GameManager>().weakContact--;
            //Protect Previous Tile from destroy
            _protected = true;
            //StartCoroutine (CoUpdate());
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
        //Tells Unity to wait
        yield return new WaitForSeconds(0.10f); //0.25f sur PC lent, 0.10f sur PC rapide
        //_particleEffect.SetActive(false);
        if (GameObject.Find("GameManager").GetComponent<GameManager>().weakContact == 1 && GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 0)
            {       
                    //Son
                    audioWeakCrack.Play();
                    //Weak Test True => Destroy this Weak Tile
                    if (_protected == false)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = true;
                        GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
                        Debug.Log("Weaked!");
                        iTween.ScaleTo(this.gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "easeType", iTween.EaseType.easeInBack, "delay", 0.0f, "time", 0.1f, "onComplete", "nothing"));
                        Destroy(this.gameObject,0.1f);
                    }       
            }
            else if(GameObject.Find("GameManager").GetComponent<GameManager>().weakContact == 1 && GameObject.Find("GameManager").GetComponent<GameManager>().tileContact == 1)
            {
                //Son
                audioWeakSimple.Play();
                //Weak Test False => Release Keyboard
                Debug.Log("WeakTile #1");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = false;
                //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
            }
            else if(GameObject.Find("GameManager").GetComponent<GameManager>().weakContact == 2)
            {
                //Son
                audioWeakDouble.Play();
                //Weak Test False => Release Keyboard
                Debug.Log("WeakTile #2");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
            }
            else
            {
                Debug.Log("WeakTile #Other");
                GameObject.Find("GameManager").GetComponent<GameManager>()._gameOver = false;
                GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
            }
    }
}
