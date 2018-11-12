using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.ComponentSystems
{
    public interface IDrawComponentSystem
    {
        int Priority { get; }
        void Draw(SpriteBatch sb, float delta);
    }
}