using CoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Info;

public struct ColliderInfo
{
    public CustomVector2 Offset {  get; set; }
    public float Width {  get; set; }
    public float Height { get; set; }

    public readonly CustomVector2[] RectVertices => new CustomVector2[]
    {
        new CustomVector2(Offset.x - Width / 2, Offset.y - Height / 2 + Radius),
        new CustomVector2(Offset.x - Width / 2, Offset.y + Height / 2 - Radius),
        new CustomVector2(Offset.x + Width / 2, Offset.y + Height / 2 - Radius),
        new CustomVector2(Offset.x + Width / 2, Offset.y - Height / 2 + Radius)
    };

    public readonly float Radius => Width / 2;
    public readonly CustomVector2 UpperCenter => new CustomVector2(Offset.x, Offset.y + Height / 2 - Radius);
    public readonly CustomVector2 LowerCenter => new CustomVector2(Offset.x, Offset.y - Height / 2 + Radius);

    public ColliderInfo SetOffset(CustomVector2 offset)
        => new ColliderInfo() { Offset = offset, Width = this.Width, Height = this.Height };

    public ColliderInfo SetWidth(float width)
        => new ColliderInfo() { Offset = this.Offset, Width = width, Height = this.Height };

    public ColliderInfo SetHeight(float height)
        => new ColliderInfo() { Offset = this.Offset, Width = this.Width, Height = height };
}
