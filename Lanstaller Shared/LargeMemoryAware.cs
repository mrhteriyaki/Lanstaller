using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.InteropServices;


namespace LanstallerShared
{
    public class LargeMemoryAware
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct IMAGE_OPTIONAL_HEADER32
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public uint BaseOfData;
            public uint ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public uint SizeOfStackReserve;
            public uint SizeOfStackCommit;
            public uint SizeOfHeapReserve;
            public uint SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;
        }

        const ushort IMAGE_FILE_32BIT_MACHINE = 0x0100;
        const ushort IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020;
        const int IMAGE_DOS_SIGNATURE = 0x5A4D;
        const int IMAGE_NT_SIGNATURE = 0x00004550;

        public static bool IsLargeAddressAware(string filePath, out bool is32Bit)
        {
            is32Bit = false;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
            {
                // Read DOS header
                reader.BaseStream.Seek(0x00, SeekOrigin.Begin);
                if (reader.ReadUInt16() != IMAGE_DOS_SIGNATURE)
                {
                    throw new InvalidDataException("Not a valid DOS header");
                }

                reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                int peHeaderOffset = reader.ReadInt32();

                // Read PE signature
                reader.BaseStream.Seek(peHeaderOffset, SeekOrigin.Begin);
                if (reader.ReadUInt32() != IMAGE_NT_SIGNATURE)
                {
                    throw new InvalidDataException("Not a valid PE file");
                }

                // Read File Header
                IMAGE_FILE_HEADER fileHeader = ReadStruct<IMAGE_FILE_HEADER>(reader);

                // Check if it's 32-bit
                is32Bit = (fileHeader.Characteristics & IMAGE_FILE_32BIT_MACHINE) != 0;

                if (is32Bit)
                {
                    // Read Optional Header
                    IMAGE_OPTIONAL_HEADER32 optionalHeader = ReadStruct<IMAGE_OPTIONAL_HEADER32>(reader);

                    // Check if Large Address Aware flag is set
                    bool isLargeAddressAware = (fileHeader.Characteristics & IMAGE_FILE_LARGE_ADDRESS_AWARE) != 0;
                    return isLargeAddressAware;
                }
                else
                {
                    //throw new InvalidDataException("The file is not a 32-bit executable");
                    return false;
                }
            }
        }

        public static void SetLargeAddressAware(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                // Read DOS header
                stream.Seek(0x00, SeekOrigin.Begin);
                if (reader.ReadUInt16() != IMAGE_DOS_SIGNATURE)
                {
                    throw new InvalidDataException("Not a valid DOS header");
                }

                stream.Seek(0x3C, SeekOrigin.Begin);
                int peHeaderOffset = reader.ReadInt32();

                // Read PE signature
                stream.Seek(peHeaderOffset, SeekOrigin.Begin);
                if (reader.ReadUInt32() != IMAGE_NT_SIGNATURE)
                {
                    throw new InvalidDataException("Not a valid PE file");
                }

                // Read File Header
                long fileHeaderPos = stream.Position;
                IMAGE_FILE_HEADER fileHeader = ReadStruct<IMAGE_FILE_HEADER>(reader);

                // Ensure it's 32-bit
                if ((fileHeader.Characteristics & IMAGE_FILE_32BIT_MACHINE) == 0)
                {
                    throw new InvalidDataException("The file is not a 32-bit executable");
                }

                // Set the Large Address Aware flag
                fileHeader.Characteristics |= IMAGE_FILE_LARGE_ADDRESS_AWARE;

                // Write the modified header back to the file
                stream.Seek(fileHeaderPos, SeekOrigin.Begin);
                WriteStruct(writer, fileHeader);
            }

            Console.WriteLine("Large Address Aware flag has been enabled.");
        }

        private static T ReadStruct<T>(BinaryReader reader) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var bytes = reader.ReadBytes(size);
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        private static void WriteStruct<T>(BinaryWriter writer, T value) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, bytes, 0, size);
                writer.Write(bytes);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

    }

}
