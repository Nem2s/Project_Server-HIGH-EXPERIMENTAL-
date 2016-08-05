using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Helper
{
    [Serializable]
    internal class ProcessDataHelper
    {
        public ProcessDataHelper(string value, byte[] img) { Str = value; Image = img; }
        public string Str { get; set; }
        public byte[] Image { get; set; }
    }

    internal class ProcessData
    {
        public ProcessData(string value, BitmapImage img) { Str = value; Image = img; }
        public string Str { get; set; }
        public BitmapImage Image { get; set; }
    }

}

public static class HelpMePlease
{
    public static BitmapImage ToBitmapImage(this Bitmap bitmap)
    {
        using (var memory = new MemoryStream())
        {
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        
    }

    private static string GetBitmapImageString(BitmapImage bi)
    {
        string s = "";
        byte[] bytes = GetBitmapImageByteArray(bi);
        s = Convert.ToBase64String(bytes);
        return s;
    }

    public static byte[] GetBitmapImageByteArray(BitmapImage src)
    {
        MemoryStream stream = new MemoryStream();
        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)src));
        encoder.Save(stream);
        stream.Flush();
        return stream.ToArray();
    }

    public static BitmapImage LoadBitmapImageFromByteArray(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;
        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }
}

internal sealed class MyBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;

        String currentAssembly = Assembly.GetExecutingAssembly().FullName;

        // In this case we are always using the current assembly
        assemblyName = currentAssembly;

        // Get the type using the typeName and assemblyName
        typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
            typeName, assemblyName));

        return typeToDeserialize;
    }

   
}


