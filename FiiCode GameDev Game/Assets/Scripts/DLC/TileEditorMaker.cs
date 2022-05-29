using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.IO;

public class TileEditorMaker : MonoBehaviour
{
    public Tile[] tiles;
    public GameObject Astro;
    public static int SelectedTile = 0;
    private bool HasStar = false;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("MapAstroStartPos"))
        {
            PlayerPrefs.SetInt("MapAstroStartPos", 7);
        }

        SetSavedAstroPos();

        if(!PlayerPrefs.HasKey("MapMaker"))
        {
            PlayerPrefs.SetString("MapMaker", "0000000000000000000000000000");
        }
        else
        {
            SetMapToSaved();
        }
    }

    // Selected tile type 1 = up, 2 = down, 3 = left, 4 = right
    // Added on tile type 1 = rock, 2 = meteor, 3 = star, 4 = end, 0 = nothing
    public void SelectTile(int type)
    {
        AudioManager.Play("MapMakerMove");
        switch (type)
        {
            case 1:
                if (SelectedTile < 7) SelectedTile = 28 - (7 - SelectedTile);
                else SelectedTile -= 7;
                break;

            case 2:
                if (SelectedTile > 20) SelectedTile = 7 - (28 - SelectedTile);
                else SelectedTile += 7;
                break;

            case 3:
                if (SelectedTile == 0) SelectedTile = 27;
                else SelectedTile--;
                break;

            case 4:
                if (SelectedTile == 27) SelectedTile = 0;
                else SelectedTile++;
                break;
        }
    }
    public void AddOnTile(int type)
    {
        if (SelectedTile == PlayerPrefs.GetInt("MapAstroStartPos"))
        {
            AudioManager.Play("Error");
            return;
        }

        tiles[SelectedTile].SpawnNothing();
        switch (type)
        {
            case 1:
                tiles[SelectedTile].isRock = true;
                tiles[SelectedTile].isImmutable = false;
                tiles[SelectedTile].isChest = false;
                tiles[SelectedTile].isEnd = false;
                tiles[SelectedTile].SpawnRock();
                break;

            case 2:
                tiles[SelectedTile].isRock = false;
                tiles[SelectedTile].isImmutable = true;
                tiles[SelectedTile].isChest = false;
                tiles[SelectedTile].isEnd = false;
                tiles[SelectedTile].SpawnImmutable();
                break;

            case 3:
                if (HasStar) { AudioManager.Play("Error"); return; }
                tiles[SelectedTile].isRock = false;
                tiles[SelectedTile].isImmutable = false;
                tiles[SelectedTile].isChest = true;
                tiles[SelectedTile].isEnd = false;
                tiles[SelectedTile].SpawnChest();
                HasStar = true;
                break;

            case 4:
                tiles[SelectedTile].isRock = false;
                tiles[SelectedTile].isImmutable = false;
                tiles[SelectedTile].isChest = false;
                tiles[SelectedTile].isEnd = true;
                tiles[SelectedTile].SpawnEnd();
                break;

            case 0:
                if (tiles[SelectedTile].isChest) HasStar = false;
                tiles[SelectedTile].isRock = false;
                tiles[SelectedTile].isImmutable = false;
                tiles[SelectedTile].isChest = false;
                tiles[SelectedTile].isEnd = false;
                tiles[SelectedTile].SpawnNothing();
                
                break;
        }
        AudioManager.Play("MapMakerAdd");

        char[] a = PlayerPrefs.GetString("MapMaker").ToCharArray();
        a[SelectedTile] = (char)('0' + Convert.ToChar(type));

        PlayerPrefs.SetString("MapMaker", new string(a));

        Debug.Log(PlayerPrefs.GetString("MapMaker"));

        PlayerPrefs.SetInt("IsPlayTested", 0);
    }
    public void ChangeAstroStartPos()
    {
        AudioManager.Play("MapMakerAdd");
        EditSaidTile(SelectedTile, 0);
        PlayerPrefs.SetInt("MapAstroStartPos", SelectedTile);
        int posX = SelectedTile % 7;
        int posY = SelectedTile / 7;

        Astro.GetComponent<Player>().posX = posX;
        Astro.GetComponent<Player>().posY = posY;

        Astro.transform.position = new Vector3(tiles[SelectedTile].transform.position.x, Astro.transform.position.y, tiles[SelectedTile].transform.position.z);
    }

    public void SetSavedAstroPos()
    {
        int posX = PlayerPrefs.GetInt("MapAstroStartPos") % 7;
        int posY = PlayerPrefs.GetInt("MapAstroStartPos") / 7;

        Astro.GetComponent<Player>().posX = posX;
        Astro.GetComponent<Player>().posY = posY;

        Astro.transform.position = new Vector3(tiles[PlayerPrefs.GetInt("MapAstroStartPos")].transform.position.x, Astro.transform.position.y, tiles[PlayerPrefs.GetInt("MapAstroStartPos")].transform.position.z);
    }

    public void SetMapToSaved()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            EditSaidTile(i, int.Parse(PlayerPrefs.GetString("MapMaker")[i].ToString()));
        }
    }

    public void ClearAll()
    {
        AudioManager.Play("MapMakerOpenAdd");
        PlayerPrefs.SetString("MapMaker", "0000000000000000000000000000");
        SetMapToSaved();
        PlayerPrefs.SetInt("IsPlayTested", 0);
    }


    private void Update()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (i == SelectedTile)
                tiles[i].GetComponent<MeshRenderer>().material.color = new Color32(0, 255, 30, 150);
        }
    }

    public void EditSaidTile(int tile, int type)
    {
        switch (type)
        {
            case 1:
                tiles[tile].isRock = true;
                tiles[tile].isImmutable = false;
                tiles[tile].isChest = false;
                tiles[tile].isEnd = false;
                tiles[tile].SpawnRock();
                break;

            case 2:
                tiles[tile].isRock = false;
                tiles[tile].isImmutable = true;
                tiles[tile].isChest = false;
                tiles[tile].isEnd = false;
                tiles[tile].SpawnImmutable();
                break;

            case 3:
                tiles[tile].isRock = false;
                tiles[tile].isImmutable = false;
                tiles[tile].isChest = true;
                tiles[tile].isEnd = false;
                tiles[tile].SpawnChest();
                break;

            case 4:
                tiles[tile].isRock = false;
                tiles[tile].isImmutable = false;
                tiles[tile].isChest = false;
                tiles[tile].isEnd = true;
                tiles[tile].SpawnEnd();
                break;

            case 0:
                tiles[tile].isRock = false;
                tiles[tile].isImmutable = false;
                tiles[tile].isChest = false;
                tiles[tile].isEnd = false;
                tiles[tile].SpawnNothing();
                break;
        }
    }


 
}
