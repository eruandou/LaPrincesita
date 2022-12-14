using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevelSelect
{
    public class LevelSelectPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Animator animator;

        private LevelNode _currentNode;
        private bool _isMoving;

        private static readonly int Move = Animator.StringToHash("Move");

        private void Start()
        {
            var actions = playerInput.actions;

            SetCallbacks(actions);
        }

        private void SetCallbacks(InputActionAsset inputActionAsset)
        {
            var move = inputActionAsset["MoveOption"];
            var select = inputActionAsset["Select"];

            move.performed += TryMove;
            select.performed += TrySelect;
        }

        private void OnDestroy()
        {
            var actions = playerInput.actions;
            var move = actions["MoveOption"];
            var select = actions["Select"];
            move.performed -= TryMove;
            select.performed -= TrySelect;
        }

        private void TryMove(InputAction.CallbackContext ctx)
        {
            if (_currentNode == default || _isMoving)
                return;
            var inputValue = ctx.ReadValue<Vector2>();
            var x = Mathf.Abs(inputValue.x);
            var y = Mathf.Abs(inputValue.y);

            LevelNode nodeToMoveTo;
            if (x > y)
            {
                nodeToMoveTo = inputValue.x > 0 ? _currentNode.GetRightConnection() : _currentNode.GetLeftConnection();
            }
            else if (x < y)
            {
                nodeToMoveTo = inputValue.y > 0 ? _currentNode.GetUpConnection() : _currentNode.GetDownConnection();
            }
            else
            {
                nodeToMoveTo = inputValue.x > 0 ? _currentNode.GetRightConnection() : _currentNode.GetLeftConnection();
            }

            if (nodeToMoveTo == default || nodeToMoveTo.IsLocked)
                return;
            MoveToNode(nodeToMoveTo);
        }

        private void MoveToNode(LevelNode nodeToMoveTo)
        {
            var distanceAxis = (nodeToMoveTo.transform.position - transform.position).normalized;

            transform.localScale = transform.localScale.xYZ(distanceAxis.x > 0 ? 2 : -2);

            animator.SetBool(Move, true);
            _currentNode = nodeToMoveTo;
            var positionToMoveTo = _currentNode.transform.position;
            _isMoving = true;
            var lerpMovement = transform.DOMove(positionToMoveTo, 2);
            lerpMovement.onComplete += FinishMovement;
        }

        private void FinishMovement()
        {
            _isMoving = false;
            animator.SetBool(Move, false);
        }

        private void TrySelect(InputAction.CallbackContext ctx)
        {
            if (_isMoving || _currentNode == default || !ctx.ReadValueAsButton())
                return;
            LevelSelectMap.lastVisitedNode = _currentNode.NodeNumber;
            GameManager.Instance.CustomSceneManager.ChangeScene(_currentNode.LevelData.levelID);
        }

        public void SetToNode(LevelNode node)
        {
            _currentNode = node;
            transform.position = _currentNode.transform.position;
        }
    }
}