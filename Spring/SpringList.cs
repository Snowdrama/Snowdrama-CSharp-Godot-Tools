using Godot;
using System.Collections.Generic;

namespace Snowdrama.Spring
{
    public class SpringList
    {
        public int Count => _states.Count;

        private List<SpringConfiguration> _springConfigs;
        private List<SpringState> _states;
        private List<Vector2> _clamps;

        public SpringList()
        {
            _springConfigs = new List<SpringConfiguration>();
            _states = new List<SpringState>();
        }

        public SpringList(int capacity)
        {
            _springConfigs = new List<SpringConfiguration>(capacity);
            _states = new List<SpringState>(capacity);
        }

        public int Add(float initialValue, SpringConfiguration springConfig, Vector2 clamp)
        {
            var id = Count;
            var state = new SpringState(initialValue, initialValue, 0f, clamp);
            _springConfigs.Add(springConfig);
            _states.Add(state);

            return id;
        }
        public bool IsResting(int id)
        {
            return _states[id].Resting;
        }

        public float GetValue(int id)
        {
            if (_springConfigs[id].Clamp)
            {
                return Mathf.Clamp(_states[id].Current, _states[id].Clamp.X, _states[id].Clamp.Y);
            }
            return _states[id].Current;
        }

        public float GetTarget(int id)
        {
            return _states[id].Target;
        }

        public float GetVelocity(int id)
        {
            return _states[id].Velocity;
        }

        public void SetValue(int id, float value)
        {
            var state = _states[id];
            state.Current = value;
            state.Velocity = 0f;
            _states[id] = state;
        }

        public void SetTarget(int id, float value)
        {
            var state = _states[id];
            state.Target = value;
            _states[id] = state;
        }

        public void SetVelocity(int id, float value)
        {
            var state = _states[id];
            state.Velocity = value;
            _states[id] = state;
        }

        public void SetSpringConfig(int id, SpringConfiguration springConfig)
        {
            _springConfigs[id] = springConfig;
        }

        private void UpdateValue(int id, float deltaTime)
        {
            var state = _states[id];
            var config = _springConfigs[id];

            while (deltaTime >= Mathf.Epsilon)
            {
                // Debug.Log($"config.Tension {config.Tension}");
                // Debug.Log($"config.Damping {config.Damping}");
                // Debug.Log($"config.Mass {config.Mass}");
                var dt = Mathf.Min(deltaTime, 0.016f);
                var force = -config.Tension * (state.Current - state.Target);
                var damping = -config.Friction * state.Velocity;
                var acceleration = (force + damping) / config.Mass;
                state.Velocity = state.Velocity + (acceleration * dt);
                state.Current = state.Current + (state.Velocity * dt);

                // Debug.Log($"state.Target {state.Target}");
                // Debug.Log($"state.Current {state.Current}");
                // Debug.Log($"state.Current {state.Velocity}");
                if (Mathf.Abs(state.Velocity) < config.Precision && Mathf.Abs(state.Current - state.Target) < config.Precision)
                {
                    state.Current = state.Target;
                    state.Velocity = 0f;
                    state.Resting = true;
                    _states[id] = state;
                    return;
                }
                else
                {
                    state.Resting = false;
                }

                //if (config.Clamp)
                //{
                //    state.Current = Mathf.Clamp(state.Current, config.ClampRange.X, config.ClampRange.Y);
                //}

                deltaTime -= dt;
            }


            _states[id] = state;
        }
        public void Update(float deltaTime)
        {
            for (var i = 0; i < Count; i++)
            {
                UpdateValue(i, deltaTime);
            }
        }
    }
}