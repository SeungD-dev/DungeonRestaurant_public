using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_WaveSkipButton : MonoBehaviour
{
    public void OnWaveSkipButton()
    {
        GameManager.Instance.combatController.Debug_NextWave();
    }
}
