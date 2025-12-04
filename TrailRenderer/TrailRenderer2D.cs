using Godot;


[Tool, GlobalClass]
public partial class TrailRenderer2D : Line2D
{
    //the thing for the trail to follow
    [Export] Node2D target;

    //the number of points in the Line2D
    [Export] int length = 150;

    //the create a new point if the target has moved this far
    [Export] float distanceThreshold = 15;

    //for measuring the distance traveled
    Vector2 lastPos;

    //switches betweeen distance or time based
    [Export] bool isTimed;

    Vector2[] pointArr = new Vector2[10];
    [Export] float shaderTexOffset;

    TrailRenderer2D()
    {
        if (target == null)
        {
            target = (Node2D)this.GetParent();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.TopLevel = true;
        SetupLine();
    }
    private void SetupLine()
    {
        pointArr = new Vector2[length];
        for (int i = 0; i < pointArr.Length; i++)
        {
            pointArr[i] = target.GlobalPosition;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        //used in tool mode when switching length in the editor
        if (Engine.IsEditorHint())
        {
            if (pointArr.Length != length)
            {
                SetupLine();
            }
        }

        //if we're not timed we only add move/remove points when the target
        //has traveled some distance
        if (!isTimed && target.GlobalPosition.DistanceTo(lastPos) > distanceThreshold)
        {
            lastPos = target.GlobalPosition;
            if (this.Texture != null && this.Material != null)
            {
                if (pointArr.Length <= 2) { return; }
                var textureRepeatUnitLength = Width * Texture.GetWidth() / Texture.GetHeight();
                shaderTexOffset += pointArr[0].DistanceTo(pointArr[1]) / textureRepeatUnitLength;
                shaderTexOffset = shaderTexOffset % 1.0f;
                //Debug.Log($"shaderTexOffset: {shaderTexOffset}");
                this.Material.Set("shader_parameter/tex_offset", shaderTexOffset);
            }

            //for (int i = pointArr.Length - 1; i > 0; i--)
            //{
            //    pointArr[i] = pointArr[i - 1];
            //}

            //pointArr[0] = target.GlobalPosition;

            for (int i = 0; i < pointArr.Length - 1; i++)
            {
                pointArr[i] = pointArr[i + 1];
            }

            pointArr[pointArr.Length - 1] = target.GlobalPosition;

            this.Points = pointArr;
        }
        //if we're timed then every physics tick we update
        //we're in physics process so line length is consistent
        //even if framerate isn't
        else if (isTimed)
        {
            lastPos = target.GlobalPosition;

            for (int i = 0; i < pointArr.Length - 1; i++)
            {
                pointArr[i] = pointArr[i + 1];
            }

            pointArr[pointArr.Length - 1] = target.GlobalPosition;

            this.Points = pointArr;
        }
    }
}
