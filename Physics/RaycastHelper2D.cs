using Godot;
using System;

public class RaycastHelperHit2D
{
    public bool hit_something;
    public Node collider;
    public int collider_id;
    public Vector2 normal;
    public Vector2 position;
    public int face_index;
    public int rid;
    public int shape;

}
public partial class RaycastHelper2D
{
    PhysicsDirectSpaceState2D space;
    public RaycastHelper2D(PhysicsDirectSpaceState2D space)
    {
        this.space = space;
    }

    public RaycastHelperHit2D Raycast(Vector2 start, Vector2 direction, float distance)
    {
        return Linecast(start, start + (direction.Normalized() * distance));
    }

    public RaycastHelperHit2D Linecast(Vector2 start, Vector2 end)
    {
        var hitDetails = space.IntersectRay(new PhysicsRayQueryParameters2D()
        {
            CollideWithBodies = true,
            CollideWithAreas = true,
            HitFromInside = false,
            From = start,
            To = end,
        });

        RaycastHelperHit2D hit = new RaycastHelperHit2D();

        if (hitDetails != null && hitDetails.Count > 0)
        {
            hit.hit_something = true;
            hit.collider = (Node)hitDetails["collider"];
            hit.collider_id = (int)hitDetails["collider_id"];
            hit.normal = (Vector2)hitDetails["normal"];
            hit.position = (Vector2)hitDetails["position"];
            hit.face_index = (int)hitDetails["face_index"];
            hit.rid = (int)hitDetails["rid"];
            hit.shape = (int)hitDetails["shape"];
        }
        else
        {
            hit.hit_something = false;
        }

        return hit;
    }

}
