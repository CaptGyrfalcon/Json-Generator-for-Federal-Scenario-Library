using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ScenarioGenerator
{
    
    [DataContract(Name="Scenarios")]
    public class ScenarioList
    {
        [DataMember]
        public List<Scenario> Scenarios { get; set; }
    }
    
    
    
    [DataContract]
    public class Scenario
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Dictionary<string, string> names { get; set; }
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public difficultyStruct difficulty { get; set; }
        [DataMember]
        public int constant { get; set; }
        [DataMember]
        public List<scoreCondition> unlockCondition { get; set; }
        [DataMember]
        public Dictionary<string, languageAllowed> languageAllowed { get; set; }
        [DataMember]
        public bool preload { get; set; }
        [DataMember]
        public string pinyin { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public string groupName { get; set; }
        [DataMember]
        public string groupDisplayName { get; set; }
        [DataMember]
        public int unlockPotential { get; set; }
        [DataMember]
        public double scoreMultiplier { get; set; }
        [DataMember]
        public string authorDisplayed { get; set; }
        [DataMember]
        public releaseDate releaseDate { get; set; }
        
    }
    [DataContract]
    public class difficultyStruct
    {
        [DataMember]
        public int rating{ get; set; }
        [DataMember]
        public bool plus{ get; set; }
    }
    [DataContract]
    public class languageAllowed
    {
        [DataMember]
        public bool available{ get; set; }
        [DataMember]
        public string fileName{ get; set; }
        public languageAllowed(bool a, string f)
        {
            available = a;
            fileName = f;
        
        }
    }
    [DataContract]
    public class scoreCondition
    {
        [DataMember]
        public int id{ get; set; }
        [DataMember]
        public string scenarioName{ get; set; }
        [DataMember]
        public int score{ get; set; }
        public scoreCondition(int i, string name, int sc)
        {
            id = i;
            scenarioName = name;
            score = sc;
        }
    }
    [DataContract]
    public class releaseDate
    {
        [DataMember]
        public int year{ get; set; }
        [DataMember]
        public int month{ get; set; }
        [DataMember]
        public int day{ get; set; }
        public releaseDate(int y, int m, int d)
        {
            year = y;
            month = m;
            day = d;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> scenarioPair = new Dictionary<string, int>();
            List<Scenario> scenarioList = new List<Scenario>();

            // 生成Scenario对象...

            // 创建ScenarioList对象
            ScenarioList scenarioListWrapper = new ScenarioList
            {
                Scenarios = scenarioList
            };
            
            
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            desktop = Path.Combine(desktop, "ScenarioConfig");
            string scenListPath = Path.Combine(desktop, "scenList.csv");
            string unlockPath = Path.Combine(desktop, "unlockOrder.txt");
            string[] scenList = File.ReadAllLines(scenListPath);
            string[] unlockList = File.ReadAllLines(unlockPath);
            
            
            int scenarioCount = scenList.Length - 1;
          

            // Generate scenarios with empty field values
            for (int i = 1; i <= scenarioCount; i++)
            {
                Scenario scenario = new Scenario
                {
                    id = i,
                    names = new Dictionary<string, string>
                    {
                        {"cn", ""},
                        {"en", ""},
                        {"ru", ""},
                        {"jp", ""}
                    },
                    languageAllowed = new Dictionary<string, languageAllowed>
                    {
                        {"cn", new languageAllowed(true, "")},
                        {"en", new languageAllowed(false, "")},
                        {"ru", new languageAllowed(false, "")},
                        {"jp", new languageAllowed(false, "")}
                    },
                    author = "",
                    type = "",
                  
                    difficulty = new difficultyStruct
                    {
                        rating = 0,
                        plus = false
                    },
                    constant = 0,
                    unlockCondition = new List<scoreCondition>(),
                    preload = false,
                    pinyin = "",
                    authorDisplayed = "",
                    fileName = "",
                    groupName = "",
                    groupDisplayName = "Single",
                    releaseDate = new releaseDate(1970, 1, 1)
                };

                scenarioList.Add(scenario);
            }
            
            
            
            for(int i = 0; i < scenarioCount; i++)
            {
                int k = i + 1;
                string[] properties = scenList[k].Split(',');
                Scenario scen = scenarioList[i];
                scen.id = k;
                if(scenarioPair.ContainsKey(properties[7]))
                {
                    Console.WriteLine(properties[7] + " is already here!");
                
                }
                else
                {
                    scenarioPair.Add(properties[7], k);
                }
                scen.names["cn"] = properties[1];
                scen.author = properties[6];
                scen.type = properties[3];
                scen.difficulty.rating = int.Parse(properties[4].Replace("+", ""));
                scen.difficulty.plus = properties[4].Contains("+") ? true : false;
                Console.WriteLine(properties[5]);
                scen.constant = (int)Math.Round(double.Parse(properties[5]) * 10.0);
                scen.languageAllowed["cn"].available = true;
                scen.languageAllowed["cn"].fileName = properties[7];
                scen.preload = properties[8].Equals("TRUE") ? true : false;
                scen.pinyin = properties[12];
                scen.fileName = properties[7];
                scen.groupName = properties[9];
                scen.groupDisplayName = properties[14];
                scen.unlockPotential = int.Parse(properties[13]);
                string multiplier = properties[2];
                if(multiplier.ToUpper().Equals("SPECIAL"))
                {
                    scen.scoreMultiplier = -1.0;
                }
                else
                {
                    scen.scoreMultiplier = double.Parse(properties[2]);
                }
                
                scen.authorDisplayed = properties[11];
                string dateString = properties[10];
                string[] dates = dateString.Split('/');
                scen.releaseDate.year = int.Parse(dates[0]);
                scen.releaseDate.month = int.Parse(dates[1]);
                scen.releaseDate.day = int.Parse(dates[2]);
                
                if(properties[15].Equals("FALSE"))
                {
                    scen.fileName = "";
                }
                
                
                
            }
            
            foreach(string line in unlockList)
            {
                string[] unlocks = line.Split(',');
                if(unlocks.Length >= 3)
                {
                    int unlockCount = unlocks.Length / 2;
                    Console.WriteLine(unlocks[0]);
                    int mainID = scenarioPair[unlocks[0]];
                    for(int i = 0; i < unlockCount; i++)
                    {
                        int secondID = scenarioPair[unlocks[2*i+1]];
                        int secondScore = int.Parse(unlocks[2*i+2]);
                        scenarioList[mainID-1].unlockCondition.Add(new scoreCondition(secondID, scenarioList[secondID-1].names["cn"], secondScore));
                    }
                }
            }
            
            string newContent = "";
            for(int i = 0 ; i < scenarioCount; i++)
            {    
                int k = i + 1;
                string[] properties = scenList[k].Split(',');
                if((!string.IsNullOrEmpty(properties[0]) || properties[1].Contains("巫尽晨昏")) && !properties[15].Equals("FALSE"))
                {
                    newContent += properties[0] + "," + properties[1] + "," + properties[2] + "," + properties[3] + "," + properties[4] + "," + double.Parse(properties[5]).ToString("N1") + "," + properties[6] + "\n";
                }
                
                
                string idText = "[场景属性]\n名称=" + properties[1] + "\n等级=" + properties[3] + " " + (properties[15].Equals("FALSE") ? "-1" : properties[4]) + "\n定数=" + (properties[15].Equals("FALSE") ? "-10.0" : double.Parse(properties[5]).ToString("N1")) + "\n类别=病理场景\n作者=" + properties[6] + "\n日期=" + properties[10] + "\n倍率=" + properties[2];
                string idPath = Path.Combine(desktop, "场景库", k.ToString() + ".ini");
                if(!string.IsNullOrEmpty(properties[0]) || properties[1].Contains("巫尽晨昏"))
                {
                    File.WriteAllBytes(idPath, Encoding.GetEncoding("GB2312").GetBytes(idText));
                }
                
                    
            }
            string newListPath = Path.Combine(desktop, "场景库", "scenList.txt");
            File.WriteAllText(newListPath, newContent);
            
            
            
            
            // Create DataContractJsonSerializer
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ScenarioList));

            // Create a MemoryStream to write the JSON data
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Serialize the scenarioList to JSON
                jsonSerializer.WriteObject(memoryStream, scenarioListWrapper);

                // Convert the MemoryStream to a byte array
                byte[] jsonData = memoryStream.ToArray();

                // Convert the byte array to a string
                string jsonString = System.Text.Encoding.UTF8.GetString(jsonData);

                // Specify the path to save the json file on your desktop
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "scenarioList.json");
                
                // Save json to file
                File.WriteAllText(filePath, jsonString);
                
                filePath = Path.Combine(desktopPath, "scenarioList.txt");
                File.WriteAllText(filePath, jsonString);
                
                
                Console.WriteLine("Scenario list json file generated: " + filePath);
            }
            
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
