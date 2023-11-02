using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator anim;
    public bool test = false;
    public bool test2 = false;
    public bool test3 = false;
    public bool test4 = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            anim.SetTrigger("Run");
        }
        if (test2)
        {
            anim.SetTrigger("Jump");
        }
        if (test3)
        {
            anim.SetTrigger("Dead");
        }
        if (test4)
        {
            anim.SetTrigger("Attack");
        }
    }
}
