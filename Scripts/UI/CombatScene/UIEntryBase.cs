using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//TODO : UIBase 상속하여 활용토록 수정하기?
public class UIEntryBase : UIBase
{
    [SerializeField] protected Transform background;
    [SerializeField] protected GameObject slotPrefab;

    [SerializeField] protected ToggleGroup toggleGroup;
    [SerializeField] protected Toggle currentActiveToggle;

    protected virtual void Awake()
    {
        toggleGroup = background.GetComponent<ToggleGroup>();
    }

    public virtual void OnToggleValueChanged(bool isOn)
    {
        currentActiveToggle = toggleGroup.ActiveToggles().FirstOrDefault();
    }
}
