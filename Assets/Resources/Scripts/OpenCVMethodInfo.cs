using OpenCVForUnity.CoreModule;
using OpenCVForUnityExample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpenCVMethodInfo : MonoBehaviour
{
    Type CV = typeof(OpenCVForUnity.ImgprocModule.Imgproc);

    Dictionary<string, List<Method_JJ>> methods_JJ;
    Dictionary<string, List<string>> constantes;

    [SerializeField] TMPro.TMP_Dropdown dd_methods;
    [SerializeField] TMPro.TMP_Dropdown dd_methods_surcharges;

    [SerializeField] TMPro.TMP_Dropdown dd_constantes_cat;
    [SerializeField] TMPro.TMP_Dropdown dd_constantes;


    void Start()
    {
        OpenCVMethods();
        OpenCVConstantes();
    }



    void OpenCVMethods()
    {
        //création du dictionnaire de méthodes de la classe Imgproc
        System.Reflection.MethodInfo[] methods = CV.GetMethods();

        methods_JJ = new Dictionary<string, List<Method_JJ>>();

        foreach (System.Reflection.MethodInfo method in methods)
        {
            Method_JJ method_JJ = new Method_JJ(method);

            if (!methods_JJ.ContainsKey(method_JJ._name))
                methods_JJ.Add(method_JJ._name, new List<Method_JJ>());
            methods_JJ[method_JJ._name].Add(method_JJ);
        }

        //pour chaque méthode tri les surcharges
        foreach (string method_name in methods_JJ.Keys)
        {
            List<Method_JJ> method = methods_JJ[method_name];
            method.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
        }


        //tri les nom des méthodes
        List<string> names = new List<string>(this.methods_JJ.Keys);
        names.Sort();
        
        //fill dropdown
        dd_methods.options.Clear();
        //foreach (string method_name in names)
        //{
        //    List<Method_JJ> method = methods_JJ[method_name];
        //    string dd_name = method_name;// + (method.Count == 1 ? "" : " [" + method.Count + "]");
        //    dd_methods.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = dd_name });
        //}

        foreach (string method_name in names)
        {
            List<Method_JJ> method = methods_JJ[method_name];
            foreach (var item in method)
            {

            string dd_name = item.ToString();// + (method.Count == 1 ? "" : " [" + method.Count + "]");
            dd_methods.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = dd_name });
            }
        }

        dd_methods.value = 0;
        dd_methods.RefreshShownValue();
    }

    public void _DropdownMethodValueChanged(TMPro.TMP_Dropdown dropDown)
    {
        string txt = dropDown.options[dropDown.value].text;
        int crochet = txt.IndexOf(" [");
        string method_name = (crochet != -1) ? txt.Substring(0, crochet) : txt;
        // Debug.Log(method_name);
        // Debug.Log(methods_JJ[method_name].Count);

        //fill dropdown surcharges
        dd_methods_surcharges.options.Clear();
        foreach (Method_JJ method in methods_JJ[method_name])
        {
            string dd_name = method.ToString();
            dd_methods_surcharges.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = dd_name });
        }
        dd_methods_surcharges.value = 0;
        dd_methods_surcharges.RefreshShownValue();
    }




    void OpenCVConstantes()
    {
        FieldInfo[] fields = CV.GetFields();

        //liste des noms complets des constantes
        List<string> names = new List<string>();
        foreach (FieldInfo field in fields)
            names.Add(field.Name);
        names.Sort();

        //liste des catégories
        //je range dans chaque catégorie les noms
        constantes = new Dictionary<string, List<string>>();
        foreach (string name in names)
        {
            int find_underscore = name.IndexOf('_');
            string categorie_constante = (find_underscore == -1) ? name : name.Substring(0, find_underscore);
            if (!constantes.ContainsKey(categorie_constante))
                constantes.Add(categorie_constante, new List<string>());
            constantes[categorie_constante].Add(name);
        }

        //fill dropdown
        dd_constantes_cat.options.Clear();
        foreach (string cat in constantes.Keys)
        {
            dd_constantes_cat.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = cat });
        }
    }

    public void _DropdownConstantesValueChanged(TMPro.TMP_Dropdown dropDown)
    {
        string categorie_constante = dropDown.options[dropDown.value].text;
        //Debug.Log(categorie_constante);

        //fill dropdown surcharges
        dd_constantes.options.Clear();
        foreach (string constante in constantes[categorie_constante])
        {
            dd_constantes.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = constante });
        }
        dd_constantes.value = 0;
        dd_constantes.RefreshShownValue();
    }
}
