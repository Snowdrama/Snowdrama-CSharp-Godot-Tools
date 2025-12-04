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
        public Vector2 ClampRangeX;
        public Vector2 ClampRangeY;
        public Vector2 ClampRangeZ;
        public Vector2 ClampRangeW;

        public SpringConfiguration()
        {
            Mass = 1f;
            Tension = 170.0f;
            Friction = 26.0f;
            Precision = 0.01f;
            Clamp = false;
            ClampRangeX = new Vector2(-1, 1);
            ClampRangeY = new Vector2(-1, 1);
            ClampRangeZ = new Vector2(-1, 1);
            ClampRangeW = new Vector2(-1, 1);
        }
        public SpringConfiguration(float Mass = 1f,
            float Tension = 170.0f,
            float Friction = 26.0f,
            float Precision = 0.01f,
            bool Clamp = false,
            Vector2 ClampRangeX = new Vector2(),
            Vector2 ClampRangeY = new Vector2(),
            Vector2 ClampRangeZ = new Vector2(),
            Vector2 ClampRangeW = new Vector2())
        {
            this.Mass = Mass;
            this.Tension = Tension;
            this.Friction = Friction;
            this.Precision = Precision;
            this.Clamp = Clamp;
            this.ClampRangeX = ClampRangeX;
            this.ClampRangeY = ClampRangeY;
            this.ClampRangeZ = ClampRangeZ;
            this.ClampRangeW = ClampRangeW;
        }
    }
}