using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
        /// Retrieves the decoding/download/end position of a file stream.
        /// </summary>
        /// <param name="Handle">The stream's handle.</param>
        /// <param name="Mode">The file position to retrieve. One of <see cref="FileStreamPosition" /> values.</param>
        /// <returns>If succesful, then the requested file position is returned, else -1 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>ID3 tags (both v1 and v2) and WAVE headers, as well as any other rubbish at the start of the file, are excluded from the calculations of this function.</para>
        /// <para>This is useful for average bitrate calculations, but it means that the <see cref="FileStreamPosition.Current"/> position may not be the actual file position - the <see cref="FileStreamPosition.Start"/> position can be added to it to get the actual file position.</para>
        /// <para>
        /// When streaming a file from the internet or a "buffered" user file stream, the entire file is downloaded even if the audio data ends before that, in case there are tags to be read.
        /// This means that the <see cref="FileStreamPosition.Download"/> position may go beyond the <see cref="FileStreamPosition.End"/> position.
        /// </para>
        /// <para>
        /// It's unwise to use this function (with mode = <see cref="FileStreamPosition.Current"/>) for syncing purposes because it returns the position that's being decoded, not the position that's being heard.
        /// Use <see cref="ChannelGetPosition" /> for syncing instead.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotFile">The stream is not a file stream.</exception>
        /// <exception cref="Errors.NotAvailable">The requested file position/status is not available.</exception>
        [DllImport(DllName, EntryPoint = "BASS_StreamGetFilePosition")]
        public static extern long StreamGetFilePosition(int Handle, FileStreamPosition Mode = FileStreamPosition.Current);

        #region StreamCreateFile
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_StreamCreateFile(bool Memory, string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        /// <summary>
        /// Creates a sample stream from an MP3, MP2, MP1, OGG, WAV, AIFF or plugin supported file.
        /// <para>This overload implements streaming from file.</para>
        /// </summary>
        /// <param name="File">Filename for which a stream should be created.</param>
        /// <param name="Offset">File Offset to begin streaming from.</param>
        /// <param name="Length">Data length... 0 = use all data up to the end of the file.</param>
        /// <param name="Flags">Any combination of <see cref="BassFlags"/>.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// BASS has built-in support for MPEG, OGG, WAV and AIFF files.
        /// Support for additional formats is available via add-ons, which can be downloaded from the BASS website: <a href="http://www.un4seen.com">www.un4seen.com</a>.
        /// </para>
        /// <para>
        /// MPEG 1.0, 2.0 and 2.5 layer 3 (MP3) files are supported, layers 1 (MP1) and 2 (MP2) are also supported.
        /// Standard RIFF and RF64 WAV files are supported, with the sample data in a PCM format or compressed with an ACM codec, but the codec is required to be installed on the user's system for the WAV to be decoded.
        /// So you should either distribute the codec with your software, or use a codec that comes with Windows (eg. Microsoft ADPCM).
        /// All PCM formats from 8 to 32-bit are supported in WAV and AIFF files, but the output will be restricted to 16-bit unless the <see cref="BassFlags.Float"/> flag is used.
        /// 64-bit floating-point WAV and AIFF files are also supported, but are rendered in 16-bit or 32-bit floating-point depending on the flags.
        /// The file's original resolution is available via <see cref="ChannelGetInfo(int, out ChannelInfo)" />.
        /// </para>
        /// <para>
        /// Chained OGG files containing multiple logical bitstreams are supported, but seeking within them is only fully supported if the <see cref="BassFlags.Prescan"/> flag is used (or the <see cref="OggPreScan"/> config option is enabled) to have them pre-scanned.
        /// Without pre-scanning, seeking will only be possible back to the start.
        /// The <see cref="PositionFlags.OGG"/> mode can be used with <see cref="ChannelGetLength" /> to get the number of bitstreams and with <see cref="ChannelSetPosition" /> to seek to a particular one.
        /// A <see cref="SyncFlags.OggChange"/> sync can be set via <see cref="ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" /> to be informed of when a new bitstream begins during decoding/playback.
        /// </para>
        /// <para>Multi-channel (ie. more than stereo) OGG, WAV and AIFF files are supported.</para>
        /// <para>
        /// Use <see cref="ChannelGetInfo(int, out ChannelInfo)" /> to retrieve information on the format (sample rate, resolution, channels) of the stream.
        /// The playback length of the stream can be retrieved using <see cref="ChannelGetLength" />.
        /// </para>
        /// <para>
        /// If <paramref name="Length"/> = 0 (use all data up to the end of the file), and the file length increases after creating the stream (ie. the file is still being written), then BASS will play the extra data too, but the length returned by <see cref="ChannelGetLength" /> will not be updated until the end is reached.
        /// The <see cref="StreamGetFilePosition" /> return values will be updated during playback of the extra data though.
        /// </para>
        /// <para>
        /// To stream a file from the internet, use <see cref="CreateStream(string, int, BassFlags, DownloadProcedure, IntPtr)" />.
        /// To stream from other locations, see <see cref="M:Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr)" />.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Away from Windows, all mixing is done in software (by BASS), so the <see cref="BassFlags.SoftwareMixing"/> flag is unnecessary.
        /// The <see cref="BassFlags.FX"/> flag is also ignored.
        /// </para>
        /// <para>
        /// On Windows and Windows CE, ACM codecs are supported with compressed WAV files.
        /// Media Foundation codecs are also supported on Windows 7 and updated versions of Vista, including support for AAC/MP4 and WMA.
        /// On iOS and OSX, CoreAudio codecs are supported, adding support for any file formats that have a codec installed.
        /// Media Foundation and CoreAudio codecs are only tried after the built-in decoders and any plugins have rejected the file.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">Only decoding channels (<see cref="BassFlags.Decode"/>) are allowed when using the "no sound" device. The <see cref="BassFlags.AutoFree"/> flag is also unavailable to decoding channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Length" /> must be specified when streaming from memory.</exception>
        /// <exception cref="Errors.FileOpen">The <paramref name="File"/> could not be opened.</exception>
        /// <exception cref="Errors.FileFormat">The file's format is not recognised/supported.</exception>
        /// <exception cref="Errors.Codec">The file uses a codec that's not available/supported. This can apply to WAV and AIFF files, and also MP3 files when using the "MP3-free" BASS version.</exception>
        /// <exception cref="Errors.SampleFormat">The sample format is not supported by the device/drivers. If the stream is more than stereo or the <see cref="BassFlags.Float"/> flag is used, it could be that they are not supported.</exception>
        /// <exception cref="Errors.Speaker">The specified SPEAKER flags are invalid. The device/drivers do not support them, they are attempting to assign a stereo stream to a mono speaker or 3D functionality is enabled.</exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.No3D">Could not initialize 3D support.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        /// <summary>
        /// Creates a sample stream from an MP3, MP2, MP1, OGG, WAV, AIFF or plugin supported memory IntPtr.
        /// <para>This overload implements streaming from memory.</para>
        /// </summary>
        /// <param name="Memory">An unmanaged pointer to the memory location as an IntPtr.</param>
        /// <param name="Offset">Offset to begin streaming from.</param>
        /// <param name="Length">Data length (needs to be set to the length of the memory stream in bytes which should be played).</param>
        /// <param name="Flags">Any combination of <see cref="BassFlags"/>.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// BASS has built-in support for MPEG, OGG, WAV and AIFF files.
        /// Support for additional formats is available via add-ons, which can be downloaded from the BASS website: <a href="http://www.un4seen.com">www.un4seen.com</a>.
        /// </para>
        /// <para>
        /// MPEG 1.0, 2.0 and 2.5 layer 3 (MP3) files are supported, layers 1 (MP1) and 2 (MP2) are also supported.
        /// Standard RIFF and RF64 WAV files are supported, with the sample data in a PCM format or compressed with an ACM codec, but the codec is required to be installed on the user's system for the WAV to be decoded.
        /// So you should either distribute the codec with your software, or use a codec that comes with Windows (eg. Microsoft ADPCM).
        /// All PCM formats from 8 to 32-bit are supported in WAV and AIFF files, but the output will be restricted to 16-bit unless the <see cref="BassFlags.Float"/> flag is used.
        /// 64-bit floating-point WAV and AIFF files are also supported, but are rendered in 16-bit or 32-bit floating-point depending on the flags.
        /// The file's original resolution is available via <see cref="ChannelGetInfo(int, out ChannelInfo)" />.
        /// </para>
        /// <para>
        /// Chained OGG files containing multiple logical bitstreams are supported, but seeking within them is only fully supported if the <see cref="BassFlags.Prescan"/> flag is used (or the <see cref="OggPreScan"/> config option is enabled) to have them pre-scanned.
        /// Without pre-scanning, seeking will only be possible back to the start.
        /// The <see cref="PositionFlags.OGG"/> mode can be used with <see cref="ChannelGetLength" /> to get the number of bitstreams and with <see cref="ChannelSetPosition" /> to seek to a particular one.
        /// A <see cref="SyncFlags.OggChange"/> sync can be set via <see cref="ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" /> to be informed of when a new bitstream begins during decoding/playback.
        /// </para>
        /// <para>Multi-channel (ie. more than stereo) OGG, WAV and AIFF files are supported.</para>
        /// <para>
        /// Use <see cref="ChannelGetInfo(int, out ChannelInfo)" /> to retrieve information on the format (sample rate, resolution, channels) of the stream.
        /// The playback length of the stream can be retrieved using <see cref="ChannelGetLength" />.
        /// </para>
        /// <para>
        /// If <paramref name="Length"/> = 0 (use all data up to the end of the file), and the file length increases after creating the stream (ie. the file is still being written), then BASS will play the extra data too, but the length returned by <see cref="ChannelGetLength" /> will not be updated until the end is reached.
        /// The <see cref="StreamGetFilePosition" /> return values will be updated during playback of the extra data though.
        /// </para>
        /// <para>
        /// When streaming from memory, the memory must not be freed before the stream is freed.
        /// There may be exceptions to that with some add-ons (see the documentation).
        /// </para>
        /// <para>
        /// To stream a file from the internet, use <see cref="CreateStream(string, int, BassFlags, DownloadProcedure, IntPtr)" />.
        /// To stream from other locations, see <see cref="M:Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr)" />.
        /// </para>
        /// <para>Don't forget to pin your memory object when using this overload.</para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Away from Windows, all mixing is done in software (by BASS), so the <see cref="BassFlags.SoftwareMixing"/> flag is unnecessary.
        /// The <see cref="BassFlags.FX"/> flag is also ignored.
        /// </para>
        /// <para>
        /// On Windows and Windows CE, ACM codecs are supported with compressed WAV files.
        /// Media Foundation codecs are also supported on Windows 7 and updated versions of Vista, including support for AAC/MP4 and WMA.
        /// On iOS and OSX, CoreAudio codecs are supported, adding support for any file formats that have a codec installed.
        /// Media Foundation and CoreAudio codecs are only tried after the built-in decoders and any plugins have rejected the file.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">Only decoding channels (<see cref="BassFlags.Decode"/>) are allowed when using the "no sound" device. The <see cref="BassFlags.AutoFree"/> flag is also unavailable to decoding channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Length" /> must be specified when streaming from memory.</exception>
        /// <exception cref="Errors.FileFormat">The file's format is not recognised/supported.</exception>
        /// <exception cref="Errors.Codec">The file uses a codec that's not available/supported. This can apply to WAV and AIFF files, and also MP3 files when using the "MP3-free" BASS version.</exception>
        /// <exception cref="Errors.SampleFormat">The sample format is not supported by the device/drivers. If the stream is more than stereo or the <see cref="BassFlags.Float"/> flag is used, it could be that they are not supported.</exception>
        /// <exception cref="Errors.Speaker">The specified SPEAKER flags are invalid. The device/drivers do not support them, they are attempting to assign a stereo stream to a mono speaker or 3D functionality is enabled.</exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.No3D">Could not initialize 3D support.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_StreamCreateFile(true, new IntPtr(Memory.ToInt32() + Offset), 0, Length, Flags);
        }

        /// <summary>
        /// Creates a sample stream from an MP3, MP2, MP1, OGG, WAV, AIFF or plugin supported file.
        /// <para>This overload implements streaming from a byte[].</para>
        /// </summary>
        /// <param name="Memory">A byte[] containing file data.</param>
        /// <param name="Offset">Offset to begin streaming from.</param>
        /// <param name="Length">Data length (needs to be set to the length of the memory stream in bytes which should be played).</param>
        /// <param name="Flags">Any combination of <see cref="BassFlags"/>.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// BASS has built-in support for MPEG, OGG, WAV and AIFF files.
        /// Support for additional formats is available via add-ons, which can be downloaded from the BASS website: <a href="http://www.un4seen.com">www.un4seen.com</a>.
        /// </para>
        /// <para>
        /// MPEG 1.0, 2.0 and 2.5 layer 3 (MP3) files are supported, layers 1 (MP1) and 2 (MP2) are also supported.
        /// Standard RIFF and RF64 WAV files are supported, with the sample data in a PCM format or compressed with an ACM codec, but the codec is required to be installed on the user's system for the WAV to be decoded.
        /// So you should either distribute the codec with your software, or use a codec that comes with Windows (eg. Microsoft ADPCM).
        /// All PCM formats from 8 to 32-bit are supported in WAV and AIFF files, but the output will be restricted to 16-bit unless the <see cref="BassFlags.Float"/> flag is used.
        /// 64-bit floating-point WAV and AIFF files are also supported, but are rendered in 16-bit or 32-bit floating-point depending on the flags.
        /// The file's original resolution is available via <see cref="ChannelGetInfo(int, out ChannelInfo)" />.
        /// </para>
        /// <para>
        /// Chained OGG files containing multiple logical bitstreams are supported, but seeking within them is only fully supported if the <see cref="BassFlags.Prescan"/> flag is used (or the <see cref="OggPreScan"/> config option is enabled) to have them pre-scanned.
        /// Without pre-scanning, seeking will only be possible back to the start.
        /// The <see cref="PositionFlags.OGG"/> mode can be used with <see cref="ChannelGetLength" /> to get the number of bitstreams and with <see cref="ChannelSetPosition" /> to seek to a particular one.
        /// A <see cref="SyncFlags.OggChange"/> sync can be set via <see cref="ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" /> to be informed of when a new bitstream begins during decoding/playback.
        /// </para>
        /// <para>Multi-channel (ie. more than stereo) OGG, WAV and AIFF files are supported.</para>
        /// <para>
        /// Use <see cref="ChannelGetInfo(int, out ChannelInfo)" /> to retrieve information on the format (sample rate, resolution, channels) of the stream.
        /// The playback length of the stream can be retrieved using <see cref="ChannelGetLength" />.
        /// </para>
        /// <para>
        /// If <paramref name="Length"/> = 0 (use all data up to the end of the file), and the file length increases after creating the stream (ie. the file is still being written), then BASS will play the extra data too, but the length returned by <see cref="ChannelGetLength" /> will not be updated until the end is reached.
        /// The <see cref="StreamGetFilePosition" /> return values will be updated during playback of the extra data though.
        /// </para>
        /// <para>
        /// The <paramref name="Memory"/> is pinned by this overload and freed when the stream is freed.
        /// </para>
        /// <para>
        /// To stream a file from the internet, use <see cref="CreateStream(string, int, BassFlags, DownloadProcedure, IntPtr)" />.
        /// To stream from other locations, see <see cref="M:Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr)" />.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Away from Windows, all mixing is done in software (by BASS), so the <see cref="BassFlags.SoftwareMixing"/> flag is unnecessary.
        /// The <see cref="BassFlags.FX"/> flag is also ignored.
        /// </para>
        /// <para>
        /// On Windows and Windows CE, ACM codecs are supported with compressed WAV files.
        /// Media Foundation codecs are also supported on Windows 7 and updated versions of Vista, including support for AAC/MP4 and WMA.
        /// On iOS and OSX, CoreAudio codecs are supported, adding support for any file formats that have a codec installed.
        /// Media Foundation and CoreAudio codecs are only tried after the built-in decoders and any plugins have rejected the file.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">Only decoding channels (<see cref="BassFlags.Decode"/>) are allowed when using the "no sound" device. The <see cref="BassFlags.AutoFree"/> flag is also unavailable to decoding channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Length" /> must be specified when streaming from memory.</exception>
        /// <exception cref="Errors.FileFormat">The file's format is not recognised/supported.</exception>
        /// <exception cref="Errors.Codec">The file uses a codec that's not available/supported. This can apply to WAV and AIFF files, and also MP3 files when using the "MP3-free" BASS version.</exception>
        /// <exception cref="Errors.SampleFormat">The sample format is not supported by the device/drivers. If the stream is more than stereo or the <see cref="BassFlags.Float"/> flag is used, it could be that they are not supported.</exception>
        /// <exception cref="Errors.Speaker">The specified SPEAKER flags are invalid. The device/drivers do not support them, they are attempting to assign a stereo stream to a mono speaker or 3D functionality is enabled.</exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.No3D">Could not initialize 3D support.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }
        #endregion

        /// <summary>
        /// Creates a sample stream from an MP3, MP2, MP1, OGG, WAV, AIFF or plugin supported file via user callback functions.
        /// </summary>
        /// <param name="System">File system to use.</param>
        /// <param name="Flags">Any combination of <see cref="BassFlags"/>.</param>
        /// <param name="Procedures">The user defined file function (see <see cref="FileProcedures" />).</param>
        /// <param name="User">User instance data to pass to the callback functions.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// The buffered file system (<see cref="StreamSystem.Buffer"/>) is what is used by <see cref="CreateStream(string, int, BassFlags, DownloadProcedure, IntPtr)" />.
        /// As the name suggests, data from the file is buffered so that it's readily available for decoding - BASS creates a thread dedicated to "downloading" the data.
        /// This is ideal for when the data is coming from a source that has high latency, like the internet.
        /// It's not possible to seek in buffered file streams, until the download has reached the requested position - it's not possible to seek at all if it's being streamed in blocks.
        /// </para>
        /// <para>
        /// The push buffered file system (<see cref="StreamSystem.BufferPush"/>) is the same, except that instead of the file data being pulled from the <see cref="FileReadProcedure" /> function in a "download" thread, the data is pushed to BASS via <see cref="StreamPutFileData(int, IntPtr, int)" />.
        /// A <see cref="FileReadProcedure" /> function is still required, to get the initial data used in the creation of the stream.
        /// </para>
        /// <para>
        /// The unbuffered file system (<see cref="StreamSystem.NoBuffer"/>) is what is used by <see cref="CreateStream(string, long, long, BassFlags)" />.
        /// In this system, BASS does not do any intermediate buffering - it simply requests data from the file as and when it needs it.
        /// This means that reading (<see cref="FileReadProcedure" />) must be quick, otherwise the decoding will be delayed and playback buffer underruns (old data repeated) are a possibility.
        /// It's not so important for seeking (<see cref="FileSeekProcedure" />) to be fast, as that is generally not required during decoding, except when looping a file.
        /// </para>
        /// <para>In all cases, BASS will automatically stall playback of the stream when insufficient data is available, and resume it when enough data does become available.</para>
        /// <para>
        /// A copy is made of the procs callback function table, so it does not have to persist beyond this function call. 
        /// This means it is not required to pin the <paramref name="Procedures" /> instance, but it is still required to keep a reference as long as BASS uses the callback delegates in order to prevent the callbacks from being garbage collected.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Away from Windows, all mixing is done in software (by BASS), so the <see cref="BassFlags.SoftwareMixing"/> flag is unnecessary.
        /// The <see cref="BassFlags.FX"/> flag is also ignored.
        /// </para>
        /// <para>
        /// On Windows and Windows CE, ACM codecs are supported with compressed WAV files.
        /// Media Foundation codecs are also supported on Windows 7 and updated versions of Vista, including support for AAC/MP4 and WMA.
        /// On iOS and OSX, CoreAudio codecs are supported, adding support for any file formats that have a codec installed.
        /// Media Foundation and CoreAudio codecs are only tried after the built-in decoders and any plugins have rejected the file.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">Only decoding channels (<see cref="BassFlags.Decode"/>) are allowed when using the "no sound" device. The <see cref="BassFlags.AutoFree"/> flag is also unavailable to decoding channels.</exception>
        /// <exception cref="Errors.Parameter"><paramref name="System" /> is not valid.</exception>
        /// <exception cref="Errors.FileFormat">The file's format is not recognised/supported.</exception>
        /// <exception cref="Errors.Codec">The file uses a codec that's not available/supported. This can apply to WAV and AIFF files, and also MP3 files when using the "MP3-free" BASS version.</exception>
        /// <exception cref="Errors.SampleFormat">The sample format is not supported by the device/drivers. If the stream is more than stereo or the <see cref="BassFlags.Float"/> flag is used, it could be that they are not supported.</exception>
        /// <exception cref="Errors.Speaker">The specified SPEAKER flags are invalid. The device/drivers do not support them, they are attempting to assign a stereo stream to a mono speaker or 3D functionality is enabled.</exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.No3D">Could not initialize 3D support.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_StreamCreateFileUser")]
        public static extern int CreateStream(StreamSystem System, BassFlags Flags, [In, Out] FileProcedures Procedures, IntPtr User = default(IntPtr));

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            return BASS_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User);
        }

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedureType Procedure, IntPtr User = default(IntPtr));

        #region Stream Put Data
        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, float[] Buffer, int Length);
        #endregion

        #region Stream Put File Data
        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, float[] Buffer, int Length);
        #endregion

        /// <summary>
        /// Frees a sample stream's resources, including any SYNC/DSP/FX it has.
        /// </summary>
        /// <param name="Handle"> The stream handle.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Init"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_StreamFree")]
        public static extern bool StreamFree(int Handle);
    }
}
