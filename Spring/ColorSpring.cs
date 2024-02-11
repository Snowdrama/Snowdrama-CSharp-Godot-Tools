using Godot;
using System;
using System.Collections.Generic;

//Min 2D representation of a spring
namespace Snowdrama.Spring
{
    public class SpringColor
    {
        SpringList springCollection;
        int rID;
        int gID;
        int bID;
        int aID;

        public Color Value
        {
            get
            {
                return new Color(springCollection.GetValue(rID), springCollection.GetValue(gID), springCollection.GetValue(bID), springCollection.GetValue(aID));
            }
            set
            {
                springCollection.SetValue(rID, value.R);
                springCollection.SetValue(gID, value.G);
                springCollection.SetValue(bID, value.B);
                springCollection.SetValue(aID, value.A);
            }
        }
        public Color Target
        {
            get
            {
                return new Color(springCollection.GetTarget(rID), springCollection.GetTarget(gID), springCollection.GetTarget(bID), springCollection.GetTarget(aID));
            }
            set
            {
                springCollection.SetTarget(rID, value.R);
                springCollection.SetTarget(gID, value.G);
                springCollection.SetTarget(bID, value.B);
                springCollection.SetTarget(aID, value.A);
            }
        }
        public Color Velocity
        {
            get
            {
                return new Color(springCollection.GetVelocity(rID), springCollection.GetVelocity(gID), springCollection.GetVelocity(bID), springCollection.GetVelocity(aID));
            }
            set
            {
                springCollection.SetVelocity(rID, value.R);
                springCollection.SetVelocity(gID, value.G);
                springCollection.SetVelocity(bID, value.B);
                springCollection.SetVelocity(aID, value.A);
            }
        }

        public SpringConfiguration SpringConfig
        {
            set
            {
                springCollection.SetSpringConfig(rID, value);
                springCollection.SetSpringConfig(gID, value);
                springCollection.SetSpringConfig(bID, value);
                springCollection.SetSpringConfig(aID, value);
            }
        }

        public SpringColor(SpringConfiguration config, Color initialValue = default)
        {
            springCollection = new SpringList(4);
            rID = springCollection.Add(initialValue.R, config);
            gID = springCollection.Add(initialValue.G, config);
            bID = springCollection.Add(initialValue.B, config);
            aID = springCollection.Add(initialValue.A, config);
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