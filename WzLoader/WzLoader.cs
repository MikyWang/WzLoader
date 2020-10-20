using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using WzLib;

namespace WzLoader
{
    class WzLoader
    {
        //private static string FILEPATH = @"D:\文档\WzFile\Base.wz";
        private static string FILEPATH = @"/Volumes/数据/MapleStory/Base.wz";

        private int loadCount = 0;

        private static WzLoader instance = new WzLoader();
        public static WzLoader Instance => instance;

        public Wz_Structure Structure { get; private set; }

        public List<Wz_File> Wz_Files { private set; get; } = new List<Wz_File>();
        public List<Wz_Image> Wz_Images { private set; get; } = new List<Wz_Image>();
        public List<Wz_Png> Wz_Pngs { private set; get; } = new List<Wz_Png>();
        public List<Wz_Sound> Wz_Sounds { private set; get; } = new List<Wz_Sound>();
        public List<Wz_Uol> Wz_Uols { private set; get; } = new List<Wz_Uol>();
        public List<Wz_Vector> wz_Vectors { private set; get; } = new List<Wz_Vector>();
        public Dictionary<Wz_Node, string> Wz_Normals { private set; get; } = new Dictionary<Wz_Node, string>();

        private WzLoader()
        {
            Structure = new Wz_Structure();
            Structure.Load(FILEPATH);
        }

        public void OutputNode(Wz_Node wz_Node)
        {
            loadCount++;
            if (loadCount > 100)
            {
                return;
            }
            var value = wz_Node.Value;
            if (value == null)
            {
                Console.WriteLine($"{wz_Node.Text}");
                foreach (var node in wz_Node.Nodes)
                {
                    OutputNode(node);
                }
                return;
            }
            Console.WriteLine(value.GetType().FullName);
            if (value is Wz_File)
            {
                Wz_Files.Add(value as Wz_File);
                foreach (var node in wz_Node.Nodes)
                {
                    OutputNode(node);
                }
            }
            if (value is Wz_Image)
            {
                Console.WriteLine(wz_Node.Text);
                var wz_Image = wz_Node.Value as Wz_Image;
                Wz_Images.Add(wz_Image);
                wz_Image.TryExtract();
                foreach (var node in wz_Image.Node.Nodes)
                {
                    OutputNode(node);
                }
            }
            else if (value is Wz_Png)
            {
                var png = (Wz_Png)value;
                Wz_Pngs.Add(png);
                Console.WriteLine(wz_Node.Text);
                using (var bmp = png.ExtractPng())
                {
                    //bmp.Save($"/Volumes/数据/test/{wz_Node.Text}.png", ImageFormat.Png);
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
                Wz_Uols.Add(uol);
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine(uol.Uol);
            }
            else if (value is Wz_Vector)
            {
                var vector = (Wz_Vector)value;
                wz_Vectors.Add(vector);
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine($"{vector.X}, {vector.Y}");
            }
            else if (value is Wz_Sound)
            {
                var sound = (Wz_Sound)value;
                Wz_Sounds.Add(sound);
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
                Wz_Normals.Add(wz_Node, value.ToString());
                Console.WriteLine(wz_Node.Text);
                Console.WriteLine(value.ToString());
            }
        }


    }
}
