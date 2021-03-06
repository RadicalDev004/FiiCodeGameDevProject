using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLayout : MonoBehaviour
{
    public GameObject Rock, Chest, End, Immutable, Steps;

    public int sizeX = 7;
    public int tileSize;
    public float offset;

    private Vector3 initialPos;

    public static Transform[] tiles;

    [Header("Spawnable Positions")]
    public Vector3 RockPos; 
    public Vector3 StarPos; 
    public Vector3 EndPos; 
    public Vector3 ImmutablePos;

    private void OnEnable()
    {
        //Prepares tiles for game start

        tiles = new Transform[transform.childCount];

        for(int i=0; i<transform.childCount; i++)
        {
            tiles[i] = transform.GetChild(i);
            tiles[i].GetComponent<Tile>().number = i;
        }

        initialPos = tiles[0].position;

        ArrangeTiles();
    }

    private void ArrangeTiles()
    {
        for(int i=0; i<tiles.Length; i++)
        {
            float x = (i % sizeX)*(-tileSize - offset);
            float y = 0f;
            float z = (i / 7) * (tileSize + offset);

            tiles[i].position = initialPos + new Vector3(x,y,z);
        }
    }
}
