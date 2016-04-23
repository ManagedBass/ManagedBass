namespace ManagedBass
{
    /// <summary>
    /// EAX environment constants to be used with <see cref="Bass.SetEAXParameters" />
    /// </summary>
    public enum EAXEnvironment
    {
        /// <summary>
        /// -1 = leave current EAX environment as is
        /// </summary>
        LeaveCurrent = -1,

        /// <summary>
        /// Generic
        /// </summary>
        Generic = 0,

        /// <summary>
        /// Padded Cell
        /// </summary>
        PaddedCell = 1,

        /// <summary>
        /// Room
        /// </summary>
        Room = 2,

        /// <summary>
        /// Bathroom
        /// </summary>
        Bathroom = 3,

        /// <summary>
        /// Livingroom
        /// </summary>
        Livingroom = 4,

        /// <summary>
        /// Stoneroom
        /// </summary>
        Stoneroom = 5,

        /// <summary>
        /// Auditorium
        /// </summary>
        Auditorium = 6,

        /// <summary>
        /// Concert Hall
        /// </summary>
        ConcertHall = 7,

        /// <summary>
        /// Cave
        /// </summary>
        Cave = 8,

        /// <summary>
        /// Arena
        /// </summary>
        Arena = 9,

        /// <summary>
        /// Hangar
        /// </summary>
        Hangar = 10,

        /// <summary>
        /// Carpeted Hallway
        /// </summary>
        CarpetedHallway = 11,

        /// <summary>
        /// Hallway
        /// </summary>
        Hallway = 12,

        /// <summary>
        /// Stone Corridor
        /// </summary>
        StoneCorridor = 13,

        /// <summary>
        /// Alley
        /// </summary>
        Alley = 14,

        /// <summary>
        /// Forest
        /// </summary>
        Forest = 15,

        /// <summary>
        /// City
        /// </summary>
        City = 16,

        /// <summary>
        /// Mountains
        /// </summary>
        Mountains = 17,

        /// <summary>
        /// Quarry
        /// </summary>
        Quarry = 18,

        /// <summary>
        /// Plain
        /// </summary>
        Plain = 19,

        /// <summary>
        /// Parkinglot
        /// </summary>
        ParkingLot = 20,

        /// <summary>
        /// Sewer Pipe
        /// </summary>
        SewerPipe = 21,

        /// <summary>
        /// Underwater
        /// </summary>
        Underwater = 22,

        /// <summary>
        /// Drugged
        /// </summary>
        Drugged = 23,

        /// <summary>
        /// Dizzy
        /// </summary>
        Dizzy = 24,

        /// <summary>
        /// Psychotic
        /// </summary>
        Psychotic = 25,

        /// <summary>
        /// Total number of environments
        /// </summary>
        Count = 26
    }
}