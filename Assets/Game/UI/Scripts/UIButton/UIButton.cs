using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour
{
    protected Button button;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButtonMethod);
    }
    private void OnDestroy() =>
        button.onClick.RemoveListener(ClickButtonMethod);
    protected abstract void ClickButtonMethod();
}