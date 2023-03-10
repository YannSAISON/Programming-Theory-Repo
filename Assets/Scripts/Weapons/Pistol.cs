using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    new void Awake()
    {
        _cadency = 0.2f;
        _damages = 2;
        _magazineCapacity = 9;
        _rechargeTime = 1.5f;
        base.Awake();
    }
}
