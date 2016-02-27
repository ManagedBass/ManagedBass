// Adopted from the great article: http://www.codeproject.com/Articles/17890/Do-Anything-With-ID

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedBass
{
    class ID3v2Reader
    {
        enum TextEncodings
        {
            Ascii,
            UTF_16,
            UTF_16BE,
            UTF8
        }

        IntPtr ptr;
        Version VersionInfo;

        public ID3v2Reader(IntPtr Pointer)
        {
            ptr = Pointer;

            TextFrames = new Dictionary<string, string>();

            if (ReadText(3, TextEncodings.Ascii) != "ID3") // If don't contain ID3v2 tag
                throw new DataMisalignedException("ID3v2 info not found");

            VersionInfo = new Version(2, ReadByte(), ReadByte()); // Read ID3v2 version           
            ptr += 1; // Flags are skipped

            ReadAllFrames(ReadSize());
        }

        void ReadAllFrames(int Length)
        {
            string FrameID;
            int FrameLength;
            byte Buf;

            // If ID3v2 is ID3v2.2 FrameID, FrameLength of Frames is 3 byte
            // otherwise it's 4 character
            int FrameIDLen = VersionInfo.Minor == 2 ? 3 : 4;

            // Minimum frame size is 10 because frame header is 10 byte
            while (Length > 10)
            {
                // check for padding( 00 bytes )
                Buf = ReadByte();

                if (Buf == 0)
                {
                    Length--;
                    continue;
                }

                // if readed byte is not zero. it must read as FrameID
                ptr -= 1;

                // ---------- Read Frame Header -----------------------
                FrameID = ReadText(FrameIDLen, TextEncodings.Ascii);
                FrameLength = Convert.ToInt32(ReadUInt(FrameIDLen));

                if (FrameIDLen == 4)
                    ReadUInt(2);

                bool Added = AddFrame(FrameID, FrameLength);

                // if don't read this frame, we must go forward to read next frame
                if (!Added)
                    ptr += FrameLength;

                Length -= FrameLength + 10;
            }
        }

        bool AddFrame(string FrameID, int Length)
        {
            if (FrameID == null || !IsValidFrameID(FrameID))
                return false;

            if (FrameID[0].Is('T', 'W')) // Is Text Frame
            {
                bool IsURL = FrameID[0] == 'W';

                TextEncodings TextEncoding;

                if (IsURL) TextEncoding = TextEncodings.Ascii;
                else
                {
                    TextEncoding = (TextEncodings)ReadByte();
                    Length--;
                    if (!Enum.IsDefined(typeof(TextEncodings), TextEncoding))
                        return false;
                }

                var Text = ReadText(Length, TextEncoding);

                TextFrames.Add(FrameID, Text);
                return true;
            }

            return false;
        }

        static bool IsValidFrameID(string FrameID)
        {
            if (FrameID == null || !FrameID.Length.Is(3, 4))
                return false;

            foreach (char ch in FrameID)
                if (!Char.IsUpper(ch) && !char.IsDigit(ch))
                    return false;

            return true;
        }

        public Dictionary<string, string> TextFrames { get; }

        #region Streaming
        string ReadText(int MaxLength, TextEncodings TEncoding)
        {
            if (MaxLength <= 0)
                return "";

            var MStream = new MemoryStream();

            if (MaxLength >= 3)
            {
                byte[] Buffer = new byte[3];

                Read(Buffer, 0, Buffer.Length);

                if (Buffer[0] == 0xFF && Buffer[1] == 0xFE)
                {   // FF FE
                    TEncoding = TextEncodings.UTF_16;// UTF-16 (LE)

                    ptr -= 1;

                    MaxLength -= 2;
                }
                else if (Buffer[0] == 0xFE && Buffer[1] == 0xFF)
                {   // FE FF
                    TEncoding = TextEncodings.UTF_16BE;

                    ptr -= 1;

                    MaxLength -= 2;
                }
                else if (Buffer[0] == 0xEF && Buffer[1] == 0xBB && Buffer[2] == 0xBF)
                {
                    // EF BB BF
                    TEncoding = TextEncodings.UTF8;
                    MaxLength -= 3;
                }
                else ptr -= 3;
            }
            bool Is2ByteSeprator = TEncoding.Is(TextEncodings.UTF_16, TextEncodings.UTF_16BE);

            byte Buf;
            while (MaxLength > 0)
            {
                Buf = ReadByte(); // Read First/Next byte from stream

                if (Buf != 0) // if it's data byte
                    MStream.WriteByte(Buf);
                else // if Buf == 0
                {
                    if (Is2ByteSeprator)
                    {
                        byte Temp = ReadByte();
                        if (Temp == 0)
                            break;
                        else
                        {
                            MStream.WriteByte(Buf);
                            MStream.WriteByte(Temp);
                            MaxLength--;
                        }
                    }
                    else break;
                }
                MaxLength--;
            }

            if (MaxLength < 0)
                ptr += MaxLength;

            return GetEncoding(TEncoding).GetString(MStream.ToArray());
        }

        byte ReadByte()
        {
            byte[] RByte = new byte[1];

            Read(RByte, 0, 1);

            return RByte[0];
        }

        uint ReadUInt(int Length)
        {
            if (Length > 4 || Length < 1)
                throw (new ArgumentOutOfRangeException("ReadUInt method can read 1-4 byte(s)"));

            byte[] Buf = new byte[Length],
                   RBuf = new byte[4];

            Read(Buf, 0, Length);

            Buf.CopyTo(RBuf, 4 - Buf.Length);
            Array.Reverse(RBuf);

            return BitConverter.ToUInt32(RBuf, 0);
        }

        int ReadSize()
        {
            /* ID3 Size is like:
             * 0XXXXXXXb 0XXXXXXXb 0XXXXXXXb 0XXXXXXXb (b means binary)
             * the zero bytes must ignore, so we have 28 bits number = 0x1000 0000 (maximum)
             * it's equal to 256MB
             */
            return ReadByte() * 0x200000 + ReadByte() * 0x4000 + ReadByte() * 0x80 + ReadByte();
        }

        Encoding GetEncoding(TextEncodings TEncoding)
        {
            switch (TEncoding)
            {
                case TextEncodings.UTF_16:
                    return Encoding.Unicode;
                case TextEncodings.UTF_16BE:
                    return Encoding.GetEncoding("UTF-16BE");
                case TextEncodings.UTF8:
                    return Encoding.UTF8;
                default:
                    return Encoding.Default;
            }
        }

        void Read(byte[] buffer, int offset, int count)
        {
            Marshal.Copy(ptr, buffer, offset, count);
            ptr += count;
        }
        #endregion
    }
}
