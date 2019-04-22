using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1.LevelParser {
    public class PaintBoard {
        private ReadFile readFile;
        private float width = 0.04347826086956521739130434782609f;
        private float height = 0.025f;
        
        public PaintBoard(string level) {
            readFile = new ReadFile();
            readFile.Read(level);

            Images = new EntityContainer<Entity>();
            CreateBoard();
        }

        public EntityContainer<Entity> Images { get; }
        
        // Iterates over boardList & checks if the given key is found in Dictionary dict.
        public void CreateBoard() {
            readFile.boardList.Reverse();
            for (int i = 0; i < readFile.boardList.Count; i++) {
                string currentString = readFile.boardList[i];
                for (int j = 0; j < currentString.Length; j++) {
                    if (readFile.dict.ContainsKey(currentString[j])) {
                        Images.AddStationaryEntity(new Entity(
                            new StationaryShape(new Vec2F(j*height, i*width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images", 
                                readFile.dict[currentString[j]]))));
                    }
                }    
            }
        }
    }
}