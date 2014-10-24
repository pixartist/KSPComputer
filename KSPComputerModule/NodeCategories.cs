using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using KSPComputer.Nodes;
using DefaultNodes;
namespace KSPComputerModule
{
    public class NodeCategories
    {
        public struct NodeInfo
        {
            public string name;
            public Type type;
            public string description;
            public Color color;
            public float width;
            public NodeInfo(string name, Type type, string description, Color color, float width)
            {
                this.name = name;
                this.type = type;
                this.description = description;
                this.color = color;
                this.width = width;
            }
            public NodeInfo(XElement element, Type type)
            {
                this.name = "Unnamed";
                this.description = "No description";
                this.color = Color.white;
                this.width = 100f;
                this.type = type;
                if (element.Element("name") != null)
                    this.name = element.Element("name").Value;
                if (element.Element("description") != null)
                    this.description = element.Element("description").Value;
                if(element.Parent != null)
                {
                    var p = element.Parent;
                    if (p.Element("r") != null && p.Element("g") != null && p.Element("b") != null)
                    {
                        int r = 0, g = 0, b = 0;
                        int.TryParse(p.Element("r").Value, out r);
                        int.TryParse(p.Element("g").Value, out g);
                        int.TryParse(p.Element("b").Value, out b);
                        this.color = new Color(r / 255f, g / 255f, b / 255f);
                    }
                }
                if(element.Element("width") != null)
                {
                    //Log.Write("FOUND WIDTH: " + element.Element("width").Value);
                    float.TryParse(element.Element("width").Value, out this.width);
                    if (this.width <= 0)
                    {
                        int w = 0;
                        int.TryParse(element.Element("width").Value, out w);
                        if (w > 0)
                            this.width = (float)w;
                    }
                   // Log.Write("PARSED WIDTH: " + this.width);
                }
                
            }
            
        }
        public struct CatInfo
        {
            public string name;
            public Color color;
            public CatInfo(XElement element)
            {
                this.name = "Unnamed category";
                this.color = Color.white;
                if (element.Element("name") != null)
                    this.name = element.Element("name").Value;
                if (element.Element("r") != null && element.Element("g") != null && element.Element("b") != null)
                {
                    int r = 0, g = 0, b = 0;
                    int.TryParse(element.Element("r").Value, out r);
                    int.TryParse(element.Element("g").Value, out g);
                    int.TryParse(element.Element("b").Value, out b);
                    this.color = new Color(r / 255f, g / 255f, b / 255f);
                }
            }

        }
        public const string catName = "category";
        public const string nodeName = "node";
        public const string fileType = ".xml";
        private Dictionary<string, Type> nodeTypes;
        private Dictionary<string, NodeInfo> nodeInfos;
        private XElement root;
        public XElement SelectedCategory { get; private set; }
        public NodeCategories(string path)
        {
            Log.Write("Loading node categories: " + path);
            GetNodeTypes(path);
            root = new XElement("Categories");
            SelectedCategory = root;
            var rootCats = Directory.GetDirectories(path);
            foreach(var d in rootCats)
            {
                Log.Write("Found root category: " + d);
                AddCategory(root, d);
            }
            root.Save("test.xml");
        }
        public Type GetType(string className)
        {
            Type t = null;
            nodeTypes.TryGetValue(className, out t);
            return t;
        }
        public void CategoryUp()
        {
            if (SelectedCategory.Parent != null)
                SelectedCategory = SelectedCategory.Parent;
        }
        public void SelectSubCategory(string cat)
        {
            var cats = from c in SelectedCategory.Elements(catName) where (c.Element("name") != null ? c.Element("name").Value == cat : false) select c;
            if (cats.Count() > 0)
                SelectedCategory = cats.First();
        }
        public CatInfo[] ListSubCategories()
        {
            return (from c in SelectedCategory.Elements(catName) select new CatInfo(c)).ToArray<CatInfo>();
        }
        
        public NodeInfo[] ListNodes()
        {
            return (from c in SelectedCategory.Elements(nodeName) where (c.Element("className") != null ? nodeInfos.ContainsKey(c.Element("className").Value) : false) select nodeInfos[c.Element("className").Value]).ToArray();

        }
        public NodeInfo GetNodeInfo(Type t)
        {
            return GetNodeInfo(t.ToString());
        }
        public NodeInfo GetNodeInfo(string className)
        {
            NodeInfo i;
            if (!nodeInfos.TryGetValue(className, out i))
            {
                Log.Write("FAILED TO FIND REQUESTED NODE INFO: " + className);
            }
            return i;
        }

        private void GetNodeTypes(string path)
        {

            nodeInfos = new Dictionary<string, NodeInfo>();
            nodeTypes = new Dictionary<string,Type>();
            var types = FindAllDerivedTypes(typeof(Node));
            foreach (var t in types)
            {
                nodeTypes.Add(t.ToString(), t);
                nodeInfos.Add(t.ToString(), new NodeInfo());
            }
            XElement temp = new XElement("types");
            foreach (var t in types)
                temp.Add(new XElement(t.ToString()));
            temp.Save("types.xml");
        }
        private void AddCategory(XElement parent, string path)
        {

            string catPath = Path.Combine(path,catName + fileType);
            //Log.Write("Looking for " + catPath);
            if (File.Exists(catPath))
            {
                XElement cat = XElement.Load(catPath);
                //Log.Write("Loaded category " + cat);
                var files = Directory.GetFiles(path, "*"+ fileType);
                string fName;
                foreach(var f in files)
                {
                    fName = Path.GetFileNameWithoutExtension(f);
                    if (fName != catName)
                    {
                       /* Type t = Type.GetType(fName);
                        if (t != null)
                        {
                            var el = XElement.Load(f);
                            cat.Add(el);
                            nodeInfos[fName] = new NodeInfo(el, t);
                            el.Add(new XElement("className", fName));
                            Log.Write("Loaded type " + fName);
                        }
                        else
                        {
                            Log.Write("Could not find node type: " + fName);
                        }*/
                       if (nodeTypes.ContainsKey(fName))
                        {
                            var el = XElement.Load(f);
                            cat.Add(el);
                            nodeInfos[fName] = new NodeInfo(el, GetType(fName));
                            el.Add(new XElement("className", fName));
                            Log.Write("Loaded node type " + fName);
                        }
                        else
                        {
                            Log.Write("Node class has definition file but does not exist in code: " + fName);
                        }
                    }
                }
                var dirs = Directory.GetDirectories(path);
                foreach(var d in dirs)
                {
                    AddCategory(cat, d);
                }
                parent.Add(cat);
            }
        }
        private XElement Get(string path, string filer, string value)
        {
            string[] sub = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            XElement at = root;
            foreach(var s in sub)
            {
                at = at.Element(s);
                if (at == null)
                    break; ;
            }
            return at;
        }

        public static List<Type> FindAllDerivedTypes(Type baseType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                //Log.Write("Looking for " + baseType + " in " + assembly);
                types.AddRange(assembly
                    .GetTypes()
                    .Where(t =>
                        baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType
                        ));
            }
            return types;
        }
    }
}
