using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using VDM.GUI.ModelExtensions;
using VDM.Data.Model;



namespace VDM.GUI.Helpers
{
    public static class Converters
    {
        public static SelectableVDP VDPModelToGUI(VirtualDesktopPreset vdp)
        {
            return new SelectableVDP()
            {
                Id = vdp.Id,
                Name = vdp.Name,
                Icon = vdp.Icon,
                IsOpenedOnSystemStart = vdp.IsOpenedOnSystemStart,
                AttachedProcesses = vdp.AttachedProcesses,

                IsSelected = false
            };
        }

        public static byte[] ImagePathToByteArray(string path)
        {
            using (var icon = Image.FromFile(path))
            using (var memStream = new MemoryStream())
            {
                icon.Save(memStream, icon.RawFormat);
                return memStream.ToArray();
            }
        }

        public static ImageSource ByteArrayToImageSource(byte[] imgData)
        {
            Image image = ByteArrayToImage(imgData);
            var bitmap = new Bitmap(image);

            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }



        private static Image ByteArrayToImage(byte[] imgData)
        {
            using (var memStream = new MemoryStream(imgData))
            {
                return Image.FromStream(memStream);
            }
        }
    }
}
