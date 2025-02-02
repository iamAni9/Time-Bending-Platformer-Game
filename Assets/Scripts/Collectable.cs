using UnityEngine;
using TMPro;

public class Collectable : MonoBehaviour
{
    public TMP_Text lifeCnt;  
    public TMP_Text clockCnt;  
    public TMP_Text keyCnt;  

    public void ChangeLifeCnt(int val)
    {
        int currentValue = int.Parse(lifeCnt.text); 
        currentValue += val;
        lifeCnt.text = currentValue.ToString();
    }
    public void ChangeClockCnt(int val)
    {
        int currentValue = int.Parse(clockCnt.text);
        currentValue += val;
        clockCnt.text = currentValue.ToString();
    }
    public void ChangeKeyCnt(int val)
    {
        int currentValue = int.Parse(keyCnt.text); 
        currentValue += val;
        keyCnt.text = currentValue.ToString();
    }

    public bool LifeCount()
    {
        return int.Parse(lifeCnt.text) > 0;
    }
    public bool ClockCount()
    {
        return int.Parse(clockCnt.text) > 0;
    }
    public bool KeyCount()
    {
        return int.Parse(keyCnt.text) > 0;
    }
}
