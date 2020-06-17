using System.Collections.Generic;

namespace OISLab1
{
    internal class Domain
    {
        private string TypeAp{ get; }
        public Domain(string typeAP)
        {
            TypeAp = typeAP;

            InBlock = new List<Block>();
            OutBlock = new List<Block>();
        }
        public List<Block> InBlock { get; }
        public List<Block> OutBlock {get;}

        public bool EqualsDomain(Domain domain)
        {
            return EqualsDomain(domain.TypeAp);
        }
        public bool EqualsDomain(string typeAp)
        {
            return TypeAp == typeAp;
        }
    }
}