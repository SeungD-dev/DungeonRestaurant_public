using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementButton : MonoBehaviour
{
    public GameObject Management;

    public void OnClickManagementButton()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 30)
        {
            ToturialsManager.Instance.isClear[13] = true;
        }
        Instantiate(Management);
    }
}
