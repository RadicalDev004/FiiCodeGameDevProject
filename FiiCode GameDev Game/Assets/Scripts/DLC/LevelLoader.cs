using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RadicalKit;
using Firebase.Database;

public class LevelLoader : MonoBehaviour
{
    public string LevelCode;
    public string AstroPos;
    public string Reward;
    public string Name;

    public int Rate;
    public Image[] RateStars;

    public Tile[] tiles;
    public GameObject Astro;
    public Button RateB;
    public TMP_Text TReward;

    public DatabaseReference DbReference;

    private void Start()
    {
        DbReference = FirebaseDatabase.DefaultInstance.RootReference;
        Name = PlayerPrefs.GetString("NameLoadedLevel");
        LevelCode = PlayerPrefs.GetString("LoadedLevelCode");
        AstroPos = PlayerPrefs.GetString("AstroPosLoadedLevel");
        Reward = PlayerPrefs.GetString("RewardLoadedLevel");
        TReward.text = "+" + Reward;
        TReward.gameObject.SetActive(false);

        for (int i = 0; i < tiles.Length; i++)
        {
            AddOnTile(i, int.Parse(LevelCode[i].ToString()));
        }
        ChangeAstroStartPos(int.Parse(AstroPos));
    }

    public void Recievereward()
    {
        TReward.gameObject.SetActive(true);
        Prefs.IncreaseInt("OnlineCurrency", int.Parse(Reward));
    }

    public void AddOnTile(int tile, int type)
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
        //Debug.Log(type);
    }
    public void ChangeAstroStartPos(int tile)
    {
        PlayerPrefs.SetInt("MapAstroStartPos", tile);
        int posX = tile % 7;
        int posY = tile / 7;

        Astro.GetComponent<Player>().posX = posX;
        Astro.GetComponent<Player>().posY = posY;

        Astro.transform.position = new Vector3(tiles[tile].transform.position.x, Astro.transform.position.y, tiles[tile].transform.position.z);
    }

    public void SetRateTo(int rate)
    {
        Rate = rate;

        for(int i = 0; i < RateStars.Length; i++)
        {
            if (i < Rate)
                RateStars[i].color = Change.ColorA(RateStars[i].color, 255);
            else
                RateStars[i].color = Change.ColorA(RateStars[i].color, 150);
        }
    }

    public void RateLevel()
    {
        if (Rate == 0) return;
        Debug.LogWarning("Rating level");
        Destroy(RateB.gameObject);
        StartCoroutine(RateCoroutine());
    }
    IEnumerator RateCoroutine()
    {
        var checkTask = DbReference.Child("Maps").Child(Name).GetValueAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        DataSnapshot dt = checkTask.Result;
        if(!dt.Child("Ratings").Exists)
        {
            Debug.LogError("No child Ratings found, going the other way");
            DbReference.Child("Maps").Child(Name).Child("Ratings").Child("0").SetValueAsync(Rate);
            yield break;
        }


        var task = DbReference.Child("Maps").Child(Name).Child("Ratings").GetValueAsync();
        Debug.LogError("Rated Level but not yet");
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(task.Exception);
        }
        else if(task.Exception == null)
        {
            DataSnapshot d = task.Result;

            long a = d.ChildrenCount;

            DbReference.Child("Maps").Child(Name).Child("Ratings").Child(a.ToString()).SetValueAsync(Rate);

            int total = 0, index = 0;
            foreach(var child in d.Children)
            {
                index++;
                total += int.Parse(child.Value.ToString());
            }
           

            int raw = (int)Mathf.Round((float)((float)total / (float)index));
            Debug.LogError((float)((float)total / (float)index));
            DbReference.Child("Maps").Child(Name).Child("RawRating").SetValueAsync(raw);
        }



        Debug.LogError("Rated Level");
    }
}
