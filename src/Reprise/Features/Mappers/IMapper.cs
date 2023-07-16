namespace Reprise
{
    /// <summary>
    /// Specifies the contract to map objects.
    /// </summary>
    public interface IMapper<T1, T2>
    {
        /// <summary>
        /// Maps a source object to a new destination object.
        /// </summary>
        T1 Map(T2 source);

        /// <summary>
        /// Maps a source object to a new destination object.
        /// </summary>
        T2 Map(T1 source);

        /// <summary>
        /// Maps a source object to an existing destination object.
        /// </summary>
        void Map(T2 source, T1 destination);

        /// <summary>
        /// Maps a source object to an existing destination object.
        /// </summary>
        void Map(T1 source, T2 destination);
    }
}
