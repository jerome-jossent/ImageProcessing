using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Method_JJ
{
    public System.Reflection.MethodInfo _method;
    public System.Reflection.ParameterInfo _returnParameter;
    public System.Reflection.ParameterInfo[] _parameters;

    string signature = "";
    string signatureFullType = "";
    string json = "";
    internal string _name;

    public override string ToString()
    {
        return signature;
    }

    public Method_JJ(System.Reflection.MethodInfo method)
    {
        _method = method;
        _name = method.Name;
        GetParameters();
        SetInfo();
    }

    void GetParameters()
    {
        _returnParameter = _method.ReturnParameter;
        _parameters = _method.GetParameters();
    }

    void SetInfo()
    {
        //info = JsonUtility.ToJson(new Method_JJ_JSON(this));
        json = Newtonsoft.Json.JsonConvert.SerializeObject(new Method_JJ_JSON(this), Newtonsoft.Json.Formatting.Indented);

        string _cBc_ = "<#FFFFFF>";
        string _cN_ = "<#000000>";
        string _cR_ = "<#FF0000>";
        string _cV_ = "<#00FF00>";
        string _cBl_ = "<#0000FF>";
        string _cBl1_ = "<#2AAAFF>"; // bleu Type
        string _cBl2_ = "<#00FFFF>";// "<#6CA4C6>"; // bleu variable
        string _cJ_ = "<#DCDCAA>";
        string _cT_ = "<#4EC9B0>";

        string args = "";
        foreach (ParameterInfo param in _parameters)
        {
            if (args != "")
                args += _cBc_ + ", ";
            args += _cBl1_ + param.ParameterType.Name + _cBl2_ + " " + param.Name;
        }
        signature = _cBl1_ + _returnParameter.ParameterType.Name + _cBc_ + " " + _cJ_ + _name + _cBc_ + "(" + args + _cBc_ + ")";

        string argsFullType = "";
        foreach (ParameterInfo param in _parameters)
        {
            if (argsFullType != "")
                argsFullType += ", ";
            argsFullType += param.ParameterType.FullName + " " + param.Name;
        }
        signatureFullType = _returnParameter.ParameterType.FullName + " " + _name + "(" + args + ")";        
    }

    class Method_JJ_JSON
    {
        public string name;
        public Parameter_JJ_JSON[] IN;
        public Parameter_JJ_JSON OUT;

        public Method_JJ_JSON(Method_JJ method_JJ)
        {
            name = method_JJ._method.Name;

            List<Parameter_JJ_JSON> ins = new List<Parameter_JJ_JSON>();
            foreach (ParameterInfo parameter in method_JJ._parameters)
                ins.Add(new Parameter_JJ_JSON(parameter));

            IN = ins.ToArray();

            OUT = new Parameter_JJ_JSON(method_JJ._returnParameter);
        }
    }

    public class Parameter_JJ_JSON
    {
        public string name;
        public string type;

        public Parameter_JJ_JSON(ParameterInfo parameter)
        {
            name = parameter.Name;
            type = parameter.ParameterType.FullName;
        }
    }
}