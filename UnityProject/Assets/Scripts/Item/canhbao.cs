using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canhbao : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyAfterAnimation());
    }
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
