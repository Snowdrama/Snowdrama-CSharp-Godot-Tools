using Godot;
using System;
using System.Collections.Generic;

//Min 2D representation of a spring
namespace Snowdrama.Spring
{
    public class Spring3D
    {
        private SpringList springCollection;
        private int xID;
        private int yID;
        private int zID;

        public Vector3 Value
        {
            get
            {
                return new Vector3(springCollection.GetValue(xID), springCollection.GetValue(yID), springCollection.GetValue(zID));
            }
            set
            {
                springCollection.SetValue(xID, value.X);
                springCollection.SetValue(yID, value.Y);
                springCollection.SetValue(zID, value.Z);
            }
        }
        public Vector3 Target
        {
            get
            {
                return new Vector3(springCollection.GetTarget(xID), springCollection.GetTarget(yID), springCollection.GetTarget(zID));
            }
            set
            {
                springCollection.SetTarget(xID, value.X);
                springCollection.SetTarget(yID, value.Y);
                springCollection.SetTarget(zID, value.Z);
            }
        }
        public Vector3 Velocity
        {
            get
            {
                return new Vector3(springCollection.GetVelocity(xID), springCollection.GetVelocity(yID), springCollection.GetVelocity(zID));
            }
            set
            {
                springCollection.SetVelocity(xID, value.X);
                springCollection.SetVelocity(yID, value.Y);
                springCollection.SetVelocity(zID, value.Z);
            }
        }

        public SpringConfiguration SpringConfig
        {
            set
            {
                springCollection.SetSpringConfig(xID, value);
                springCollection.SetSpringConfig(yID, value);
                springCollection.SetSpringConfig(zID, value);
            }
        }

        public Spring3D(SpringConfiguration config, Vector3 initialValue = default)
        {
            springCollection = new SpringList(3);
            xID = springCollection.Add(initialValue.X, config, config.ClampRangeX);
            yID = springCollection.Add(initialValue.Y, config, config.ClampRangeY);
            zID = springCollection.Add(initialValue.Z, config, config.ClampRangeZ);
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