using DG.Tweening;
using UnityEngine;

namespace TheLostQuestTest.Characters
{
    public abstract class CharacterViewBase : MonoBehaviour, ICharacterView
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private ICharacterController characterController;
        private Tween colorTween;

        public virtual void Initialize(ICharacterController characterController)
        {
            this.characterController = characterController;
            characterController.DestroyViewAction += OnDestroyViewAction;
            characterController.TakeStrikeAction += OnTakeStrikeAction;
        }

        public virtual void OnTakeStrikeAction()
        {
            if (spriteRenderer != null)
            {
                if (colorTween == null)
                {
                    colorTween = SpriteColorTween.AnimateColor(spriteRenderer, Color.red, 1f);
                    colorTween.OnComplete(() => colorTween = null);
                }
            }
        }

        protected virtual void OnDestroyViewAction()
        {
            characterController.DestroyViewAction -= OnDestroyViewAction;
            characterController.TakeStrikeAction -= OnTakeStrikeAction;
            characterController = null;
            spriteRenderer = null;
            if (colorTween != null)
            {
                colorTween.Kill();
            }
            Destroy(gameObject);
        }
    }
}
