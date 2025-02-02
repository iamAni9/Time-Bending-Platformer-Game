using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarRewind : MonoBehaviour
{
    private bool isRewinding = false;
    private List<float> healthHistory = new List<float>(); 

    [SerializeField] private Image healthBar;
    [SerializeField] private float recordTime = 5f; 
    [SerializeField] private float rewindCooldown = 10f; 
    private bool canRewind = true;

    void FixedUpdate()
    {
        if (isRewinding)
        {
            RewindHealth();
        }
        else
        {
            RecordHealth();
        }
    }

    void RecordHealth()
    {
        int maxFrames = Mathf.RoundToInt(recordTime / Time.fixedDeltaTime);
        if (healthHistory.Count > maxFrames)
        {
            healthHistory.RemoveAt(0); 
        }

        if (healthBar != null)
        {
            healthHistory.Add(healthBar.fillAmount);
        }
    }

    void RewindHealth()
    {
        if (healthHistory.Count > 0 && healthBar != null)
        {
            healthBar.fillAmount = healthHistory[healthHistory.Count - 1];
            healthHistory.RemoveAt(healthHistory.Count - 1);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        if (canRewind && healthHistory.Count > 0)
        {
            isRewinding = true;
            canRewind = false;
            Invoke(nameof(ResetRewindCooldown), rewindCooldown);
        }
    }

    public void StopRewind()
    {
        isRewinding = false;
    }

    private void ResetRewindCooldown()
    {
        canRewind = true;
    }
}