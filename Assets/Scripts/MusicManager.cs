using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource _AudioSource;
 
     public AudioClip _AudioMusicClip1;
     public AudioClip _AudioMusicClip2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
        void Update () 
     {
 
         if (Input.GetKeyDown(KeyCode.M))
         {
 
             if (_AudioSource.clip == _AudioMusicClip1)
             {
 
                 _AudioSource.clip = _AudioMusicClip2;
                _AudioSource.volume = 1f;
 
                 _AudioSource.Play();
 
             }
 
             else
             {
                 
                 _AudioSource.clip = _AudioMusicClip1;
                _AudioSource.volume = 0.25f;

                _AudioSource.Play();
 
             }
 
         }
     
     }
    
}
