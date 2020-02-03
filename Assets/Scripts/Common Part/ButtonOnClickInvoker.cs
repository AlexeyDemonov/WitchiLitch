using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonOnClickInvoker : MonoBehaviour
{
    public KeyCode KeyCode;

    Button _button;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode))
        {
            _button.onClick.Invoke();
        }
    }
}
