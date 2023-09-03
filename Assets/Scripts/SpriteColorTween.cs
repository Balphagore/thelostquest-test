using DG.Tweening;
using UnityEngine;

public static class SpriteColorTween
{
    public static Tween AnimateColor(SpriteRenderer spriteRenderer, Color targetColor, float duration)
    {
        Color originalColor = spriteRenderer.color;

        Sequence colorSequence = DOTween.Sequence();
        colorSequence.Append(spriteRenderer.DOColor(targetColor, duration / 2));
        colorSequence.Append(spriteRenderer.DOColor(originalColor, duration / 2));

        return colorSequence;
    }
}
