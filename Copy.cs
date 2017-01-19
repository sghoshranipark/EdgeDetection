using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Edge_Detection
{
    class Copy
    {
        //This Class is Intended to perform a Deep Copy of Objects
        public T DeepCopy<T>(T other)
        {
            //Generic Function that performs Deepcopy
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            };
        }
    }
}
