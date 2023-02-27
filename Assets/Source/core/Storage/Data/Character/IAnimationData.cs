namespace game.core.Storage.Data.Character
{
    public interface IAnimationData 
    {
		
    }

    public interface IAnimationData<TEnum, TClipType> : IAnimationData
    {
        public TEnum type { get; }
        public TClipType clip { get; }

        public float length { get; }
    }
}