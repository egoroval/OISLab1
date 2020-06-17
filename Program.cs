using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace OISLab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var listDomains = new List<Domain>();
            var listBlocks = new List<Block>();
            string[] command;
            while (true)
            {
                command = Console.ReadLine().Split(' ');
                switch (command[0])
                {
                    case "q":
                        return;
                    case "gen":
                        int count;
                        if (!int.TryParse(command[1], out count))
                        {
                            Console.WriteLine($"{command[1]} is not a int");
                            continue;
                        }
                        if (command.Length > 2)
                            RandomGenerator(count, command[2]);
                        else
                            RandomGenerator(count);
                        break;
                    case "load":

                        break;
                    case "reset":
                        listDomains = new List<Domain>();
                        listBlocks = new List<Block>();
                        break;
                    default:
                        Console.WriteLine("unknown command");
                        break;

                }
            }
        }

        // static void Load(string path, List<Domain> domains, List<Block> blocks)
        // {
        //     using (var reader = new XmlTextReader(path))
        //     {
        //         while(reader.Read())
        //         {
        //             if(reader.Name != "block")
        //                 continue;
        //             while(reader.Read())
        //             {
        //                 if()
        //             }
        //         }
        //     }
        // }
        
        // static AccessPoint[] GetAccessPoints(XmlTextReader reader)
        // {
            
        // }

        static void RandomGenerator(int count, string outPath = "temp.xml")
        {
            var random = new Random();
            var xml = new StringBuilder("<root>");

            for (var i = 0; i < count; i++)
            {
                xml.Append("<block>");

                xml.Append("<in>");
                GenerateAp(random.Next(1, 4), random, xml);
                xml.Append("</in>");

                xml.Append("<out>");
                GenerateAp(1, random, xml);
                xml.Append("</out>");

                xml.Append("</block>");
            }

            xml.Append("</root>");
            using (var writer = new StreamWriter(outPath, false))
            {
                writer.Write(xml);
            }
        }

        static void GenerateAp(int count, Random random, StringBuilder xml)
        {
            for (var i = 0; i < count; i++)
            {
                switch (random.Next(4))
                {
                    case 0:
                        xml.Append("<array>");

                        GenerateAp(random.Next(1, 3), random, xml);

                        xml.Append("</array>");
                        break;
                    case 1:
                        xml.Append("<int />");
                        break;
                    case 2:
                        xml.Append("<string />");
                        break;
                    case 3:
                        xml.Append("<bool />");
                        break;
                }
            }
        }

    }
}
