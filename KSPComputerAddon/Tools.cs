using KSPComputer;
using KSPComputerModule;
using System.IO;
using UnityEngine;

namespace KSPComputerAddon {
    [System.Serializable]
    public abstract class TestA {
        public string abs;
    }
    [System.Serializable]
    public class Test1 {
        public string n;
    }
    [System.Serializable]
    public class Test2 {
        public Test1 t;
    }
    [System.Serializable]
    public class Test3 : Test1 {
        public Test1 t;
    }
    [System.Serializable]
    public class Test4 : TestA {
        public string con;
    }
    static class Tools {
        public static Color FromRGB(int r, int g, int b) {
            return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f);

        }
        public static void Save(this TreeNode obj, string file) {
            string txt = JsonUtility.ToJson(obj);
            using (StreamWriter f = File.CreateText(file)) {
                f.Write(txt);
            }
        }
        public static T Load<T>(string file) {
            using (StreamReader f = File.OpenText(file)) {
                string data = f.ReadToEnd();
                Log.Write("Loading json: " + data);
                T result = JsonUtility.FromJson<T>(data);
                Log.Write("Result: " + result);
                Log.Write("Resuljt: " + JsonUtility.ToJson(result));
                return result;
            }
        }

    }
}

