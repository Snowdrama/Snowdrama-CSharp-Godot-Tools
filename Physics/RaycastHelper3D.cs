using Godot;
using System;
using static Godot.TextServer;

public class RaycastHelperHit3D
{
    public bool hit_something;
    public Node collider;
    public int collider_id;
    public Vector3 normal;
    public Vector3 position;
    public int face_index;
    public int rid;
    public int shape;

}
public partial class RaycastHelper3D
{
	PhysicsDirectSpaceState3D space;
	public RaycastHelper3D(PhysicsDirectSpaceState3D space) 
	{
        this.space = space;
    }


	public RaycastHelperHit3D Raycast(Vector3 start, Vector3 direction, float distance)
    {
        return Linecast(start, start + (direction.Normalized() * distance));
    }

	public RaycastHelperHit3D Linecast(Vector3 start, Vector3 end)
    {
        var hitDetails = space.IntersectRay(new PhysicsRayQueryParameters3D()
        {
            CollideWithBodies = true,
            CollideWithAreas = true,
            HitFromInside = false,
            HitBackFaces = false,
            From = start,
            To = end,
        });

        RaycastHelperHit3D hit = new RaycastHelperHit3D();

        if (hitDetails != null && hitDetails.Count > 0)
        {
            hit.hit_something = true;
            hit.collider = (Node)hitDetails["collider"];
            hit.collider_id = (int)hitDetails["collider_id"];
            hit.normal = (Vector3)hitDetails["normal"];
            hit.position = (Vector3)hitDetails["position"];
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
