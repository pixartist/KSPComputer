using KSPComputer;
using KSPComputer.Nodes;
using KSPComputerAddon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPComputerModule {
    [Serializable]
    public abstract class TreeNode {
        [NonSerialized]
        public CategoryModel parent;
    }


    [Serializable]
    public class CategoryModel : TreeNode {
        public string name;
        public int r;
        public int g;
        public int b;

        public override string ToString() {
            return "Category " + name + "(" + r + "," + g + "," + b + ")";
        }
        [NonSerialized]
        public List<TreeNode> children = new List<TreeNode>();
    }

    [Serializable]
    public class NodeModel : TreeNode {
        public string name;
        public string description;
        public int width;

        public override string ToString() {
            return "Node " + name + " [" + description + "], width: " + width;
        }
        [NonSerialized]
        public string className;
    }

    public class NodeCategories {
        private static NodeCategories instance;
        public static NodeCategories Instance {
            get {
                if (instance == null)
                    instance = new NodeCategories();
                return instance;
            }
        }
        public const string catName = "category";
        public const string nodeName = "node";
        public const string fileType = ".json";
        public bool TopCategory {
            get {
                return SelectedCategory.parent == null;
            }
        }
        private Dictionary<string, Type> nodeTypes;
        private Dictionary<string, NodeInfo> nodeInfos;
        private CategoryModel root;
        public CategoryModel SelectedCategory { get; private set; }
        private NodeCategories() {
            string path = Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, "GameData"), "FlightComputer"), "Nodes");
            Log.Write("Loading node categories: " + path);
            GetNodeTypes(path);
            root = new CategoryModel();
            root.parent = null;
            SelectedCategory = root;
            var rootCats = Directory.GetDirectories(path);
            foreach (var d in rootCats) {
                Log.Write("Found root category: " + d);
                AddCategory(root, d);
            }
            //root.Save("test.json");
        }
        public Type GetType(string className) {
            nodeTypes.TryGetValue(className, out Type t);
            return t;
        }
        public void CategoryUp() {
            if (!TopCategory)
                SelectedCategory = SelectedCategory.parent;
        }
        public void SelectSubCategory(string cat) {
            var cats = from c in SelectedCategory.children where (c is CategoryModel && (c as CategoryModel).name == cat) select (c as CategoryModel);
            if (cats.Count() > 0)
                SelectedCategory = cats.First();
        }
        public CatInfo[] ListSubCategories() {
            return (from c in SelectedCategory.children where c is CategoryModel select new CatInfo(c as CategoryModel)).ToArray<CatInfo>();
        }

        public NodeInfo[] ListNodes() {
            return (from c in SelectedCategory.children where (c is NodeModel && (c as NodeModel).className != null ? nodeInfos.ContainsKey((c as NodeModel).className) : false) select nodeInfos[(c as NodeModel).className]).ToArray();

        }
        public NodeInfo GetNodeInfo(Type t) {
            return GetNodeInfo(t.ToString());
        }
        public NodeInfo GetNodeInfo(string className) {
            NodeInfo i;
            if (!nodeInfos.TryGetValue(className, out i)) {
                Log.Write("FAILED TO FIND REQUESTED NODE INFO: " + className);
            }
            return i;
        }

        private void GetNodeTypes(string path) {
            nodeInfos = new Dictionary<string, NodeInfo>();
            nodeTypes = new Dictionary<string, Type>();
            var types = FindAllDerivedTypes(typeof(Node));
            foreach (var t in types) {
                nodeTypes.Add(t.ToString(), t);
                nodeInfos.Add(t.ToString(), new NodeInfo());
            }
            /* JObject temp = new JObject();
             JObject typeNode = new JObject();
             temp.Add("types", typeNode);
             foreach (var t in types)
                 typeNode.Add(t.ToString());
             temp.Save("types.json");*/
        }
        private void AddCategory(CategoryModel parent, string path) {
            string catPath = Path.Combine(path, catName + fileType);
            //Log.Write("Looking for " + catPath);
            if (File.Exists(catPath)) {
                CategoryModel cat = Tools.Load<CategoryModel>(catPath);
                cat.parent = parent;
                parent.children.Add(cat);
                //Log.Write("Loaded category " + cat);
                var files = Directory.GetFiles(path, "*" + fileType);
                string fName;
                foreach (var f in files) {
                    fName = Path.GetFileNameWithoutExtension(f);
                    if (fName != catName) {
                        if (nodeTypes.ContainsKey(fName)) {
                            var el = Tools.Load<NodeModel>(f);
                            cat.children.Add(el);
                            nodeInfos[fName] = new NodeInfo(
                                el.name,
                                GetType(fName),
                                el.description,
                                Tools.FromRGB(cat.r, cat.g, cat.b),
                                el.width
                                );
                            el.className = fName;
                            Log.Write("Loaded node type " + fName + ", color: " + nodeInfos[fName].color);
                        } else {
                            Log.Write("Node class has definition file but does not exist in code: " + fName);
                        }
                    }
                }
                var dirs = Directory.GetDirectories(path);
                foreach (var d in dirs) {
                    AddCategory(cat, d);
                }
            }
        }

        public static List<Type> FindAllDerivedTypes(Type baseType) {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies) {
                try {
                    //Log.Write("Looking for " + baseType + " in " + assembly);
                    var aTypes = assembly
                     .GetTypes()
                     .Where(t =>
                         baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType
                         );
                    types.AddRange(aTypes);
                } catch (Exception e) {
                    Log.Write("Error, could not load types from assembly \"" + assembly + "\": " + e.Message);
                }
            }
            return types;
        }
    }
}
