using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_GetStringUtil : MonoBehaviour
{
    public string key;
    [SerializeField] Text _targetText;

    private void OnEnable()
    {
        if(_targetText == null)
        {
            _targetText = GetComponent<Text>();
        }

        _targetText.SetTextWithStringKey(key);
    }
}
