namespace Dunkel.Game.ComponentSystems
{
    public interface IUpdateComponentSystem
    {
        int Priority { get; }
        void Update();
    }
}