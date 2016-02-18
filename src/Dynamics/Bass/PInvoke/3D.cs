using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
        /// Applies changes made to the 3D system.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This must be called to apply any changes made with Set3D methods. 
        /// It improves performance to have DirectSound do all the required recalculating at the same time like this, rather than recalculating after every little change is made.
        /// </para>
        /// <para>This function applies 3D changes on all the initialized devices - there's no need to re-call it for each individual device when using multiple devices.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Apply3D")]
        public static extern void Apply3D();

        /// <summary>
        /// Retrieves the factors that affect the calculations of 3D sound.
        /// <para>This overload allows you to only get all three values at a time.</para>
        /// </summary>
        /// <param name="Distance">The distance factor.</param>
        /// <param name="RollOff">The rolloff factor.</param>
        /// <param name="Doppler">The doppler factor.</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.No3D">The device was not initialized with 3D support.</exception>
        /// <remarks>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</remarks>
        [DllImport(DllName, EntryPoint = "BASS_Get3DFactors")]
        public static extern bool Get3DFactors(ref float Distance, ref float RollOff, ref float Doppler);

        /// <summary>
        /// Sets the factors that affect the calculations of 3D sound.
        /// </summary>
        /// <param name="Distance">
        /// The distance factor... less than 0.0 = leave current... examples: 1.0 = use meters, 0.9144 = use yards, 0.3048 = use feet.
        /// By default BASS measures distances in meters, you can change this setting if you are using a different unit of measurement.
        /// </param>
        /// <param name="RollOff">The rolloff factor, how fast the sound quietens with distance... 0.0 (min) - 10.0 (max), less than 0.0 = leave current... examples: 0.0 = no rolloff, 1.0 = real world, 2.0 = 2x real.</param>
        /// <param name="Doppler">
        /// The doppler factor... 0.0 (min) - 10.0 (max), less than 0.0 = leave current... examples: 0.0 = no doppler, 1.0 = real world, 2.0 = 2x real.
        /// The doppler effect is the way a sound appears to change pitch when it is moving towards or away from you (say hello to Einstein!).
        /// The listener and sound velocity settings are used to calculate this effect, this doppf value can be used to lessen or exaggerate the effect.
        /// </param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.No3D">The device was not initialized with 3D support.</exception>
        /// <remarks>
        /// <para>As with all 3D functions, use <see cref="Apply3D" /> to apply the changes.</para>
        /// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Set3DFactors")]
        public static extern bool Set3DFactors(float Distance, float RollOff, float Doppler);

        /// <summary>
        /// Retrieves the current type of EAX environment and it's parameters.
        /// </summary>
        /// <param name="Environment">The EAX environment to get (one of the <see cref="EAXEnvironment" /> values).</param>
        /// <param name="Volume">The volume of the reverb.</param>
        /// <param name="Decay">The decay duration.</param>
        /// <param name="Damp">The damping.</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NoEAX">The current device does not support EAX.</exception>
        /// <remarks>
        /// When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.
        /// <para><b>Platform-specific</b></para>
        /// <para>EAX and this function are only available on Windows</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_GetEAXParameters")]
        public static extern bool GetEAXParameters(ref EAXEnvironment Environment, ref float Volume, ref float Decay, ref float Damp);

        public static bool SetEAXPreset(EAXEnvironment Environment)
        {
            switch (Environment)
            {
                case EAXEnvironment.Generic:
                    return SetEAXParameters(Environment, 0.5f, 1.493f, 0.5f);

                case EAXEnvironment.PaddedCell:
                    return SetEAXParameters(Environment, 0.25f, 0.1f, 0);

                case EAXEnvironment.Room:
                    return SetEAXParameters(Environment, 0.417f, 0.4f, 0.666f);

                case EAXEnvironment.Bathroom:
                    return SetEAXParameters(Environment, 0.653f, 1.499f, 0.166f);

                case EAXEnvironment.Livingroom:
                    return SetEAXParameters(Environment, 0.208f, 0.478f, 0);

                case EAXEnvironment.Stoneroom:
                    return SetEAXParameters(Environment, 0.5f, 2.309f, 0.888f);

                case EAXEnvironment.Auditorium:
                    return SetEAXParameters(Environment, 0.403f, 4.279f, 0.5f);

                case EAXEnvironment.ConcertHall:
                    return SetEAXParameters(Environment, 0.5f, 3.961f, 0.5f);

                case EAXEnvironment.Cave:
                    return SetEAXParameters(Environment, 0.5f, 2.886f, 1.304f);

                case EAXEnvironment.Arena:
                    return SetEAXParameters(Environment, 0.361f, 7.284f, 0.332f);

                case EAXEnvironment.Hangar:
                    return SetEAXParameters(Environment, 0.5f, 10, 0.3f);

                case EAXEnvironment.CarpetedHallway:
                    return SetEAXParameters(Environment, 0.153f, 0.259f, 2);

                case EAXEnvironment.Hallway:
                    return SetEAXParameters(Environment, 0.361f, 1.493f, 0);

                case EAXEnvironment.StoneCorridor:
                    return SetEAXParameters(Environment, 0.444f, 2.697f, 0.638f);

                case EAXEnvironment.Alley:
                    return SetEAXParameters(Environment, 0.25f, 1.752f, 0.776f);

                case EAXEnvironment.Forest:
                    return SetEAXParameters(Environment, 0.111f, 3.145f, 0.472f);

                case EAXEnvironment.City:
                    return SetEAXParameters(Environment, 0.111f, 2.767f, 0.224f);

                case EAXEnvironment.Mountains:
                    return SetEAXParameters(Environment, 0.194f, 7.841f, 0.472f);

                case EAXEnvironment.Quarry:
                    return SetEAXParameters(Environment, 1, 1.499f, 0.5f);

                case EAXEnvironment.Plain:
                    return SetEAXParameters(Environment, 0.097f, 2.767f, 0.224f);

                case EAXEnvironment.ParkingLot:
                    return SetEAXParameters(Environment, 0.208f, 1.652f, 1.5f);

                case EAXEnvironment.SewerPipe:
                    return SetEAXParameters(Environment, 0.652f, 2.886f, 0.25f);

                case EAXEnvironment.Underwater:
                    return SetEAXParameters(Environment, 1, 1.499f, 0);

                case EAXEnvironment.Drugged:
                    return SetEAXParameters(Environment, 0.875f, 8.392f, 1.388f);

                case EAXEnvironment.Dizzy:
                    return SetEAXParameters(Environment, 0.139f, 17.234f, 0.666f);

                case EAXEnvironment.Psychotic:
                    return SetEAXParameters(Environment, 0.486f, 7.563f, 0.806f);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Sets the type of EAX environment and it's parameters.
        /// </summary>
        /// <param name="Environment">The EAX environment... -1 = leave current, or one of the these <see cref="EAXEnvironment" />.</param>
        /// <param name="Volume">The volume of the reverb... 0.0 (off) - 1.0 (max), less than 0.0 = leave current.</param>
        /// <param name="Decay">The time in seconds it takes the reverb to diminish by 60dB... 0.1 (min) - 20.0 (max), less than 0.0 = leave current.</param>
        /// <param name="Damp">The damping, high or low frequencies decay faster... 0.0 = high decays quickest, 1.0 = low/high decay equally, 2.0 = low decays quickest, less than 0.0 = leave current.</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NoEAX">The current device does not support EAX.</exception>
        /// <remarks>
        /// <para>
        /// Obviously, EAX functions have no effect if no EAX supporting device is used.
        /// You can use <see cref="GetInfo" /> to check if the current device suports EAX.
        /// EAX only affects 3D channels, but EAX functions do NOT require <see cref="Apply3D" /> to apply the changes.
        /// </para>
        /// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
        /// <para><b>Platform-specific</b></para>
        /// <para>This function is only available on Windows.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_SetEAXParameters")]
        public static extern bool SetEAXParameters(EAXEnvironment Environment, float Volume, float Decay, float Damp);

        /// <summary>
        /// Retrieves the position, velocity, and orientation of the listener.
        /// </summary>
        /// <param name="Position">The position of the listener</param>
        /// <param name="Velocity">The listener's velocity</param>
        /// <param name="Front">The direction that the listener's front is pointing</param>
        /// <param name="Top">The direction that the listener's top is pointing</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.No3D">The device was not initialized with 3D support.</exception>
        /// <remarks>
        /// <para>The <paramref name="Front" /> and <paramref name="Top" /> parameters must both be retrieved in a single call, they can not be retrieved individually.</para>
        /// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Get3DPosition")]
        public static extern bool Get3DPosition(ref Vector3D Position, ref Vector3D Velocity, ref Vector3D Front, ref Vector3D Top);

        /// <summary>
        /// Sets the position, velocity, and orientation of the listener (i.e. the player).
        /// </summary>
        /// <param name="Position">The position of the listener</param>
        /// <param name="Velocity">The listener's velocity</param>
        /// <param name="Front">The direction that the listener's front is pointing</param>
        /// <param name="Top">The direction that the listener's top is pointing</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.No3D">The device was not initialized with 3D support.</exception>
        /// <remarks>
        /// <para>The <paramref name="Front" /> and <paramref name="Top" /> parameters must both be set in a single call, they can not be set individually.</para>
        /// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Set3DPosition")]
        public static extern bool Set3DPosition(Vector3D Position, Vector3D Velocity, Vector3D Front, Vector3D Top);

        /// <summary>
        /// The 3D algorithm for software mixed 3D channels.
        /// </summary>
        /// <remarks>
        /// These algorithms only affect 3D channels that are being mixed in software.
        /// <see cref="ChannelGetInfo(int,out ChannelInfo)"/> can be used to check whether a channel is being software mixed.
        /// Changing the algorithm only affects subsequently created or loaded samples, musics, or streams; 
        /// it does not affect any that already exist.
        /// On Windows, DirectX 7 or above is required for this option to have effect.
        /// On other platforms, only the <see cref="ManagedBass.Dynamics.Algorithm3D.Default"/> and <see cref="ManagedBass.Dynamics.Algorithm3D.Off"/> options are available.
        /// </remarks>
        public static Algorithm3D Algorithm3D
        {
            get { return (Algorithm3D)GetConfig(Configuration.Algorithm3D); }
            set { Configure(Configuration.Algorithm3D, (int)value); }
        }

        /// <summary>
        /// Retrieves the 3D attributes of a sample, stream, or MOD music channel with 3D functionality.
        /// </summary>
        /// <param name="Handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Mode">The 3D processing mode (see <see cref="Mode3D" />).</param>
        /// <param name="Min">The minimum distance.</param>
        /// <param name="Max">The maximum distance.</param>
        /// <param name="iAngle">The angle of the inside projection cone.</param>
        /// <param name="oAngle">The angle of the outside projection cone.</param>
        /// <param name="OutVol">The delta-volume outside the outer projection cone.</param>
        /// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>The <paramref name="iAngle"/> and <paramref name="oAngle"/> parameters must both be retrieved in a single call to this function (ie. you can't retrieve one without the other).</remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not a valid channel.</exception>
        /// <exception cref="Errors.No3D">The channel does not have 3D functionality.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGet3DAttributes")]
        public static extern bool ChannelGet3DAttributes(int Handle, ref Mode3D Mode, ref float Min, ref float Max, ref int iAngle, ref int oAngle, ref float OutVol);

        /// <summary>
        /// Sets the 3D attributes of a sample, stream, or MOD music channel with 3D functionality.
        /// </summary>
        /// <param name="Handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Mode">The 3D processing mode... one of these flags, -1 = leave current (see <see cref="Mode3D" />)</param>
        /// <param name="Min">The minimum distance. The channel's volume is at maximum when the listener is within this distance... less than 0.0 = leave current.</param>
        /// <param name="Max">The maximum distance. The channel's volume stops decreasing when the listener is beyond this distance... less than 0.0 = leave current.</param>
        /// <param name="iAngle">The angle of the inside projection cone in degrees... 0 (no cone) - 360 (sphere), -1 = leave current.</param>
        /// <param name="oAngle">The angle of the outside projection cone in degrees... 0 (no cone) - 360 (sphere), -1 = leave current.</param>
        /// <param name="OutVol">The delta-volume outside the outer projection cone... 0 (silent) - 100 (same as inside the cone), -1 = leave current.</param>
        /// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not a valid channel.</exception>
        /// <exception cref="Errors.No3D">The channel does not have 3D functionality.</exception>
        /// <exception cref="Errors.IllegalParameter">One or more of the attribute values is invalid.</exception>
        /// <remarks>
        /// <para>The <paramref name="iAngle"/> and <paramref name="oAngle"/> parameters must both be set in a single call to this function (ie. you can't set one without the other). 
        /// The <paramref name="iAngle"/> and <paramref name="oAngle"/> angles decide how wide the sound is projected around the orientation angle. Within the inside angle the volume level is the channel volume, as set with <see cref="ChannelSetAttribute(int,ChannelAttribute,float)" />.
        /// Outside the outer angle, the volume changes according to the <paramref name="OutVol"/> value. Between the inner and outer angles, the volume gradually changes between the inner and outer volume levels. 
        /// If the inner and outer angles are 360 degrees, then the sound is transmitted equally in all directions.</para>
        /// <para>As with all 3D functions, use <see cref="Apply3D" /> to apply the changes made.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSet3DAttributes")]
        public static extern bool ChannelSet3DAttributes(int Handle, Mode3D Mode, float Min, float Max, int iAngle, int oAngle, float OutVol);

        /// <summary>
        /// Retrieves the 3D position of a sample, stream, or MOD music channel with 3D functionality.
        /// </summary>
        /// <param name="Handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Position">Position of the sound.</param>
        /// <param name="Orientation">Orientation of the sound.</param>
        /// <param name="Velocity">Velocity of the sound.</param>
        /// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not a valid channel.</exception>
        /// <exception cref="Errors.No3D">The channel does not have 3D functionality.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGet3DPosition")]
        public static extern bool ChannelGet3DPosition(int Handle, ref Vector3D Position, ref Vector3D Orientation, ref Vector3D Velocity);

        /// <summary>
        /// Sets the 3D position of a sample, stream, or MOD music channel with 3D functionality.
        /// </summary>
        /// <param name="Handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Position">Position of the sound.</param>
        /// <param name="Orientation">Orientation of the sound.</param>
        /// <param name="Velocity">Velocity of the sound. This is only used to calculate the doppler effect, and has no effect on the sound's position.</param>
        /// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>As with all 3D functions, <see cref="Apply3D" /> must be called to apply the changes made.</remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not a valid channel.</exception>
        /// <exception cref="Errors.No3D">The channel does not have 3D functionality.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSet3DPosition")]
        public static extern bool ChannelSet3DPosition(int Handle, Vector3D Position, Vector3D Orientation, Vector3D Velocity);
    }
}