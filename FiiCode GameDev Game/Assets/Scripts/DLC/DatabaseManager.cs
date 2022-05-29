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

public class DatabaseManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependecyStatus;
    public FirebaseAuth auth;
    public DatabaseReference DBreference;

    [Header("Other")]
    public static DatabaseManager Instance;
    public string Result;
    public static bool IsReady;

    void Awake()
    {
        IsReady = false;
        Instance = this;
        StartCoroutine(CheckAndFixDependancies());
    }

    public static FirebaseAuth Auth() => Instance.auth;
    public static DatabaseReference DbReference() => Instance.DBreference;


    public static void RecieveFromDatabase(string path, Ref<string> reff)
    {
        Instance.StartCoroutine(Instance.IRecieveFromDatabase(path, reff));
    }

    public static void UpdateDatabase(string path, string key) => Instance.StartCoroutine(Instance.IUpdateDatabase(path, key));
    public static void UpdateDatabase(string path, string path2, string key) => Instance.StartCoroutine(Instance.IUpdateDatabase(path, path2, key));

    public IEnumerator IUpdateDatabase(string path, string key)
    {
        var task = DBreference.Child("Maps").Child(path).SetValueAsync(key);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        Debug.LogError(key + " " + DBreference.Child("Maps").Child(path).GetValueAsync().Result.Value.ToString());
    }
    public IEnumerator IUpdateDatabase(string path, string path2, string key)
    {
        var task = DBreference.Child("Maps").Child(path).Child(path2).SetValueAsync(key);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        Debug.LogError(key + " " + DBreference.Child("Maps").Child(path).Child(path2).GetValueAsync().Result.Value.ToString());
    }

    public IEnumerator IRecieveFromDatabase(string path, Ref<string> reff)
    {
        var task = Instance.DBreference.Child("Maps").Child(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
        else if (task.Result == null)
        {
            Debug.LogWarning("No data can be found");
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            reff.Value = snapshot.Value.ToString();
        }
    }

    private IEnumerator CheckAndFixDependancies()
    {
        var checkAndFixDependanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependanciesTask.IsCompleted);

        var dependancyResult = checkAndFixDependanciesTask.Result;

        if (dependancyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
            Debug.Log("Initialized firebase");
        }
        else
        {
            Debug.LogError("Error on Initialize");
        }
    }
    private void InitializeFirebase()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        IsReady = true;
    }
}

public class Ref<T>
{
    private T backing;
    public T Value { get { return backing; } set { backing = value; } }
    public Ref(T reference)
    {
        backing = reference;
    }
}

