using System.Collections.Generic;
using UnityEngine;

public class RewindableObject : MonoBehaviour
{
    private bool isRewinding = false;
    private Rigidbody2D rb;
    private Animator anim;
    public TimerActivation timer;
    private ParticleSystem[] particles;

    private List<Vector2> positionHistory = new List<Vector2>();
    private List<float> rotationHistory = new List<float>();
    private List<float> animTimeHistory = new List<float>();
    private List<int> animStateHistory = new List<int>();

    [SerializeField] private float recordTime = 5f; // How many seconds to track rewind
    [SerializeField] private float rewindCooldown = 10f; // Cooldown after using rewind
    private bool canRewind = true; // Track if rewind is available
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    void Record()
    {
        int maxFrames = Mathf.RoundToInt(recordTime / Time.fixedDeltaTime);
        if (positionHistory.Count > maxFrames)
        {
            positionHistory.RemoveAt(0);
            rotationHistory.RemoveAt(0);
            animTimeHistory.RemoveAt(0);
            animStateHistory.RemoveAt(0);
        }

        positionHistory.Add(transform.position);
        rotationHistory.Add(transform.eulerAngles.z);

        if (anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            animStateHistory.Add(currentState.fullPathHash);
            animTimeHistory.Add(currentState.normalizedTime);
        }
    }

    void Rewind()
    {
        if (positionHistory.Count > 0)
        {
            transform.position = positionHistory[positionHistory.Count - 1];
            transform.rotation = Quaternion.Euler(0, 0, rotationHistory[rotationHistory.Count - 1]);

            positionHistory.RemoveAt(positionHistory.Count - 1);
            rotationHistory.RemoveAt(rotationHistory.Count - 1);

            if (anim != null && animStateHistory.Count > 0)
            {
                int stateHash = animStateHistory[animStateHistory.Count - 1];
                float playbackTime = animTimeHistory[animTimeHistory.Count - 1];

                anim.Play(stateHash, 0, playbackTime);

                animStateHistory.RemoveAt(animStateHistory.Count - 1);
                animTimeHistory.RemoveAt(animTimeHistory.Count - 1);
            }

            foreach (ParticleSystem ps in particles)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        if (canRewind && positionHistory.Count > 0)
        {
            isRewinding = true;
            canRewind = false;
            if (rb != null) rb.isKinematic = true;

            foreach (ParticleSystem ps in particles)
            {
                ps.Play();
                var main = ps.main;
                main.simulationSpeed = -1f;
            }
            timer.StartAnim();
            Invoke(nameof(ResetRewindCooldown), rewindCooldown);
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
        if (rb != null) rb.isKinematic = false;

        foreach (ParticleSystem ps in particles)
        {
            var main = ps.main;
            main.simulationSpeed = 1f;
            ps.Stop();
        }
    }

    private void ResetRewindCooldown()
    {
        canRewind = true;
        timer.StopAnim();
    }
}