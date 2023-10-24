using Godot;
using System;
using System.Collections.Generic;
//A 2D representation of a spring
namespace Snowdrama.Spring
{
    public class Spring2D
    {
        SpringList springCollection;
        int xID;
        int yID;

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

        public Spring2D(SpringConfiguration config, Vector2 initialValue = default)
        {
            springCollection = new SpringList();
            xID = springCollection.Add(initialValue.X, config);
            yID = springCollection.Add(initialValue.Y, config);
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