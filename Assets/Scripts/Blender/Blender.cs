using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Food;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blender
{
    public class Blender : MonoBehaviour, IBlender
    {
        private static readonly int AnimationOpenTriggerName = Animator.StringToHash("Open");
        private static readonly int AnimationCloseTriggerName = Animator.StringToHash("Close");
        
        public event Action<Ingredient> IngredientMixed;
        public event Action ShakeCompleted; 

        [SerializeField] private float _shakeTime;
        [SerializeField] private float _shakeStrength; 
        [SerializeField] private IngredientsMixer _ingredientsMixer;
        [SerializeField] private float _maxHeight;
        [SerializeField] private MeshRenderer _liquidMat;
        [SerializeField] private ParticleSystem _shakeEffect;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _timeBeforeLidClose;
        [SerializeField] private GameState _gameState;
        [SerializeField] private ShakeButton _shakeButton;
        [SerializeField] private IngredientsManager _ingredientsManager;
        
        private Coroutine _shakeCoroutine;
        private Coroutine _waitForCloseCoroutine;

        private List<Ingredient> _mixedIngredients;
        
        public void Clean()
        {
            _liquidMat.sharedMaterial.SetFloat("_Fill", 0f);
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            
            _ingredientsMixer.gameObject.SetActive(false);
            _ingredientsMixer.transform.localPosition = Vector3.zero;
            
            _mixedIngredients.Clear();
        }
        
        private void Awake()
        {
            _mixedIngredients = new List<Ingredient>();
            
            _ingredientsMixer.IngredientMixed += OnIngredientMixed;
            _shakeButton.ShakeClicked += OnShakeClicked;
            _ingredientsManager.IngredientClicked += OnIngredientClicked;
            Clean();
        }

        private void OnIngredientClicked(Ingredient ingredient)
        {
            OpenLid();
        }
        
        private void OpenLid()
        {
            if (_waitForCloseCoroutine != null)
            {
                StopCoroutine(_waitForCloseCoroutine);
                _waitForCloseCoroutine = null;
            }
            else
            {
                _animator.SetTrigger(AnimationOpenTriggerName);
            }
            
            _waitForCloseCoroutine = StartCoroutine(WaitBeforeCloseRoutine());
        }

        private void OnShakeClicked()
        {
            //if(_gameState.State != GameSateType.Ordering) return;
            
            //_gameState.State = GameSateType.Shaking;
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            
            _shakeCoroutine = StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            _ingredientsMixer.gameObject.SetActive(true);
            _ingredientsMixer.transform.DOLocalMoveY(_maxHeight, _shakeTime);
            
            transform.DOShakeRotation(_shakeTime, _shakeStrength, fadeOut:false);
            
            _shakeEffect.Play();
            
            var fillProgress = 0f;
            var startTime = Time.time;
            while (fillProgress < 1f)
            {
                fillProgress = (Time.time - startTime) / _shakeTime;
                _liquidMat.sharedMaterial.SetFloat("_Fill", fillProgress);
                yield return null;
            }
            
            _shakeEffect.Stop();
            
            ShakeCompleted?.Invoke();
        }

        private IEnumerator WaitBeforeCloseRoutine()
        {
            yield return new WaitForSeconds(_timeBeforeLidClose);
            
            _animator.SetTrigger(AnimationCloseTriggerName);

            _waitForCloseCoroutine = null;
        }

        private void OnIngredientMixed(Ingredient ingredient)
        {
            _mixedIngredients.Add(ingredient);
            
            UpdateColor();
            
            IngredientMixed?.Invoke(ingredient);
        }
        
        private void UpdateColor()
        {
            var color = ColorHelper.GetColorFromIngredients(_mixedIngredients);
            
            _liquidMat.sharedMaterial.SetColor("_Color", color);

            var main = _shakeEffect.main;
            main.startColor = color;
        }
    }
}