using System.Linq;

namespace OISLab1
{
    internal interface IAccessPoint
    {
        string TypeStr { get; }
        bool EqualsAP(AccessPoint ap);
    }

    internal class AccessPoint : IAccessPoint
    {
        public string TypeStr { get; set; }
        public AccessPoint(string type = "array", params AccessPoint[] points)
        {
            TypeStr = type;
            Include = points ?? new[] {this};
        }
        public AccessPoint[] Include { get; }
        
        public virtual bool EqualsAP(AccessPoint ap)
        {
            if(TypeStr == ap.TypeStr)
            {
                if(TypeStr == "array")
                {
                    var listAp = ap.Include.ToArray();
                    foreach(var accessPoint in Include)
                    {
                        var tmp = listAp.FirstOrDefault(a => a.EqualsAP(accessPoint));
                        if(tmp == null)
                        return false;
                    }
                }
                return true;
            }


            return false;
        }
    }
    internal class AccessPointInt : AccessPoint
    {
        public AccessPointInt() : base("int", null)
        {
        }
    }

    internal class AccessPointString : AccessPoint
    {
        public AccessPointString() : base("string", null)
        {
        }
    }

    internal class AccessPointBool : AccessPoint
    {
        public AccessPointBool() : base("bool", null)
        {
        }
    }

    internal class AccessPointObject : AccessPoint
    {
        private IAccessPoint Object { get; }
        public AccessPointObject(IAccessPoint obj) : base(obj.TypeStr)
        {
            Object = obj;
        }

        public override bool EqualsAP(AccessPoint ap)
        {
            return Object.EqualsAP(ap);
        }
    }
}