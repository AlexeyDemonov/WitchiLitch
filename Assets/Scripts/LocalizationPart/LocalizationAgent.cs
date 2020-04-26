using UnityEngine;
using UnityEngine.UI;

public class LocalizationAgent : MonoBehaviour
{
    public Text[] Texts;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (var text in Texts)
        {
            text.text = LocalizationCore.Localize(text.text);
        }
    }
}