using Godot;
/// <summary>
/// Used by the QuadTree to represent a rectangular area.
/// </summary>
[System.Serializable]
public struct Quad
{
    private Vector2 _min;
    private Vector2 _max;
    private Vector2 _position;
    private Vector2 _size;
    public Vector2 Min
    {
        get
        {
            return _min;
        }
        set
        {
            _min = value;
            _position = _min + ((_max - _min) / 2);
            _size = _max - _min;
        }
    }
    public Vector2 Max
    {
        get
        {
            return _max;
        }
        set
        {
            _max = value;
            _position = _min + ((_max - _min) / 2);
            _size = _max - _min;
        }
    }
    public Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            _min = _position - (_size / 2.0f);
            _max = _position + (_size / 2.0f);
        }
    }
    public Vector2 Size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = value;
            _min = _position - (_size / 2.0f);
            _max = _position + (_size / 2.0f);
        }
    }
    /// <summary>
    /// Construct a new Quad.
    /// </summary>
    /// <param name="min">Minimum</param>
    /// <param name="min">Minimum</param>
    public Quad(Vector2 min, Vector2 max)
    {
        _min = min;
        _max = max;
        _position = _min + ((_max - _min) / 2);
        _size = _max - _min;
    }

    /// <summary>
    /// Construct a new Quad.
    /// </summary>
    /// <param name="minX">Minimum x.</param>
    /// <param name="minY">Minimum y.</param>
    /// <param name="maxX">Max x.</param>
    /// <param name="maxY">Max y.</param>
    public Quad(float minX, float minY, float maxX, float maxY)
    {
        _min = new Vector2(minX, minY);
        _max = new Vector2(maxX, maxY);
        _position = _min + ((_max - _min) / 2);
        _size = _max - _min;
    }

    /// <summary>
    /// Set the Quad's position.
    /// </summary>
    /// <param name="minX">Minimum x.</param>
    /// <param name="minY">Minimum y.</param>
    /// <param name="maxX">Max x.</param>
    /// <param name="maxY">Max y.</param>
    public void Set(float minX, float minY, float maxX, float maxY)
    {
        Min = new Vector2(minX, minY);
        Max = new Vector2(maxX, maxY);
    }

    /// <summary>
    /// Check if this Quad intersects with another.
    /// </summary>
    public bool Intersects(ref Quad other)
    {
        return Min.X < other.Max.X && Min.Y < other.Max.Y && Max.X > other.Min.X && Max.Y > other.Min.Y;
    }

    /// <summary>
    /// Check if this Quad can completely contain another.
    /// </summary>
    public bool Contains(ref Quad other)
    {
        return other.Min.X >= Min.X && other.Min.Y >= Min.Y && other.Max.X <= Max.X && other.Max.Y <= Max.Y;
    }

    /// <summary>
    /// Check if this Quad contains the point.
    /// </summary>
    public bool Contains(float x, float y)
    {
        return x > Min.X && y > Min.Y && x < Max.X && y < Max.Y;
    }
    public override string ToString()
    {
        return $"Min:({Min}) Max:({Max}) Position:({Position}) GRID_SIZE({Size})";
    }
}


