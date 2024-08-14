using UnityEngine;

public class InstantiateUIBtn : MonoBehaviour
{
    public GameObject UI;

    public void OpenInstantiateUI()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 3)
        {
            ToturialsManager.Instance.isClear[0] = true;
        }
        Instantiate(UI);
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 6)
        {
            ToturialsManager.Instance.isClear[1] = true;
        }
        Destroy(UI);
    }
}
