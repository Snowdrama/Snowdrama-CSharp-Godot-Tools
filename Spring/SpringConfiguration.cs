using Godot;

namespace Snowdrama.Spring
{
    [System.Serializable]
    public struct SpringConfiguration
    {
        public float Mass;
        public float Tension;
        public float Friction;
        public float Precision;
        public bool Clamp;
        public Vector2 ClampRange;

        public SpringConfiguration()
        {
            Mass = 1f;
            Tension = 170.0f;
            Friction = 26.0f;
            Precision = 0.01f;
            Clamp = false;
            ClampRange = new Vector2();
        }
        public SpringConfiguration(float Mass = 1f, float Tension = 170.0f, float Friction = 26.0f, float Precision = 0.01f, bool Clamp = false, Vector2 ClampRange = new Vector2())
        {
            //Debug.Log($"{Mass}");
            //Debug.Log($"{Tension}");
            //Debug.Log($"{Friction}");
            this.Mass = Mass;
            this.Tension = Tension;
            this.Friction = Friction;
            this.Precision = Precision;
            this.Clamp = Clamp;
            this.ClampRange = ClampRange;
        }
    }
}