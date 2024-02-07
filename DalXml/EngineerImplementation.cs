

namespace Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        if (EngineerList.Elements().FirstOrDefault(s => (int?)s.Element("id") == item.Id)
            is not null)
            throw new DalAlreadyExistsException("An object of type Engineer with such an ID already exists");

        XElement NewEngineer = create(item, item.Id);

            
        EngineerList.Add(NewEngineer);
        XMLTools.SaveListToXMLElement(EngineerList, "engineers");
        return item.Id;
    }




    public void Delete(int id)
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        XElement engineer = EngineerList.Elements().FirstOrDefault(s => (int?)s.Element("id") == id) ??
            throw new DalDoesNotExistException(@"Object of type ""Engineer"" with such ID does not exist");
        engineer.Remove();
        XMLTools.SaveListToXMLElement(EngineerList, "engineers");
    }



    public Engineer? Read(int id)
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);

        Engineer? engineerObject = (Engineer?)(from s in EngineerList.Elements()
                                               where (int?)s.Element("id") == id
                                               select new Engineer()
                                               {

                                                   Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                                                   Cost = s.ToDoubleNullable("cost") ?? 0.0,
                                                   name = (string?)s.Element("name") ?? "",
                                                   Email = (string?)s.Element("email") ?? "",
                                                   level = s.ToEnumNullable<DO.EngineerExperience>("level") ?? 0
                                               }).FirstOrDefault();

        return engineerObject;

    }



    public Engineer? Read(Func<Engineer, bool> filter)
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        IEnumerable<Engineer> Engineers = EngineerList.Elements()
              .Select(s => new Engineer()
              {


                  Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                  Cost = s.ToDoubleNullable("cost") ?? 0.0,
                  name = (string?)s.Element("name") ?? "",
                  Email = (string?)s.Element("email") ?? "",
                  level = s.ToEnumNullable<DO.EngineerExperience>("level") ?? 0

              });
        return Engineers.FirstOrDefault(k => filter(k));
    }







    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        IEnumerable<Engineer> Engineers = EngineerList.Elements()
              .Select(s => new Engineer()
              {


                  Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                  Cost = s.ToDoubleNullable("cost") ?? 0.0,
                  name = (string?)s.Element("name") ?? "",
                  Email = (string?)s.Element("email") ?? "",
                  level = s.ToEnumNullable<DO.EngineerExperience>("level") ?? 0

              });
        if (filter == null)
            return Engineers;
        else
            return Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        Delete(item.Id);
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);

        XElement UpdateEngineer = create(item, item.Id);
        EngineerList.Add(UpdateEngineer);
        XMLTools.SaveListToXMLElement(EngineerList, "engineers");
    }



    public void DeleteAll()
    {
        XElement? EngineerList = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        EngineerList.RemoveAll();
        XMLTools.SaveListToXMLElement(EngineerList, "engineers");
    }

    //פונקציית עזר ליצירת אובייקט חדש עבור הכנסה לקובץ
    public XElement create(Engineer item, int NewId)
    {




        XElement NewEngineer = new XElement("Engineer", new XElement("id", NewId),



            new XElement("cost", item.Cost), new XElement("name", item.name),
            new XElement("email", item.Email),
            new XElement("level", item.level));


        return NewEngineer;
    }


}


