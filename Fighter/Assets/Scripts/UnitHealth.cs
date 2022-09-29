using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    //fields: values that make up the class
    int _currentHealth;
    int _maxHealth;
    
    //constructor: used to instantiate the class
    public UnitHealth(int health, int max)
    {
        _maxHealth = max;
        _currentHealth = health;
    }

    //properties: allows access to fields to get or set 
    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    //methods
    public void dmgUnit(int dmg)
    {
        if(_currentHealth > 0)
        {
            if(_currentHealth - dmg < 0)
            {
                _currentHealth = 0;
            }
            else
            {
                _currentHealth -= dmg;
            }
        }
    }


    public void healUnit(int heal)
    {
        if (_currentHealth < _maxHealth)
        {
            if(_currentHealth + heal > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            else
            {
                _currentHealth += heal;
            }
        }
    }

}
