using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;


public class CombatDebug : OdinEditorWindow
{
    [MenuItem("Debug Tools/Combat")]
    private static void OpenWindow()
    {
        GetWindow<CombatDebug>().Show();
    }

    [BoxGroup("Card")]
    [ShowInInspector, ReadOnly] private CardDeck cardDeck = CardDeck.Instance;

    [DisableInEditorMode]
    bool isCardDeckNotExist = true;

    [OnInspectorGUI]
    [DisableInEditorMode]
    private void check()
    {
        if (CardDeck.Instance != null)
        { 
            isCardDeckNotExist = false;
        }
        else
        { 
            isCardDeckNotExist = true;
        }
    }

    [BoxGroup("Card")]
    [DisableInEditorMode]
    [ReadOnly]
    [DisableIf("isCardDeckNotExist")]
    public RecipeData currentRecipe;

    [BoxGroup("Card")]
    [HorizontalGroup("Card/Row1")]
    [PropertyOrder(1)]
    [DisableInEditorMode]
    [Button("-", ButtonSizes.Small)]
    public void BeforeRecipe()
    {
        currentRecipe = RecipeDataManager.Instance.recipes[--index];
    }

    [BoxGroup("Card")]
    [HorizontalGroup("Card/Row1")]
    [PropertyOrder(2)]
    [DisableInEditorMode]
    [HideLabel]
    public int index;

    [BoxGroup("Card")]
    [HorizontalGroup("Card/Row1")]
    [PropertyOrder(3)]
    [DisableInEditorMode]
    [Button("+", ButtonSizes.Small)]
    public void NextRecipe()
    {
        currentRecipe = RecipeDataManager.Instance.recipes[++index];
    }

    [BoxGroup("Card")]
    [DisableInEditorMode]
    [DisableIf("isCardDeckNotExist")]
    [Button("Draw")]
    public void Draw()
    {
        CardDeck.Instance.Darw(currentRecipe);
    }

}
