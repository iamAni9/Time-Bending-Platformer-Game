using UnityEngine;

public class TimerActivation : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartAnim()
    {
        anim.SetBool("timer", true);
    }
    public void StopAnim()
    {
        anim.SetBool("timer", false);
    }
}
