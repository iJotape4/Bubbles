using UnityEngine;

public class EnableOrDisableObjectButton : UIButton
{
    [SerializeField] GameObject objectToEnableOrDisable;

    protected override void ClickButtonMethod()
    {
        objectToEnableOrDisable.SetActive(!objectToEnableOrDisable.activeSelf);
    }
}