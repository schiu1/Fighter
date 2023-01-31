using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider _healthSlider;

    void Start()
    {
        _healthSlider = gameObject.GetComponent<Slider>();
    }

    //prob won't use this
    public void SetMaxHealth(int max)
    {
        _healthSlider.maxValue = max;
        _healthSlider.value = max;
    }

    public void SetHealth(int health)
    {
        _healthSlider.value = health;
    }

}
