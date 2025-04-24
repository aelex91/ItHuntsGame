using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractPopupUI : MonoBehaviour
{

    private TextMeshProUGUI text;
    private Image image;

    [SerializeField]
    private Color imageColor;
    [SerializeField]
    private Color textColor;


    public bool IsInteracting;

    void Start()
    {

        image = GetComponentInChildren<Image>(true);

        text = GetComponentInChildren<TextMeshProUGUI>(true);

        image.Guard(this.name);

        text.Guard(this.name);

    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
