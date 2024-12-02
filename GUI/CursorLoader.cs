using System;
using System.IO;

namespace CorexOS.GUI
{
    public class CursorLoader
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[] PixelData { get; private set; }
        public int HotspotX { get; private set; }
        public int HotspotY { get; private set; }

        public CursorLoader(string bmpPath)
        {
            // Read the BMP file
            byte[] bmpData = File.ReadAllBytes(bmpPath);

            // Parse BMP header (simplified for 24-bit BMP files)
            Width = bmpData[18] | (bmpData[19] << 8);   // BMP width
            Height = bmpData[22] | (bmpData[23] << 8);  // BMP height

            // Extract pixel data (starting at offset 54 for 24-bit BMP)
            int pixelDataOffset = bmpData[10] | (bmpData[11] << 8);
            PixelData = new byte[bmpData.Length - pixelDataOffset];
            Array.Copy(bmpData, pixelDataOffset, PixelData, 0, PixelData.Length);

            // Set the hotspot (e.g., top-left corner)
            HotspotX = 0; // Adjust as needed
            HotspotY = 0; // Adjust as needed
        }
    }
}
