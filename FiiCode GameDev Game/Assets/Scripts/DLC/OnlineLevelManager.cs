using Firebase;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;


public class OnlineLevelManager : MonoBehaviour
{
    public DatabaseReference DbReference;
    public GameObject OnlineLevelTemplate;
    public TMP_InputField SearchBar;
    public GameObject NormalLevels, SearchLevels;

    public int MaxLoaded = 15;
    public bool IsReady;

    private void Start()
    {
        StartCoroutine(CheckAndFixDependancies());
        StartCoroutine(AwaitDependacies());
    }

    private IEnumerator AwaitDependacies()
    {
        yield return new WaitUntil(() => IsReady);

        DbReference.Child("Maps").ChildAdded += HandleChildAdded;
    }
    private void OnDestroy()
    {
        DbReference.Child("Maps").ChildAdded -= HandleChildAdded;
    }

    public void OnSearchSliderValueChanged()
    {
        if (SearchBar.text == null || SearchBar.text == "") return;

        Debug.Log("Searchh slider value changed");
        NormalLevels.SetActive(false);
        SearchLevels.SetActive(true);

        string toSearch = SearchBar.text;
        StartCoroutine(SearchLevel(toSearch));
    }
    public void OnDisSelectSlider()
    {
        SearchBar.text = null;

        NormalLevels.SetActive(true);
        SearchLevels.SetActive(false);

        for (int i = 0; i < SearchLevels.transform.childCount; i++)
        {
            Destroy(SearchLevels.transform.GetChild(i).gameObject);

        }
    }

    public bool IsLevelButtonLareadyLoaded(string name)
    {
        for (int i = 0; i < SearchLevels.transform.childCount; i++)
        {
            if (SearchLevels.transform.GetChild(i).GetComponent<OnlineLevelButton>().Name == name)
                return true;
        }
        return false;
    }

    private IEnumerator SearchLevel(string name)
    {
        Debug.LogWarning(name);
        var task = DbReference.Child("Maps").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.Log(task.Exception);
        }
        else
        {
            DataSnapshot snap = task.Result;

            foreach (var map in snap.Children)
            {
                if (map.Key.Contains(name))
                {
                    if (IsLevelButtonLareadyLoaded(map.Key))
                        yield break;

                    Debug.Log(map.Key);
                    GameObject g = Instantiate(OnlineLevelTemplate, SearchLevels.transform);
                    g.SetActive(true);
                    g.GetComponent<OnlineLevelButton>().Name = map.Key;
                    g.GetComponent<OnlineLevelButton>().Code = map.Child("MapCode").Value.ToString();
                    g.GetComponent<OnlineLevelButton>().AstroPos = map.Child("AstroPos").Value.ToString();
                    g.GetComponent<OnlineLevelButton>().Reward = map.Child("MapReward").Value.ToString();
                    g.GetComponent<OnlineLevelButton>().Difficulty = map.Child("MapDifficulty").Value.ToString();

                    int R = 0;
                    int nr = 0;

                    foreach (var rating in map.Child("Ratings").Children)
                    {
                        //Debug.Log(d.Child("Ratings").ChildrenCount + " " + d.Child("MapDifficulty").Value.ToString());
                        R += int.Parse(rating.Value.ToString());
                        nr++;
                    }

                    if (nr == 0)
                    {
                        g.GetComponent<OnlineLevelButton>().Rating = 0;
                    }
                    else
                        g.GetComponent<OnlineLevelButton>().Rating = R / nr;
                }
            }

            for (int i = 0; i < SearchLevels.transform.childCount; i++)
            {
                if (!SearchLevels.transform.GetChild(i).GetComponent<OnlineLevelButton>().Name.Contains(name))
                {
                    Destroy(SearchLevels.transform.GetChild(i).gameObject);
                }
            }
        }
    }


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (MaxLoaded <= 0)
            return;
        MaxLoaded--;

        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }


        DataSnapshot d = args.Snapshot;

        GameObject g = Instantiate(OnlineLevelTemplate, NormalLevels.transform);
        g.SetActive(true);
        g.GetComponent<OnlineLevelButton>().Name = d.Key;
        g.GetComponent<OnlineLevelButton>().Code = d.Child("MapCode").Value.ToString();
        g.GetComponent<OnlineLevelButton>().AstroPos = d.Child("AstroPos").Value.ToString();
        g.GetComponent<OnlineLevelButton>().Reward = d.Child("MapReward").Value.ToString();
        g.GetComponent<OnlineLevelButton>().Difficulty = d.Child("MapDifficulty").Value.ToString();

        int R = 0;
        int nr = 0;

        foreach (var rating in d.Child("Ratings").Children)
        {
            //Debug.Log(d.Child("Ratings").ChildrenCount + " " + d.Child("MapDifficulty").Value.ToString());
            R += int.Parse(rating.Value.ToString());
            nr++;
        }

        if (nr == 0)
        {
            g.GetComponent<OnlineLevelButton>().Rating = 0;
        }
        else
            g.GetComponent<OnlineLevelButton>().Rating = R / nr;
    }
    private IEnumerator CheckAndFixDependancies()
    {
        var checkAndFixDependanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependanciesTask.IsCompleted);

        var dependancyResult = checkAndFixDependanciesTask.Result;

        if (dependancyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError("Error on Initialize");
        }
    }
    private void InitializeFirebase()
    {
        DbReference = FirebaseDatabase.DefaultInstance.RootReference;
        IsReady = true;
    }


}
