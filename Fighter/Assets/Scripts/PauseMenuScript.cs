using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject firstSelected;

    // Start is called before the first frame update
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
