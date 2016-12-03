using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Shaders
{
    public class TexturedShader : Effect
    {
        private const string PARAM_TEXTURE0 = "Texture0";
        private const string PARAM_TEXTURE1 = "Texture1";
        private const string PARAM_TEXTURE2 = "Texture2";
        private const string PARAM_WORLD = "World";
        private const string PARAM_VIEW = "View";
        private const string PARAM_PROJECTION = "Projection";

        public TexturedShader(Effect textureEffect)
            : base(textureEffect)
        {
            CurrentTechnique = Techniques["Textured"];
        }

        public Texture2D Texture0
        {
            get { return Parameters[PARAM_TEXTURE0].GetValueTexture2D(); }
            set { Parameters[PARAM_TEXTURE0].SetValue(value); }
        }
        public Texture2D Texture1
        {
            get { return Parameters[PARAM_TEXTURE1].GetValueTexture2D(); }
            set { Parameters[PARAM_TEXTURE1].SetValue(value); }
        }
        public Texture2D Texture2
        {
            get { return Parameters[PARAM_TEXTURE2].GetValueTexture2D(); }
            set { Parameters[PARAM_TEXTURE2].SetValue(value); }
        }

        public Matrix World
        {
            get { return Parameters[PARAM_WORLD].GetValueMatrix(); }
            set { Parameters[PARAM_WORLD].SetValue(value); }
        }

        public Matrix View
        {
            get { return Parameters[PARAM_VIEW].GetValueMatrix(); }
            set { Parameters[PARAM_VIEW].SetValue(value); }
        }

        public Matrix Projection
        {
            get { return Parameters[PARAM_PROJECTION].GetValueMatrix(); }
            set { Parameters[PARAM_PROJECTION].SetValue(value); }
        }
    }
}
