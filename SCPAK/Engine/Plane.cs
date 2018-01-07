using System;
namespace Engine
{
    public struct Plane : IEquatable<Plane>
    {
        public Vector3 Normal;

        public float D;

        public Plane(Vector3 normal, float d)
        {
            this.Normal = normal;
            this.D = d;
        }
        public override bool Equals(object obj)
        {
            return obj is Plane && this.Equals((Plane)obj);
        }

        public bool Equals(Plane other)
        {
            return this.Normal == other.Normal && this.D == other.D;
        }

        public override int GetHashCode()
        {
            return this.Normal.GetHashCode() + this.D.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", new object[] {
                this.Normal.X,
                this.Normal.Y,
                this.Normal.Z,
                this.D
            });
        }
        public static bool operator ==(Plane p1, Plane p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Plane p1, Plane p2)
        {
            return !p1.Equals(p2);
        }
    }
}
