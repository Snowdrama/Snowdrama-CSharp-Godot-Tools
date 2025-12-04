// https://stackoverflow.com/questions/67412060/godot-engine-merging-more-than-two-polygons-using-geometrys-merge-polygons-2d
// https://github.com/a327ex/a327ex-template/blob/dd0a63aefbc3cae268d165deef8fdf931f776f55/engine/map/tilemap.lua
// https://github.com/a327ex/blog/issues/5
// https://godotengine.org/qa/30006/build-a-collision-polygon-from-tilemap-outline
// https://gist.github.com/afk-mario/15b5855ccce145516d1b458acfe29a28
using Godot;

[Tool]
public partial class CombineCollisionShapes : Node
{
    private Node2D PostImport(Node2D level)
    {
        Polygon2D mergedPolygon = new Polygon2D();
        foreach (var child in level.GetChildren())
        {

            if (child is CollisionPolygon2D polyCollider)
            {
                //it's already a polygon so merge
                mergedPolygon.Polygon = Geometry2D.MergePolygons(mergedPolygon.Polygon, polyCollider.Polygon)[0];


                //merge the polygon with the existing one
            }


            if (child is CollisionShape2D shape2D)
            {
                //convert the shape into a polygon

                //merge the polygon with the existing one
            }
        }
        return mergedPolygon;
    }

    private Polygon2D? ExtractPolygonFromShape(CollisionShape2D shape2D)
    {
        Polygon2D polygon = new Polygon2D();

        if (shape2D.Shape is RectangleShape2D r2d)
        {


            return polygon;
        }

        return null;
    }
}
