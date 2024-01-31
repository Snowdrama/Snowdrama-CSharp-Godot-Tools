using Godot;
using System;

public partial class Delaunay : Node
{
    
    public Delaunay()
    {

    }
}


public class DelaunyTriangle
{
    public int[] triangleInexes;

}

public class DelaunyEdge
{
    public int[] triangleInexes;

}

public class DelaunyCircle
{
    public Vector2 center;
    public double radius;


    public DelaunyCircle(Vector2 center, double radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public static bool IsPointInCircle(Vector2 point, DelaunyCircle circle)
    {
        Vector2 vec = circle.center - point;
        
        //we use the squared length vs the squared radius to avoid a square root call
        return (vec.LengthSquared() <  (circle.radius * circle.radius))? false : true;
    }


    //find the circle that lies on all 3 points
    public static DelaunyCircle CreateCircleFromPoint(Vector2 point1, Vector2 point2, Vector2 point3)
    {

        var midPoint = new Vector2((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        var midPoint2 = new Vector2((point1.X + point3.X) / 2, (point1.Y + point3.Y) / 2);

        var k1 = -(point2.X - point1.X) / (point2.Y - point1.Y);
        var k2 = -(point3.X - point1.X) / (point3.Y - point1.Y);

        var centerX = (midPoint2.Y - midPoint.Y - k2 * midPoint2.X + k1 * midPoint.X) / (k1 - k2);
        var centerY = midPoint.Y + k1 * (midPoint2.Y - midPoint.Y - k2 * midPoint2.X + k2 * midPoint.X) / (k1 - k2);

        var center = new Vector2(centerX, centerY);
        var radius = Math.Sqrt((centerX - point1.X) * (centerX - point1.X) + (centerY - point1.Y) * (centerY - point1.Y));

        return new DelaunyCircle(center, radius);
    }
}


public class DelaunayPoint
{
    public Vector2 point;
}