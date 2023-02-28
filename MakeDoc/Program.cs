using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Xml.Linq;
namespace MakeDoc
{
    class Program
    {
        public const string catName = "category";
        public const string nodeName = "node";
        public const string fileType = ".xml";
        static void Main(string[] args)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Nodes");
            var root = new XElement("Categories");
            var rootCats = Directory.GetDirectories(path);
            foreach (var d in rootCats)
            {
                AddCategory(root, d);
            }
            using(FileStream fs = new FileStream("Docs.txt", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                var cats = root.Elements("category");
                foreach(var c in cats)
                    WriteNode(c, sw, "");
                sw.Flush();
            }
        }
        private static void WriteNode(XElement node, StreamWriter writer, string prefix)
        {
            var subcats = node.Elements("category");
            var nodes = node.Elements("node");
            writer.WriteLine(prefix + "* **" + node.Element("name").Value + "**");
            writer.WriteLine("  ");
            foreach(var c in subcats)
            {
                WriteNode(c, writer,  "    " + prefix);
            }
            foreach(var n in nodes)
            {
                string na = n.Element("name").Value;
                writer.WriteLine(prefix + "    -" + n.Element("name").Value + "  ");
                writer.WriteLine("     " + "_" + n.Element("description").Value + "_  ");
                writer.WriteLine("  ");
            }
        }
        private static void AddCategory(XElement parent, string path)
        {

            string catPath = Path.Combine(path, catName + fileType);
            //Log.Write("Looking for " + catPath);
            if (File.Exists(catPath))
            {
                XElement cat = XElement.Load(catPath);
                //Log.Write("Loaded category " + cat);
                var files = Directory.GetFiles(path, "*" + fileType);
                string fName;
                foreach (var f in files)
                {
                    fName = Path.GetFileNameWithoutExtension(f);
                    if (fName != catName)
                    {
                        cat.Add(XElement.Load(f));
                    }
                }
                
                var dirs = Directory.GetDirectories(path);
                foreach (var d in dirs)
                {
                    AddCategory(cat, d);
                }
                parent.Add(cat);
            }
        }
    }
}
