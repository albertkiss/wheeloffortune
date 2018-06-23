namespace wheeloffortune.Library
{
    public interface IStorage<T>
    {
        void Save(T stuff);
        T Load();
    }
}