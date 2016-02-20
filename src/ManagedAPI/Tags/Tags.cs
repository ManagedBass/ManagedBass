using ManagedBass.Dynamics;
using System.Collections.Generic;
using static System.Runtime.InteropServices.Marshal;
using static ManagedBass.Extensions;

namespace ManagedBass
{
    public static class Tags
    {
        public static string GetLyrics3v2(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.Lyrics3v2));

        public static string GetVendor(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.OggEncoder));

        public static string GetRiffDisp(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffDISP));

        public static string GetIcyShoutcastMeta(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.META));

        public static string GetOggEncoder(int Handle) => PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.OggEncoder));

        public static string GetWMAMidStreamTag(int Handle) => PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.WmaMeta));

        public static string[] GetOggComments(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.OGG));

        public static string[] GetHTTPHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.HTTP));

        public static string[] GetICYHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.ICY));

        public static string[] GetAPETags(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.APE));

        public static string[] GetMP4Metadata(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MP4));

        public static string[] GetWMATags(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.WMA));

        public static string[] GetMFTags(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MF));

        public static string[] GetRiffTags(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffInfo));
    }
}
