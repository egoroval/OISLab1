namespace OISLab1
{
    internal class Block
    {
        public AccessPoint[] InAccessPoints { get; }
        public AccessPoint[] OutAccessPoints { get; }

        public Domain OutDomain {get; set;}

        public Block(AccessPoint[] inAccessPoints, AccessPoint[] outAccessPoints)
        {
            InAccessPoints = inAccessPoints;
            OutAccessPoints = outAccessPoints;
        }
    }
}