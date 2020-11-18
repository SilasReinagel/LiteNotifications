using System.Collections.Generic;

namespace LiteNotifications.WebApi.Domain
{
    public sealed class GroupOutletData
    {
        public string Id { get; set; }
        public string OutletType { get; set; }
        public string OutletGroup { get; set; }
        public string Target { get; set; }

        public override bool Equals(object obj)
        {
            var outlet = obj as GroupOutletData;
            if (outlet == null) return false;
            return OutletType == outlet.OutletType
                && OutletGroup == outlet.OutletGroup
                && Target == outlet.Target;
        }
        public override int GetHashCode()
        {
            return (OutletType + OutletGroup + Target).GetHashCode();
        }
    }
    
    public sealed class GroupOutlets : DictionaryWithDefault<string, List<GroupOutletData>> 
    {
        public GroupOutlets() : base(new List<GroupOutletData>()) { }
    }
}
