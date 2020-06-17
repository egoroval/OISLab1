using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;

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
                        Load(command[1], listDomains, listBlocks);
                        break;
                    case "print":
                        Print(listDomains, listBlocks);
                        break;
                    case "show":
                        int index;
                        if (!int.TryParse(command[1], out index))
                        {
                            Console.WriteLine($"{command[1]} is not a int");
                            continue;
                        }
                        Console.WriteLine($"Block: {index}");
                        Console.WriteLine("in access points:");
                        foreach(var ap in listBlocks[index].InAccessPoints)
                        {
                            Console.WriteLine(ap.TypeStr);
                        }
                        Console.WriteLine("out access points:");
                        foreach(var ap in listBlocks[index].OutAccessPoints)
                        {
                            Console.WriteLine(ap.TypeStr);
                        }
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
        static void Print(List<Domain> domains, List<Block> blocks)
        {
            foreach(var block in blocks)
            {
                foreach(var str in GetChain(block.OutDomain, block, blocks))
                    Console.WriteLine(str);
            }          
        }
        static string[] GetChain(Domain domain, Block block, List<Block> blocks)
        {
            var res = new List<string>();
            res.Add(blocks.IndexOf(block).ToString());
            // foreach(var inBlock in domain.InBlock)
            // {
            //     var str = blocks.IndexOf(block).ToString() + " - " + blocks.IndexOf(inBlock).ToString();
            //     var array = GetChain(inBlock.OutDomain, inBlock, blocks);
            //     res.Add(str);
            //     foreach(var b in array)
            //     {
            //         res.Add(str + " - " + b);
            //     }
            // }

            return res.ToArray();
        }
        static void Load(string path, List<Domain> domains, List<Block> blocks)
        {
            using (var reader = new XmlTextReader(path))
            {
                while (reader.Read())
                {
                    if (reader.Name != "block")
                        continue;
                    var inList = new List<AccessPoint>();
                    var outList = new List<AccessPoint>();
                    while (reader.Read())
                    {
                        if (reader.Name == "in" && reader.NodeType == XmlNodeType.Element)
                            inList.AddRange(GetAccessPoints(reader));
                        else if (reader.Name == "out" && reader.NodeType == XmlNodeType.Element)
                            outList.AddRange(GetAccessPoints(reader));
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "block")
                        {
                            blocks.Add(new Block(inList.ToArray(), outList.ToArray()));
                            inList = new List<AccessPoint>();
                            outList = new List<AccessPoint>();
                        }
                    }
                }
            }
            foreach(var block in blocks)
            {
                foreach(var ap in block.InAccessPoints)
                {
                    var domain = domains.FirstOrDefault(d => d.TypeAp.EqualsAP(ap));
                    if(domain == null)
                    {
                        domain = new Domain(ap);
                        domains.Add(domain);
                    }
                    domain.InBlock.Add(block);
                }
                foreach(var ap in block.OutAccessPoints)
                {
                    var domain = domains.FirstOrDefault(d => d.TypeAp.EqualsAP(ap));
                    if(domain == null)
                    {
                        domain = new Domain(ap);
                        domains.Add(domain);
                    }
                    domain.OutBlock.Add(block);
                    block.OutDomain = domain;
                }
            }
        }

        static AccessPoint[] GetAccessPoints(XmlTextReader reader)
        {
            var aps = new List<AccessPoint>();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                (reader.Name == "in" || reader.Name == "out" || reader.Name == "array"))
                    break;

                switch (reader.Name)
                {
                    case "array":
                        aps.Add(new AccessPoint("array", GetAccessPoints(reader)));
                        break;
                    case "int":
                        aps.Add(new AccessPointInt());
                        break;
                    case "string":
                        aps.Add(new AccessPointString());
                        break;
                    case "bool":
                        aps.Add(new AccessPointBool());
                        break;
                }
            }
            return aps.ToArray();
        }

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
