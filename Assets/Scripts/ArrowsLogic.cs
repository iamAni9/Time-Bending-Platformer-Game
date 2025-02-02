using UnityEngine;
using System.Collections;
public class ArrowsLogic : MonoBehaviour
{
    public Animator arrowAnim;
    public TimeManager timeManager;

    void Start()
    {
        if (arrowAnim == null)
            arrowAnim = GetComponent<Animator>(); 
    }

    public void ArrowAttack()
    {
        if (arrowAnim != null && !timeManager.isRewinding)
        {
            arrowAnim.SetBool("arrowAttack", true);
        }
        else
        {
            Debug.LogError("Animator is missing on " + gameObject.name);
        }
    }
}
