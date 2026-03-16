using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QM_CloneSort
{
    internal static class ManualOrderStore
    {
        private static string FilePath => Path.GetFullPath(
            Path.Combine(Application.persistentDataPath, "../Quasimorph_ModConfigs/QM_CloneSort/manual_order.txt"));

        public static List<string> Load()
        {
            try
            {
                string path = FilePath;
                if (!File.Exists(path))
                    return null;

                string[] lines = File.ReadAllLines(path);
                var list = new List<string>(lines.Length);
                foreach (string line in lines)
                    if (!string.IsNullOrEmpty(line))
                        list.Add(line);
                return list.Count > 0 ? list : null;
            }
            catch
            {
                return null;
            }
        }

        public static void Save(IList<string> profileIds)
        {
            try
            {
                string path = FilePath;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllLines(path, profileIds);
            }
            catch { }
        }
    }
}
