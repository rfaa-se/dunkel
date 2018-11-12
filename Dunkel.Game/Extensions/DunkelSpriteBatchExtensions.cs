using System;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class DunkelSpriteBatchExtensions
    {
        private static Texture2D _pixel;

        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, float x, float y,
            float width, float height, Color color, float rotation = 0f)
        {
            var matrix = 
                  Matrix.CreateTranslation(-new Vector3(x + width / 2f, y + height / 2f, 0f))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateTranslation(new Vector3(x + width / 2f, y + height / 2f, 0f));

            spriteBatch.Draw(
                texture: GetPixel(spriteBatch),
                position: Vector2.Transform(new Vector2(x, y), matrix),
                sourceRectangle: null,
                color: color,
                rotation: rotation,
                origin: Vector2.Zero,
                scale: new Vector2(width, height),
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, float x, float y,
            float width, float height, Color color, float rotation = 0f, int border = 1)
        {
            var halfBorder = border / 2f;
            var matrix = 
                  Matrix.CreateTranslation(-new Vector3(x + width / 2f, y + height / 2f, 0f))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateTranslation(new Vector3(x + width / 2f, y + height / 2f, 0f));

            // top
            DrawLine(
                spriteBatch: spriteBatch,
                point1: Vector2.Transform(new Vector2(x, y + halfBorder), matrix),
                point2: Vector2.Transform(new Vector2(x + width, y + halfBorder), matrix),
                color: color,
                border: border);

            // bottom
            DrawLine(
                spriteBatch: spriteBatch,
                point1: Vector2.Transform(new Vector2(x, y + height - halfBorder), matrix),
                point2: Vector2.Transform(new Vector2(x + width, y + height - halfBorder), matrix),
                color: color,
                border: border);

            // left
            DrawLine(
                spriteBatch: spriteBatch,
                point1: Vector2.Transform(new Vector2(x + halfBorder, y + border), matrix),
                point2: Vector2.Transform(new Vector2(x + halfBorder, y + height - border), matrix),
                color: color,
                border: border);

            // right
            DrawLine(
                spriteBatch: spriteBatch,
                point1: Vector2.Transform(new Vector2(x + width - halfBorder, y + border), matrix),
                point2: Vector2.Transform(new Vector2(x + width - halfBorder, y + height - border), matrix),
                color: color,
                border: border);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, 
            int border = 1)
        {
            var distance = Vector2.Distance(point1, point2);
            var rotation = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(distance, border);

            spriteBatch.Draw(
                texture: GetPixel(spriteBatch),
                position: point1,
                sourceRectangle: null,
                color: color,
                rotation: rotation,
                origin: origin,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0f);
        }

        private static Texture2D GetPixel(SpriteBatch spriteBatch)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _pixel.SetData(new[] { Color.White });
            }

            return _pixel;
        }
    }
}