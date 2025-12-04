using Godot;
using System;

namespace Snowdrama.Spring
{
    [GlobalClass]
    public partial class SpringConfigurationResource : Resource
    {
        [Export] public float Mass = 1;
        [Export] public float Tension = 170;
        [Export] public float Friction = 26;
        [Export] public float Precision = 0.01f;
        [Export] public bool Clamp;
        [Export] public Vector2 ClampRangeX = new Vector2(-1.0f, 1.0f);
        [Export] public Vector2 ClampRangeY = new Vector2(-1.0f, 1.0f);
        [Export] public Vector2 ClampRangeZ = new Vector2(-1.0f, 1.0f);
        [Export] public Vector2 ClampRangeW = new Vector2(-1.0f, 1.0f);
        private SpringConfiguration _config;
        private bool init = false;
        public SpringConfiguration Config
        {
            get
            {
                if (!init)
                {
                    init = true;
                    _config = new SpringConfiguration();
                    _config.Mass = Mass;
                    _config.Tension = Tension;
                    _config.Friction = Friction;
                    _config.Precision = Precision;
                    _config.Clamp = Clamp;
                    _config.ClampRangeX = ClampRangeX;
                    _config.ClampRangeY = ClampRangeY;
                    _config.ClampRangeZ = ClampRangeZ;
                    _config.ClampRangeW = ClampRangeW;
                }
                return _config;
            }
        }

    }
}