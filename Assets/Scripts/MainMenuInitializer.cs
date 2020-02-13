using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicBox.GetInstance().Play(0);
    }
}
