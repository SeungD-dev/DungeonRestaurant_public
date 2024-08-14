using UnityEngine;
using UnityEngine.UI;

public class Debug_Resurrect : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnResurrectButton);
    }

    public void OnResurrectButton()
    {
        GameManager.Instance.combatController.Debug_ResurrectPlayerCharacters();
    }
}
