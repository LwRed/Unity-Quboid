using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Goal : MonoBehaviour
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
    float height = 0.05f;
    private float decalage = 0.1f;
    private float _firstdecalage;

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
            else
            {
                decalage = _firstdecalage;
            }

    }

    void OnCollisionEnter (Collision col)
    {

            if (col.gameObject.tag == "Player")
            {
                Debug.Log("Tile Contact On");
                GameObject.Find("GameManager").GetComponent<GameManager>().tileContact++;
                //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
            }

    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Tile Contact Off");
            GameObject.Find("GameManager").GetComponent<GameManager>().tileContact--;
            //GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = true;
        }
    }


    void Update()
    {

    //calculate what the new Y position will be
    float newY = Mathf.Sin(Time.time * decalage * speed) * height + _firstPos.y;
     //set the object's Y to the new calculated Y
    transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
    } 
}
