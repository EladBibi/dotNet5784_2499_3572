

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;
 
internal class DependencyImplementation:IDependency
{
    readonly string s_dependency_xml = "dependencies";

    public int Create(Dependency item)
    {
        int NewId = Config.NextDependencyId;
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement NewDependency = create(item, NewId);
       DependencyList.Add(NewDependency);
        XMLTools.SaveListToXMLElement(DependencyList, "dependencies");
        return NewId;
    }

    public void Delete(int id)
    {
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement dependency = DependencyList.Elements().FirstOrDefault(s => (int?)s.Element("id") == id) ??
            throw new DalDoesNotExistException(@"Object of type ""Dependency"" with such ID does not exist");
        dependency.Remove();
        XMLTools.SaveListToXMLElement(DependencyList, "dependencies");
    }

    public Dependency? Read(int id)
    {
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);

        Dependency? dependencyObject = (Dependency?)(from s in DependencyList.Elements()
          where (int?)s.Element("id") == id
         select new Dependency()
       {

        Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
        DependentTask = (int?)(s.Element("dependentTask")) ?? 0 ,
        DependsOnTask = (int?)(s.Element("dependsOnTask")) ?? 0,
        
          }).FirstOrDefault();

        return dependencyObject;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        IEnumerable<Dependency> Dependencies = DependencyList.Elements()
              .Select(s => new Dependency()
              {


                  Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                  DependentTask = (int?)(s.Element("dependentTask")) ?? 0,
                  DependsOnTask = (int?)(s.Element("dependsOnTask")) ?? 0,




              });
        return Dependencies.FirstOrDefault(k => filter(k));
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        IEnumerable<Dependency> Dependencies = DependencyList.Elements()
              .Select(s => new Dependency()
              {


                  Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                  DependentTask = (int?)(s.Element("dependentTask")) ?? 0,
                  DependsOnTask = (int?)(s.Element("dependsOnTask")) ?? 0,

              });
        if (filter == null)
            return Dependencies;
        else
            return Dependencies.Where(filter);


    
}

    public void Update(Dependency item)
    {
        Delete(item.Id);
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        
        XElement UpdateDependency = create(item, item.Id);
        DependencyList.Add(UpdateDependency);
        XMLTools.SaveListToXMLElement(DependencyList, "dependencies");
    }





    public void DeleteAll()
    {
        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        DependencyList.RemoveAll();
        XMLTools.SaveListToXMLElement(DependencyList, "dependencies");
        XElement? config = XMLTools.LoadListFromXMLElement("data-config");
        config.Element("NextDependencyId")?.SetValue("0");
        XMLTools.SaveListToXMLElement(config, "data-config");
    }

    //פונקציית עזר ליצירת אובייקט חדש עבור הכנסה לקובץ
    public XElement create(Dependency item, int NewId)
    {

        XElement? DependencyList = XMLTools.LoadListFromXMLElement(s_dependency_xml);


        XElement NewDependency = new XElement("Dependency", new XElement("id", NewId),

            new XElement("dependentTask", item.DependentTask),
            new XElement("dependsOnTask", item.DependsOnTask));

        return NewDependency;
    }




    
}
