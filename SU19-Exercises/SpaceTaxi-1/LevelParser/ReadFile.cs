using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceTaxi_1.LevelParser {
    public class ReadFile {
        private string line;
        private int counter;
        public Dictionary<char, string> dict;
        public List<string> boardList;        
        
        // short-n-sweet.txt OR the-beach.txt (as 'level' input)
        public void Read(string level) {
            StreamReader file = new StreamReader(Path.Combine("Levels", level));
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
    }
}