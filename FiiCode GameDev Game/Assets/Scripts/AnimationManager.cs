using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class AnimationManager : MonoBehaviour
{
    public Animator animator;

    public int State { get { return animator.GetInteger("state"); } set { animator.SetInteger("state", value); } }

    private void Awake()
    {
        StartCoroutine(IdleTimer());
    }

    public IEnumerator IdleTimer()
    {
        yield return new WaitUntil(() => State == 0);
        int a = Random.Range(4, 7);

        yield return new WaitForSeconds(a);

        if (State >= 10) { StartCoroutine(IdleTimer()); yield break; }

        State = Random.Range(2, 6);

        yield return new WaitForSeconds(3);

        if (State >= 10) { StartCoroutine(IdleTimer()); yield break; }

        State = 0;

        StartCoroutine(IdleTimer());    
    }
}
