using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TooltipSystem : MonoBehaviourSingleton<TooltipSystem>
{
    private Tween _delayTween;
    private CanvasGroup _canvasGroup;
    
    [SerializeField] private float _delayDuration;
    [SerializeField] private Tooltip _tooltip;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(string content, string header = "")
    {
        _delayTween = DOVirtual.DelayedCall(_delayDuration, () =>
        {
            _canvasGroup.DOFade(1f, 0.25f);
            Instance._tooltip.SetText(content,header);
            Instance._tooltip.gameObject.SetActive(true);
        });
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, 0.25f);
        _delayTween.Rewind();
        Instance._tooltip.gameObject.SetActive(false);
    }
}
