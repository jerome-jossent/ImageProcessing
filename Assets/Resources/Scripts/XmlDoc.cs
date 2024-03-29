﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;


    public class XmlDoc
    {
        private readonly Dictionary<string, string> MethodSummaries = new Dictionary<string, string>();

        /// <summary>
        /// Load method information from XML documentation file
        /// </summary>
        public XmlDoc(string xmlFile)
        {
            XDocument doc = XDocument.Load(xmlFile);
            foreach (XElement element in doc.Element("doc").Element("members").Elements())
            {
                string xmlName = element.Attribute("name").Value;
                string xmlSummary = element.Element("summary").Value.Trim();
                MethodSummaries[xmlName] = xmlSummary;
            }
        }

        /// <summary>
        /// Return a pretty description of the full method
        /// </summary>
        /// <returns></returns>
        public static string MethodSignature(MethodInfo mi)
        {
            string name = mi.DeclaringType.FullName + "." + mi.Name;
            if (mi.IsGenericMethod)
                name += "<T>";
            List<string> paramLabels = new List<string>();
            
            

            foreach (var p in mi.GetParameters())
            {
                string paramType = p.ParameterType.Name;
                if (p.ToString().Contains("Nullable"))
                    paramType = p.ToString().Split('[')[1].Split(']')[0];

                // TODO: add more replacements for language shortcuts
                paramType = paramType.Replace("System.", "");
                paramType = paramType switch
                {
                    "String" => "string",
                    "Double" => "double",
                    "Single" => "float",
                    "Byte" => "byte",
                    "Int32" => "int",
                    _ => paramType
                };

                if (p.ToString().Contains("Nullable"))
                    paramType += "?";
                paramLabels.Add($"{paramType} {p.Name}");
            }
            string sig = name + "(" + string.Join(", ", paramLabels) + ")";
            return sig;
        }

        /// <summary>
        /// Return the XML summary for the given method
        /// </summary>
        public string XmlSummary(MethodInfo mi)
        {
            string xmlName = XmlName(mi);
            if (MethodSummaries.ContainsKey(xmlName))
                return MethodSummaries[xmlName];
            else
                return "SUMMARY NOT FOUND!";
        }

        /// <summary>
        /// Get the name used in XML documentation for a MethodInfo found using Reflection.
        /// </summary>
        public static string XmlName(MethodInfo mi)
        {
            // start with the method name
            string name = "M:" + mi.DeclaringType.FullName + "." + mi.Name;

            // determine XML names for each parameter
            ParameterInfo[] parameters = mi.GetParameters();
            string[] parameterNames = new string[parameters.Length];
            int genericParameterIndex = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                Type pt = parameters[i].ParameterType;
                parameterNames[i] = pt.FullName;

                // special formatting for generics
                if (parameterNames[i] is null)
                {
                    parameterNames[i] = $"``{genericParameterIndex++}";
                    parameterNames[i] += pt.IsArray ? "[]" : "";
                }

                // special formatting for nullable generic types
                if (parameterNames[i].StartsWith("System.Nullable"))
                    parameterNames[i] = "System.Nullable{" + pt.GetGenericArguments()[0] + "}";
            }

            // add XML parameters to the method name
            if (parameters.Length > 0)
                name += "(" + string.Join(",", parameterNames) + ")";

            // special replacement for generic return types
            if (mi.IsGenericMethod)
                name = name.Replace("(", $"``{genericParameterIndex}(");

            return name;
        }
    }

