using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceTaxi_1.LevelParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpaceTaxi_1.LevelParser;

namespace LevelParserTesting {
    [TestFixture]
    public class Tests {
        private ReadFile readFile;
        
        [Test]
        public void BoardTest1() {
            readFile = new ReadFile();
            readFile.Read("short-n-sweet.txt");
            List<string> shortnsweetlist = new List<string> {
                "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#",
                "#               JW      JW             %",
                "%      h2g                             #",
                "#      222                     >       %",
                "%      H2G                        o    #",
                "#       3                           o  %",
                "%       3                              #",
                "#       3                           o  %",
                "%       3                       j%i    #",
                "#       3                       W Xi   %",
                "%       3                          %   #",
                "#                                 xI   %",
                "%    o                           xI    #",
                "#                               xI     %",
                "%                              xI      #",
                "#  o   o                      xI       %",
                "%                            xI        #",
                "#    o                      xI         %",
                "%      o                   xI          #",
                "#  o                      xI           %",
                "%       o                 I            #",
                "#         m1111111111111n              %",
                "%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#"
            };
            
            for (int i = 0; i < shortnsweetlist.Count; i++) {
                Assert.True(String.Equals(shortnsweetlist[i],readFile.boardList[i]));   
            }
        }

        [Test]
        public void BoardTest2() {
            readFile = new ReadFile();
            readFile.Read("the-beach.txt");
            List<string> thebeachlist = new List<string> {
                "CTTTTTTTTTTTTTTTTD^^^^^^CTTTTTTTTTTTTttt",
                "A                                    stt",
                "A                                      B",
                "A HJJJJJJJJG                           B",
                "HIIIIIIIIIG                            B",
                "A  HIIIG                               B",
                "A                     prrrrrq          B",
                "A                      poooooqbc       B",
                "A                       poooooad       B",
                "A                        poooooq       B",
                "A                         poooooq      B",
                "A                          pooooo      B",
                "A                           aoooo      B",
                "A                           apooo      B",
                "A              >            a poo      B",
                "A                           a  po      B",
                "A ujl                       a   p      B",
                "A  ujl                      a          B",
                "A   ujl                     a          B",
                "A    ujiiiiiiiiiiiiiij      a          B",
                "A     gef         gef       a          B",
                "MMMMMMMeMMMMMMMMMMMeMMMMMMMMaMMMMMMMMMMM",
                "OOOOSSSeSSSSSSSSSSSeSSSSSSSOaOOOOOOOOOOO"
            };

            for (int i = 0; i < thebeachlist.Count; i++) {
                Assert.True(String.Equals(thebeachlist[i],readFile.boardList[i]));
            }
        }

        [Test]
        public void DictionaryTest1() {
            readFile = new ReadFile();
            readFile.Read("the-beach.txt");
            Dictionary<char,string> thebeachDictionary = new Dictionary<char, string>() {
                {'A', "aspargus-edge-left.png"},
                {'B', "aspargus-edge-right.png"},
                {'T', "aspargus-edge-top.png"},
                {'C', "aspargus-edge-top-left.png"},
                {'D', "aspargus-edge-top-right.png"},
                {'G', "white-left-half-circle.png"},
                {'H', "white-right-half-circle.png"},
                {'I', "white-square.png"},
                {'J', "white-square.png"},
                {'O', "olive-green-square.png"},
                {'S', "salt-box-square.png"},
                {'M', "minsk-square.png"},
                {'a', "emperor-square.png"},
                {'b', "emperor-lower-right.png"},
                {'c', "emperor-lower-left.png"},
                {'d', "emperor-upper-left.png"},
                {'e', "deep-bronze-square.png"},
                {'f', "deep-bronze-left-half-circle.png"},
                {'g', "deep-bronze-right-half-circle.png"},
                {'i', "ironstone-square.png"},
                {'j', "ironstone-square.png"},
                {'l', "ironstone-lower-left.png"},
                {'u', "ironstone-lower-left.png"},
                {'o', "studio-square.png"},
                {'p', "studio-upper-right.png"},
                {'q', "studio-lower-left.png"},
                {'r', "studio-square.png"},
                {'t', "tacha-square.png"},
                {'s', "tacha-upper-right.png"}
            };

            foreach (KeyValuePair<char,string> valuePair in thebeachDictionary) {
                Assert.AreSame(valuePair, readFile.dict.Values);
            } 
            
            
        }
    }
}