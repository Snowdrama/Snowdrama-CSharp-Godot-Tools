using Godot;
//Min 2D representation of a spring
namespace Snowdrama.Spring
{
    public class Spring2D
    {
        private SpringList springCollection;
        private int xID;
        private int yID;

        public Vector2 Value
        {
            get
            {
                return new Vector2(springCollection.GetValue(xID), springCollection.GetValue(yID));
            }
            set
            {
                springCollection.SetValue(xID, value.X);
                springCollection.SetValue(yID, value.Y);
            }
        }
        public Vector2 Target
        {
            get
            {
                return new Vector2(springCollection.GetTarget(xID), springCollection.GetTarget(yID));
            }
            set
            {
                springCollection.SetTarget(xID, value.X);
                springCollection.SetTarget(yID, value.Y);
            }
        }
        public Vector2 Velocity
        {
            get
            {
                return new Vector2(springCollection.GetVelocity(xID), springCollection.GetVelocity(yID));
            }
            set
            {
                springCollection.SetVelocity(xID, value.X);
                springCollection.SetVelocity(yID, value.Y);
            }
        }

        public SpringConfiguration SpringConfig
        {
            set
            {
                springCollection.SetSpringConfig(xID, value);
                springCollection.SetSpringConfig(yID, value);
            }
        }

        public Spring2D(Vector2 initialValue = default)
        {
            SpringConfiguration defaultConfig = new SpringConfiguration()
            {
                Mass = 1f,
                Tension = 170.0f,
                Friction = 26.0f,
                Precision = 0.01f,
                Clamp = false,
            };
            springCollection = new SpringList();
            xID = springCollection.Add(initialValue.X, defaultConfig, new Vector2(-1, 1));
            yID = springCollection.Add(initialValue.Y, defaultConfig, new Vector2(-1, 1));
        }
        public Spring2D(SpringConfiguration config, Vector2 initialValue = default)
        {
            springCollection = new SpringList();
            xID = springCollection.Add(initialValue.X, config, config.ClampRangeX);
            yID = springCollection.Add(initialValue.Y, config, config.ClampRangeY);
        }

        public void Update(float deltaTime)
        {
            springCollection.Update(deltaTime);
        }
        public void Update(double deltaTime)
        {
            Update((float)deltaTime);
        }
    }
}