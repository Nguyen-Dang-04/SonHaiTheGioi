using UnityEngine;

public class KeyS : MonoBehaviour
{
    private Animator anim;
    private bool isSpessAtive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!isSpessAtive)
            {
                anim.SetBool("Press", true);
                isSpessAtive = true;
            }
        }
        else
        {
            if (isSpessAtive)
            {
                anim.SetBool("Press", false);
                isSpessAtive = false;
            }
        }
    }
}
