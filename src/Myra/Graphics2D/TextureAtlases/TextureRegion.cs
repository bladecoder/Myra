using System;

#if !XENKO
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Texture2D = Xenko.Graphics.Texture;
#endif

namespace Myra.Graphics2D.TextureAtlases
{
	public class TextureRegion: IImage
	{
		private readonly Texture2D _texture;
		private readonly Rectangle _bounds;
        private readonly Point _offset;
        private readonly Point _size;

        public Texture2D Texture
		{
			get { return _texture; }
		}

		public Rectangle Bounds
		{
			get { return _bounds; }
		}

        public Point Offset
        {
            get { return _offset; }
        }

        public Point Size
		{
			get { return _size; }
		}

		/// <summary>
		/// Covers the whole texture
		/// </summary>
		/// <param name="texture"></param>
		public TextureRegion(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height))
		{
		}

		public TextureRegion(Texture2D texture, Rectangle bounds)
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}

			_texture = texture;
			_bounds = bounds;
            _size = new Point(bounds.Width, bounds.Height);
            _offset = Point.Zero;
		}

        public TextureRegion(Texture2D texture, Rectangle bounds, Point offset, Point size)
        {
            _texture = texture ?? throw new ArgumentNullException("texture");
            _bounds = bounds;
            _size = size;
            _offset = offset;

            // correct for Y axis up
            //_offset.Y = size.Y - offset.Y - bounds.Height;
        }

        public TextureRegion(TextureRegion region, Rectangle bounds)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}

			_texture = region.Texture;
			bounds.Offset(region.Bounds.Location);
			_bounds = bounds;
            _size = new Point(bounds.Width, bounds.Height);
            _offset = Point.Zero;
        }

		public virtual void Draw(SpriteBatch batch, Rectangle dest, Color color)
		{
            float scaleX = dest.Width / (float)Size.X;
            float scaleY = dest.Height / (float)Size.Y;
            var dest2 = new Rectangle((int)(dest.X + Offset.X * scaleX), (int)(dest.Y + Offset.Y * scaleY), (int)(Bounds.Width * scaleX), (int)(Bounds.Height * scaleY)); 
			batch.Draw(Texture,	dest2, Bounds, color);
		}
	}
}