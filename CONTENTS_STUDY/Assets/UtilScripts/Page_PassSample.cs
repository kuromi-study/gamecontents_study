using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page_PassSample : MonoBehaviour
{
    [SerializeField] private GameObject _scrollView;

    private void OnEnable()
    {
        for (int i = 0; i < 5; i++)
        {
            var passitem = Resources.Load<GameObject>("PAGE_PASS_PASSITEM");
            var passitem_object = Instantiate<GameObject>(passitem, _scrollView.transform);
            passitem_object.transform.localScale = Vector3.one - 2 * Vector3.up;
        }
    }
}
