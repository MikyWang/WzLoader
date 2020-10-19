using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WzLib;

namespace WzLoader
{
    class WzLoader
    {
        private static WzLoader instance = new WzLoader();
        public static WzLoader Instance => instance;
        public Wz_Structure Structure { get; private set; }
        private WzLoader()
        {
            Structure = new Wz_Structure();
            Structure.Load(@"D:\文档\WzFile\Base.wz");
        }

        public void OutputNode(Wz_Node wz_Node)
        {
            var value = wz_Node.Value;
            if (value == null)
            {
                Console.WriteLine($"无值：{wz_Node.Text}");
                foreach (var node in wz_Node.Nodes)
                {
                    OutputNode(node);
                }
                return;
            }
            Console.WriteLine(value.GetType().FullName);
            if (value is Wz_File)
            {
                foreach (var node in wz_Node.Nodes)
                {
                    OutputNode(node);
                }
            }
            if (value is Wz_Image)
            {
                Console.WriteLine(wz_Node.Text);
                var wz_Image = wz_Node.Value as Wz_Image;
                wz_Image.TryExtract();
                foreach (var node in wz_Image.Node.Nodes)
                {
                    OutputNode(node);
                }
            }
            else if (value is Wz_Png)
            {
                var png = (Wz_Png)value;
                Console.WriteLine(wz_Node.Text);
                using (var bmp = png.ExtractPng())
                {
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] data = ms.ToArray();
                        Console.WriteLine(Convert.ToBase64String(data));
                    }
                }
            }
            else if (value is Wz_Uol)
            {
                var uol = (Wz_Uol)value;
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine(uol.Uol);
            }
            else if (value is Wz_Vector)
            {
                var vector = (Wz_Vector)value;
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine($"{vector.X}, {vector.Y}");
            }
            else if (value is Wz_Sound)
            {
                var sound = (Wz_Sound)value;
                Console.WriteLine(wz_Node.Text);
                byte[] data = sound.ExtractSound();
                if (data == null)
                {
                    data = new byte[sound.DataLength];
                    sound.WzFile.FileStream.Seek(sound.Offset, SeekOrigin.Begin);
                    sound.WzFile.FileStream.Read(data, 0, sound.DataLength);
                }
                Console.WriteLine(Convert.ToBase64String(data));
            }
            else
            {
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine(value.ToString());
            }
        }


    }
}
