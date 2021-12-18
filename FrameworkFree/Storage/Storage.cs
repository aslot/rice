namespace Data
{
    public sealed class Storage : IStorage
    {
        public IMemory Fast { get; }

        public IDatabase Slow { get; }

        public Storage(IMemory memory, IDatabase database)
        {
            Fast = memory;
            Slow = database;
        }
    }
}