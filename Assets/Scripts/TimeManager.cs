using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private List<RewindableObject> rewindableObjects = new List<RewindableObject>();
    // private bool isRewinding = false;
    public ParticleSystem particleEffect;   
    public HealthBarRewind healthBarRewind;
    public Collectable collectable;
    public Animator arrowAnim;
    public bool isRewinding = false;
    void Awake()
    {
        particleEffect.Stop();
    }
    void Start()
    {
        // arrowAnim = GetComponent<Animator>();
        RewindableObject[] objects = FindObjectsOfType<RewindableObject>();
        rewindableObjects.AddRange(objects);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && collectable.ClockCount())
        {
            isRewinding = true;
            StartRewind();
            healthBarRewind.StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            isRewinding = false;
            StopRewind();
            healthBarRewind.StopRewind();
        }
    }

    void StartRewind()
    {
        particleEffect.Play();
        arrowAnim.SetBool("arrowAttack", false);
        collectable.ChangeClockCnt(-1);
        foreach (var obj in rewindableObjects)
        {
            obj.StartRewind();
        }
    }

    void StopRewind()
    {
        particleEffect.Stop();
        // isRewinding = false;
        foreach (var obj in rewindableObjects)
        {
            obj.StopRewind();
        }
    }
}