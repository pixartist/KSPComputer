using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Ionic.Zlib;
using KSPComputer;
namespace KSPComputerModule
{
    public static class ProgramSerializer
    {
        public static FlightProgram Load(string base64, bool compressed)
        {
            base64 = base64.Replace('_', '/');
            byte[] data = Convert.FromBase64String(base64);

            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {
                    
                    using (DeflateStream gz = new DeflateStream(ms, CompressionMode.Decompress))
                    {
                        return (FlightProgram)f.Deserialize(gz);
                    }
                }
                else
                {
                    return (FlightProgram)f.Deserialize(ms);
                }
            }
        }
        public static string Save(FlightProgram program, bool compressed)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                if (compressed)
                {
                    
                    using (DeflateStream gz = new DeflateStream(ms, CompressionMode.Compress))
                    {
                        f.Serialize(gz, program);
                    }
                }
                else
                {
                    f.Serialize(ms, program);
                }
                return Convert.ToBase64String(ms.ToArray()).Replace('/', '_');
            }
        }
    }
}
