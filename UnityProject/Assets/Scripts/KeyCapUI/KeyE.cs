using UnityEngine;

public class KeyE : MonoBehaviour
{
    private Animator anim;
    private bool isSpessAtive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
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
