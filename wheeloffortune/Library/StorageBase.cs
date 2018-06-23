using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using wheeloffortune.Models;

namespace wheeloffortune.Library
{
    public class StorageBase<T> : IStorage<T>
    {
        protected string FileName { get; }

        public StorageBase(string fileName)
        {
            FileName = Path.Combine(Path.GetTempPath(), fileName);
        }

        public void Save(T stuff)
        {
            var text = JsonConvert.SerializeObject(stuff);
            File.WriteAllText(FileName,text);
        }

        public T Load()
        {
            var fileinfo=new FileInfo(FileName);
            if (!fileinfo.Exists)
                return default(T);
            var text = File.ReadAllText(FileName);
            var result = JsonConvert.DeserializeObject<T>(text);
            
            return result;
        }
    }

    
}