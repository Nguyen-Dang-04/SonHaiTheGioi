using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private Animator anim;
    public GameObject bat;

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

    public void Instantiate()
    {
        Instantiate(bat, transform.position, transform.rotation);
    }
}
