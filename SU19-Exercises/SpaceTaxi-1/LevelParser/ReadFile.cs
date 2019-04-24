using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceTaxi_1.LevelParser {
    public class ReadFile {
        private string line;
        private int counter;
        public Dictionary<char, string> dict;
        public List<string> boardList;        
        
        public void Read(string level) {
            StreamReader file = new StreamReader(GetLevelPath(level));
            boardList = new List<string>();
            dict = new Dictionary<char, string>();           
            
            // Reads and adds the line to lists.            
            while ((line = file.ReadLine()) != null) {
                counter++;
                if (line.EndsWith(".png")) {
                    dict.Add(line[0],line.Substring(3));
                }
                if (counter < 24) {
                    boardList.Add(line);
                }
            }
            file.Close();
        }

        private string GetLevelPath(string fileName) {
            DirectoryInfo dir = new DirectoryInfo(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            while (dir.Name != "bin") {
                dir = dir.Parent;
            }

            dir = dir.Parent;

            string path = Path.Combine(dir.FullName.ToString(), "Levels", fileName);

            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }

            return path;
        }
    }
}