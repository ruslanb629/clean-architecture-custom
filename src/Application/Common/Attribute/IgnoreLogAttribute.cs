using System.ComponentModel;

namespace Application.Common.Attribute;

[AttributeUsage((AttributeTargets.Method | AttributeTargets.Class 
    | AttributeTargets.Field | AttributeTargets.Property), AllowMultiple = false, Inherited = true)]
public class IgnoreLogAttribute : DisplayNameAttribute
{

}
