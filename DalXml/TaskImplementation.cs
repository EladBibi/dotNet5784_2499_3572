
namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {

        int NewId = Config.NextTaskId;
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);

        XElement NewTask = create(item, NewId);
        TaskList.Add(NewTask);
        XMLTools.SaveListToXMLElement(TaskList, "tasks");
        return NewId;


    }

    public void Delete(int id)
    {
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        XElement task = TaskList.Elements().FirstOrDefault(s => (int?)s.Element("id") == id) ??
            throw new DalDoesNotExistException(@"Object of type ""Task"" with such ID does not exist");
        task.Remove();
        XMLTools.SaveListToXMLElement(TaskList, "tasks");
    }

    public Task? Read(int id)
    {
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);

        Task? taskObject = (Task?)(from s in TaskList.Elements()
                                   where (int?)s.Element("id") == id
                                   select new Task()
                                   {
                                       Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                                       EngineerId = (int)(s.Element("engineerid") ?? throw new XmlFormatException("engineerid")),
                                       Alias = (string?)(s.Element("alias")),
                                       Description = (string?)(s.Element("description")),
                                       Deliverables = (string?)(s.Element("deliverables")),
                                       Remarks = (string?)(s.Element("remarks")),
                                      

                                       CreatedAtDate = s.ToDateTimeNullable("createdAtDate"),
                                       scheduledDate = s.ToDateTimeNullable("scheduledDate"),
                                       StartDate = s.ToDateTimeNullable("startDate"),
                                       CompleteDate = s.ToDateTimeNullable("completeDate"),
                                      
                                       Complexity = s.ToEnumNullable<DO.EngineerExperience>("complexity") ?? 0,
                                       RequiredEffortTime = s.ToTimeSpanNullable("requiredEffortTime")

                                   }).FirstOrDefault();

        return taskObject;
    }

    public Task? Read(Func<Task, bool> filter)
    {
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        IEnumerable<Task> Tasks = TaskList.Elements()
              .Select(s => new Task()
              {
                  Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                  EngineerId = (int)(s.Element("engineerid") ?? throw new XmlFormatException("engineerid")),
                  Alias = (string?)(s.Element("alias")),
                  Description = (string?)(s.Element("description")),
                  Deliverables = (string?)(s.Element("deliverables")),
                  Remarks = (string?)(s.Element("remarks")),
                 

                  CreatedAtDate = s.ToDateTimeNullable("createdAtDate"),
                  scheduledDate = s.ToDateTimeNullable("scheduledDate"),
                  StartDate = s.ToDateTimeNullable("startDate"),
                  CompleteDate = s.ToDateTimeNullable("completeDate"),
                 
                  Complexity = s.ToEnumNullable<DO.EngineerExperience>("complexity") ?? 0,
                  RequiredEffortTime = s.ToTimeSpanNullable("requiredEffortTime")
              });
        return Tasks.FirstOrDefault(k => filter(k));
    }






    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        IEnumerable<Task> Tasks = TaskList.Elements()
            .Select(s => new Task()
            {
                Id = (int)(s.Element("id") ?? throw new XmlFormatException("id")),
                EngineerId = (int)(s.Element("engineerid") ?? throw new XmlFormatException("engineerid")),
                Alias = (string?)(s.Element("alias")),
                Description = (string?)(s.Element("description")),
                Deliverables = (string?)(s.Element("deliverables")),
                Remarks = (string?)(s.Element("remarks")),
                //IsMilestone = bool.TryParse(s.Element("isMilestone")?.Value, out bool result) ? result : false,

                CreatedAtDate = s.ToDateTimeNullable("createdAtDate"),
                scheduledDate = s.ToDateTimeNullable("scheduledDate"),
                StartDate = s.ToDateTimeNullable("startDate"),
                CompleteDate = s.ToDateTimeNullable("completeDate"),
                //DeadLineDate = s.ToDateTimeNullable("deadLineDate"),
                Complexity = s.ToEnumNullable<DO.EngineerExperience>("complexity") ?? 0,
                RequiredEffortTime = s.ToTimeSpanNullable("requiredEffortTime")


            });
        if (filter == null)
            return Tasks;
        else
            return Tasks.Where(filter);
    }

    public void Update(Task item)
    {
        Delete(item.Id);
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);

        XElement UpdateTask = create(item, item.Id);
       TaskList.Add(UpdateTask);
        XMLTools.SaveListToXMLElement(TaskList, "tasks");
    }
    public void DeleteAll()
    {
        XElement? TaskList = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        TaskList.RemoveAll();
        XMLTools.SaveListToXMLElement(TaskList, "tasks");
        XElement? config = XMLTools.LoadListFromXMLElement("data-config");
        config.Element("NextTaskId")?.SetValue("0");
        XMLTools.SaveListToXMLElement(config, "data-config");

    }
    //פונקציית עזר ליצירת אובייקט חדש עבור הכנסה לקובץ
    public XElement create(Task item, int NewId)
    {

        XElement NewTask = new XElement("Task", new XElement("id", NewId),

          new XElement("engineerid", item.EngineerId),
          new XElement("alias", item.Alias), new XElement("description", item.Description),
          new XElement("deliverables", item.Deliverables), new XElement("remarks", item.Remarks),
          new XElement("createdAtDate", item.CreatedAtDate),
         new XElement("scheduledDate", item.scheduledDate), new XElement("startDate", item.StartDate),
         new XElement("completeDate", item.CompleteDate),
         new XElement("complexity", item.Complexity), new XElement("requiredEffortTime", item.RequiredEffortTime));
        return NewTask;

    }
}









    
