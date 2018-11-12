namespace Dunkel.Game.Components.Attributes
{
    public class ClassificationComponent : IComponent
    {
        public ClassificationType Type { get; set; }

        public void Reset() 
        {
            Type = ClassificationType.Unknown;
        }
    }

    public enum ClassificationType
    {
        Unknown = 0,
        Ship
    }
}