using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _contentText;

    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Vector2Reference _mousePosition;

    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            _headerText.gameObject.SetActive(false);
        }
        else
        {
            _headerText.gameObject.SetActive(true);
            _headerText.text = header;
        }

        _contentText.text = content;
        
        UpdateTooltipBoxSize();
    }

    private void UpdateTooltipBoxSize()
    {
        int headerLength = _headerText.text.Length;
        int contentLength = _headerText.text.Length;
        
        _layoutElement.enabled = Math.Max(_headerText.preferredWidth, _contentText.preferredWidth) >= _layoutElement.preferredWidth;
    }
    
    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateTooltipBoxSize();
        }

        Vector2 _pivot = new Vector2(_mousePosition.Value.x / Screen.width, _mousePosition.Value.y / Screen.height);
        _rectTransform.pivot = _pivot;
        
        transform.position = _mousePosition.Value;
    }
}
