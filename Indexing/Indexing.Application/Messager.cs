using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Indexing.Application
{
    public static class Messager
    {
        public static object Send(NetworkStream stream, object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.Unicode.GetBytes(json);
            var sizeBytes = BitConverter.GetBytes(bytes.Length);
            stream.Write(sizeBytes.Concat(bytes).ToArray());
            return JsonConvert.DeserializeObject(json);
        }

        public static object Recive(NetworkStream stream)
        {
            byte[] sizeBytes = new byte[4];
            stream.Read(sizeBytes);
            var size = BitConverter.ToInt32(sizeBytes);
            byte[] data = new byte[size];
            stream.Read(data);
            var json = Encoding.Unicode.GetString(data);
            return JsonConvert.DeserializeObject(json);
        }
    }
}
