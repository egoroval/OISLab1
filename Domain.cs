using System.Collections.Generic;

namespace OISLab1
{
    internal class Domain
    {
        public AccessPoint TypeAp{ get; }
        public Domain(AccessPoint typeAP)
        {
            TypeAp = typeAP;

            InBlock = new List<Block>();
            OutBlock = new List<Block>();
        }
        public List<Block> InBlock { get; }
        public List<Block> OutBlock {get;}
    }
}