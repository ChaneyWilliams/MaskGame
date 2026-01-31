using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] MainMenuManager.MainMenuButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.MainMenuButtonClicked(_buttonType);
    }
}
