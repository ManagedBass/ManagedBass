using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Error in a function return value
    /// </summary>
    /// <typeparam name="T">The Type of the function return value</typeparam>
    public class Return<T>
    {
        public Errors ErrorCode { get; private set; }

        Return() { }

        public T Value { get; private set; }

        public static implicit operator T(Return<T> e) { return e.Value; }

        public static implicit operator Return<T>(T e)
        {
            return new Return<T>()
            {
                ErrorCode = Bass.LastError,
                Value = e
            };
        }

        public bool Success { get { return ErrorCode == Errors.OK; } }
    }
}