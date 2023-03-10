using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private Slider _heatBar;

    public Slider HeatBar
    {
        get { return _heatBar; }
    }

    [SerializeField] private TextMeshProUGUI _amminitionText;

    public TextMeshProUGUI AmmunitionText
    {
        get { return _amminitionText; }
    }
}
