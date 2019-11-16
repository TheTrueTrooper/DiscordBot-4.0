using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace DiscordBot.Services
{
    public static class StaticServices
    {
        public static Random RandomN {get; private set;} = new Random();

        public static string AddFileEnding(Bitmap Bitmap, string Name = "TempImage")
        {
            if (Bitmap.RawFormat.Guid == ImageFormat.Png.Guid)
                return $"{Name}.png";
            if (Bitmap.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                return $"{Name}.jpeg";
            if (Bitmap.RawFormat.Guid == ImageFormat.Gif.Guid)
                return $"{Name}.gif";
            if (Bitmap.RawFormat.Guid == ImageFormat.Tiff.Guid)
                return $"{Name}.tiff";
            if (Bitmap.RawFormat.Guid == ImageFormat.Wmf.Guid)
                return $"{Name}.wmf";
            if (Bitmap.RawFormat.Guid == ImageFormat.Icon.Guid)
                return $"{Name}.ico";
            if (Bitmap.RawFormat.Guid == ImageFormat.Emf.Guid)
                return $"{Name}.emf";
            if (Bitmap.RawFormat.Guid == ImageFormat.Exif.Guid)
                return $"{Name}.jpg";
            if (Bitmap.RawFormat.Guid == ImageFormat.Bmp.Guid)
                return $"{Name}.bmp";
            throw new Exception("unknown file type.");
        }

        public static async Task SendPhotoAsync(this ISocketMessageChannel This, Bitmap Picture, string FileName, string Message = null, bool IsTTS = false, Embed Embed = null, RequestOptions RO = null, bool IsSpoiller = false)
        {
            MemoryStream BMPMemory = new MemoryStream();
            Picture.Save(BMPMemory, Picture.RawFormat);
            BMPMemory.Position = 0;
            await This.SendFileAsync(BMPMemory, AddFileEnding(Picture, FileName), Message, IsTTS, Embed, RO, IsSpoiller);
        }
    }
}
