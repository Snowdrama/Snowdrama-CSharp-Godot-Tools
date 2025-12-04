using Godot;

namespace Snowdrama.Spring
{
    public struct SpringState
    {
        public float Target;
        public float Current;
        public float Velocity;
        public bool Resting;
        public Vector2 Clamp;

        public SpringState(float target, float current, float velocity, Vector2 clamp)
        {
            Target = target;
            Current = current;
            Velocity = velocity;
            Resting = true;
            Clamp = clamp;
        }
    }
}