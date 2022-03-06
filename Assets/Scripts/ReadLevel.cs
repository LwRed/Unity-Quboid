using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ReadLevel : MonoBehaviour
{

public TextAsset mapText;
 public Transform player;
 public Transform tile;
 public Transform weak;
 public Transform start;
 public Transform goal;
 
 public const string s_tile = "t";
 public const string s_weak = "w";
 public const string s_start = "s";
 public const string s_goal = "g";


 
 void Start() {
     //Level Construction
     GenerateLevel();

 }

void Update()
{

}


 public void GenerateLevel()
 {
     if (mapText.text != null)
     {
        string[] lines = Regex.Split(mapText.text, "\r\n|\r|\n");
        int rows = lines.Length;
        //Debug.Log("Lignes :" + rows);

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)  {
         string[] stringsOfLine = Regex.Split(lines[i], " ");
         levelBase[i] = stringsOfLine;
        }
        //return levelBase;
        //Debug.Log(levelBase);

        string[][] jagged = levelBase;

        // create planes based on matrix
        for (int y = 0; y < jagged.Length; y++) {
             for (int x = 0; x < jagged[0].Length; x++) {
                switch (jagged[y][x]){
                case s_start:
                    Instantiate(start, new Vector3(x, 0, -y), Quaternion.Euler(-90, 0, 0));
                    Instantiate(player, new Vector3(x, 2, -y), Quaternion.identity);
                    break;
                case s_tile:
                    Instantiate(tile, new Vector3(x, 0, -y), Quaternion.Euler(-90, 0, 0));
                    break;
                case s_weak:
                    Instantiate(weak, new Vector3(x, 0, -y), Quaternion.Euler(-90, 0, 0));
                    break;
                case s_goal:
                    Instantiate(goal, new Vector3(x, 0, -y), Quaternion.Euler(-90, 0, 0));
                    break;
                }
            }
        }
     }
 }
}
