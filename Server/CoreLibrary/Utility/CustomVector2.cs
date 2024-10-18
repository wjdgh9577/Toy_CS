using Server.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Utility
{
    public struct CustomVector2 : IEquatable<CustomVector2>
    {
        public float x;
        public float y;

        public CustomVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public CustomVector2(CustomVector2Int cv2i)
        {
            this.x = cv2i.x;
            this.y = cv2i.y;
        }

        public bool Equals(CustomVector2 other)
            => this.x == other.x && this.y == other.y;

        public static explicit operator CustomVector2Int(CustomVector2 a)
            => new CustomVector2Int(a);

        public static bool operator ==(CustomVector2 a, CustomVector2 b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2 a, CustomVector2 b)
            => !a.Equals(b);

        public static CustomVector2 operator +(CustomVector2 a, CustomVector2 b)
            => new CustomVector2(a.x + b.x, a.y + b.y);

        public static CustomVector2 operator -(CustomVector2 a, CustomVector2 b)
            => new CustomVector2(a.x - b.x, a.y - b.y);

        public float Square()
            => Square(this);

        public static float Square(CustomVector2 a)
            => a.x * a.x + a.y * a.y;

        public float Distance(CustomVector2 a)
            => Distance(this, a);

        public static float Distance(CustomVector2 a, CustomVector2 b)
            => (a - b).Magnitude();

        public float Magnitude()
            => Magnitude(this);

        public static float Magnitude(CustomVector2 a)
            => (float)Math.Sqrt(a.x * a.x + a.y * a.y);

        public float Dot(CustomVector2 a)
            => Dot(this, a);

        public static float Dot(CustomVector2 a, CustomVector2 b)
            => a.x * b.x + a.y * b.y;

        public float Cross(CustomVector2 a)
            => Cross(this, a);

        public static float Cross(CustomVector2 a, CustomVector2 b)
            => a.x * b.y - a.y * b.x;
    }

    public struct CustomVector2Int : IEquatable<CustomVector2Int>
    {
        public int x;
        public int y;

        public CustomVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CustomVector2Int(float x, float y)
        {
            this.x = x.ToInt32Safe();
            this.y = y.ToInt32Safe();
        }

        public CustomVector2Int(CustomVector2 cv2)
        {
            this.x = cv2.x.ToInt32Safe();
            this.y = cv2.y.ToInt32Safe();
        }

        public bool Equals(CustomVector2Int other)
            => this.x == other.x && this.y == other.y;

        public static implicit operator CustomVector2(CustomVector2Int cv2i)
            => new CustomVector2(cv2i);

        public static bool operator ==(CustomVector2Int a, CustomVector2Int b)
            => a.Equals(b);

        public static bool operator !=(CustomVector2Int a, CustomVector2Int b)
            => !a.Equals(b);

        public static CustomVector2Int operator +(CustomVector2Int a, CustomVector2Int b)
            => new CustomVector2Int(a.x + b.x, a.y + b.y);

        public static CustomVector2Int operator -(CustomVector2Int a, CustomVector2Int b)
            => new CustomVector2Int(a.x - b.x, a.y - b.y);
    }
}
