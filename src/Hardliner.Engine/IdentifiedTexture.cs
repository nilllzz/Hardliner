using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine
{
    public class IdentifiedTexture
    {
        private struct TextureIntegrity
        {
            public int Width;
            public int Height;
            public int ColorDataHash;

            public int GenerateHash()
            {
                int hash = 17;
                unchecked
                {
                    hash = (hash * 23) ^ Width;
                    hash = (hash * 23) ^ Height;
                    hash = (hash * 23) ^ ColorDataHash;
                }
                return hash;
            }
        }

        private TextureIntegrity _integrity;

        public Texture2D Resource { get; }

        public IdentifiedTexture(GraphicsDevice device, int width, int height, Color[] data)
        {
            Resource = new Texture2D(device, width, height);
            Resource.SetData(data);

            GenerateIntegrity();
        }

        public IdentifiedTexture(Texture2D texture)
        {
            Resource = texture;

            GenerateIntegrity();
        }

        private void GenerateIntegrity()
        {
            var data = new Color[Resource.Width * Resource.Height];
            Resource.GetData(data);

            var hash = 17;

            unchecked
            {
                foreach (var color in data)
                    hash = (hash * 16777619) ^ color.GetHashCode();
            }

            _integrity = new TextureIntegrity
            {
                Width = Resource.Width,
                Height = Resource.Height,
                ColorDataHash = hash
            };
        }

        private static bool Equals(IdentifiedTexture t1, IdentifiedTexture t2)
        {
            if (ReferenceEquals(t1, null) && ReferenceEquals(t2, null))
                return true;
            if (ReferenceEquals(t1, null) || ReferenceEquals(t2, null))
                return false;

            return t1._integrity.Width == t2._integrity.Width &&
                t1._integrity.Height == t2._integrity.Height &&
                t1._integrity.ColorDataHash == t2._integrity.ColorDataHash &&
                t1.GetHashCode() == t2.GetHashCode();
        }

        public override int GetHashCode() => _integrity.GenerateHash();

        public override bool Equals(object obj)
            => Equals(this, obj as IdentifiedTexture);

        public static bool operator ==(IdentifiedTexture tLeft, IdentifiedTexture tRight)
            => Equals(tLeft, tRight);

        public static bool operator !=(IdentifiedTexture tLeft, IdentifiedTexture tRight)
            => !Equals(tLeft, tRight);
    }
}
