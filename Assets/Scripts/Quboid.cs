using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CUBESATE
{
    HORIZON_X, HORIZON_Z, VERTICAL
}

public enum DIRECTION
{
    UP, DOWN, RIGHT, LEFT
}

public class Quboid : MonoBehaviour
{
    //Orientation du Cuboid
    public CUBESATE _state;
    //Fluidite de Rotation
    public float smoothness = 45 ;

    //Son
    public AudioSource audioSource;
    private float _height;
    private float _width;

    private int _moveCount;
    private Vector3 _pivot;
    private Vector3 _axis;
    private bool _isTurning;
    private Vector3 _firstPos;
    private Quaternion _currentRot = Quaternion.identity;
    private BoxCollider _boxColl;
    private Rigidbody _rigid;
    private bool winBool = false;
    private bool looseBool = false;



    private void Awake()
    {

        _boxColl = this.GetComponent<BoxCollider>();
        _rigid = this.GetComponent<Rigidbody>();

        _state = CUBESATE.VERTICAL;
        _isTurning = false;
        _height = this.transform.localScale.y;
        _width = this.transform.localScale.x /2.0f;

        _firstPos = this.transform.position;
    }
    // Use this for initialization
    void Start ()
    {
  
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (_isTurning == false && GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard == true)
        {

            //Clavier a bloquer
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine("TurningCube", DIRECTION.UP);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine("TurningCube", DIRECTION.DOWN);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine("TurningCube", DIRECTION.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine("TurningCube", DIRECTION.RIGHT);
            }
        }
    }


    void SetState(DIRECTION dir)
    {
        switch (_state)
        {

            case CUBESATE.HORIZON_X:
                {
                    if (dir == DIRECTION.RIGHT || dir == DIRECTION.LEFT)
                    {
                        _state = CUBESATE.VERTICAL;
                        
                    }
                    break;
                }
            case CUBESATE.HORIZON_Z:
                {
                    if (dir == DIRECTION.UP || dir == DIRECTION.DOWN)
                    {
                        _state = CUBESATE.VERTICAL;
                   
                    }
                    break;
                }
            case CUBESATE.VERTICAL:
                {
                    if (dir == DIRECTION.UP || dir == DIRECTION.DOWN)
                    {
                        _state = CUBESATE.HORIZON_Z;
                      
                    }
                    else
                    {
                        _state = CUBESATE.HORIZON_X;
                    
                    }
                    break;
                }
        }
    }

    void SetPivot(DIRECTION dir)
    {
        // Def du Pivot
        Debug.Log(dir.ToString());
        _pivot = this.transform.position;

        // Etat
        if (_state == CUBESATE.VERTICAL)
        {
            _pivot.y -= _height;
        }
        else
        {
            _pivot.y -= _width;
        }

        switch (dir)
        {
            case DIRECTION.UP:
                if (_state == CUBESATE.HORIZON_Z)
                {
                    _pivot.z += _height;
                }
                else
                {
                    _pivot.z += _width;
                }
                break;
            case DIRECTION.DOWN:
                if (_state == CUBESATE.HORIZON_Z)
                {
                    _pivot.z -= _height;
                }
                else
                {
                    _pivot.z -= _width;
                }
                break;
            case DIRECTION.LEFT:
                if (_state == CUBESATE.HORIZON_X)
                {
                    _pivot.x -= _height;
                }
                else
                {
                    _pivot.x -= _width;
                }
                break;
            case DIRECTION.RIGHT:
                if (_state == CUBESATE.HORIZON_X)
                {
                    _pivot.x += _height;
                }
                else
                {
                    _pivot.x += _width;
                }
                break;
        }
    }

    void SetAxis(DIRECTION dir)
    {
        switch (dir)
        {
            case DIRECTION.UP:
            case DIRECTION.DOWN:
                _axis = new Vector3(1.0f, 0.0f, 0.0f);
                break;
            case DIRECTION.RIGHT:
            case DIRECTION.LEFT:
                _axis = new Vector3(0.0f, 0.0f, 1.0f);
                break;
        }
    }

    void SetCurrentPos(DIRECTION dir)
    {
        switch (dir)
        {
            case DIRECTION.UP:
                _currentRot.x += 90;
                break;
            case DIRECTION.DOWN:
                _currentRot.x -= 90;
                break;
            case DIRECTION.LEFT:
                _currentRot.z += 90;
                break;
            case DIRECTION.RIGHT:
                _currentRot.z -= 90;
                break;
        }
        this.transform.rotation = _currentRot;
    }

    IEnumerator TurningCube(DIRECTION dir)
    {
        //Comptage des Mouvements
        GameObject.Find("GameManager").GetComponent<GameManager>().moves++;

        _isTurning = true;
        SetGravity(false);

        SetPivot(dir);
        SetAxis(dir);

        int mark;

        if (dir == DIRECTION.UP || dir == DIRECTION.LEFT) mark = 1;
        else mark = -1;

        for (int i = 0; i < smoothness; ++i)
        {
            this.transform.RotateAround(_pivot, _axis, 90 / smoothness * mark);
            yield return null;
        }

        SetState(dir);
        _isTurning = false;
        SetGravity(true);

        //Play Audio
        audioSource.Play();
    }

        void SetGravity(bool isGravity)
    {
        _rigid.useGravity = isGravity;
    }



    void OnCollisionStay (Collision col)
    {
        //Check si Gagne
        if (col.gameObject.tag == "TileGoal" && _state == CUBESATE.VERTICAL)
        {
            if (winBool == false)
            {
            Debug.Log("GoalTile #Check");
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
            winBool = true;
            }
        }
        //Check si Perdu
        if (col.gameObject.tag == "Ground")
        {
            if (looseBool == false)
            {
            Debug.Log("Ground #Check");
            GameObject.Find("GameManager").GetComponent<GameManager>().activeKeyboard = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().ResetLevel();
            looseBool = true;
            }
        }
    }
}
