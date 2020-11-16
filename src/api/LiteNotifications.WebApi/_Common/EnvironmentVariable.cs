using System;

namespace LiteNotifications.WebApi
{
    public sealed class EnvironmentVariable : IValue<string>
    {
        private readonly string _name;
        
        public EnvironmentVariable(string name) => _name = name;
        public static implicit operator string(EnvironmentVariable var) => var.Get();
        public string Get() => Environment.GetEnvironmentVariable(_name).Required($"Environment Variable {_name}");
    }
}
