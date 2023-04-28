using System;
using System.Collections.Generic;
using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Base;

namespace PhlegmaticOne.SharpTennis.Game.Common.Tween
{
    public class DoTweenManager : BehaviorObject
    {
        private static DoTweenManager _instance;
        public static DoTweenManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var go = new GameObject("DoTweenManager");
                var manager = new DoTweenManager();
                go.AddComponent(manager);
                _instance = manager;
                return _instance;
            }
        }

        private readonly List<ITweenAction> _actions = new List<ITweenAction>();

        public void KillTweensWithTransform(Transform transform, Action onKill)
        {
            for (var i = _actions.Count - 1; i >= 0; i--)
            {
                var action = _actions[i];

                if (action.Transform == transform)
                {
                    action.OnKill(onKill);
                    _actions.Remove(action);
                }
            }
        }

        public void AddAction(ITweenAction action)
        {
            _actions.Add(action);
        }

        protected override void Update()
        {
            foreach (var tweenAction in _actions.ToArray())
            {
                tweenAction.Update();
            }

            _actions.RemoveAll(x => x.IsFinished);
        }
    }
}
