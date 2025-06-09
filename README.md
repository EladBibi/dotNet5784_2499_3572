# Project Overview – Multi-Layered Engineering Task Management System
This project is a comprehensive multi-layered .NET application developed as part of an academic software engineering course. The system is designed to manage engineers, tasks, and dependencies between them in a structured and maintainable way, using a robust three-tier architecture  
System Purpose:  
The goal of the project is to simulate a full-scale task scheduling and engineer management system, where project managers can assign tasks, manage dependencies, monitor progress via Gantt-like views, and handle scheduling constraints  
The system reflects real-world scenarios in software project management  

---

## Architecture Breakdown

1️⃣ **DAL (Data Access Layer)**  
- DAL Structure - Split into two implementations:  
  - DalList: stores data using in-memory static lists  
  - DalXml: handles persistent data storage using XML files (e.g., tasks.xml, engineers.xml, dependencies.xml, etc.)  
- Interfaces are defined in DalApi (e.g., IEngineer, ITask, IDependency) to decouple implementation logic from the business logic layer  
- XML Configuration: The XML-based persistence is handled via XmlTools.cs, which abstracts serialization and deserialization for flexible and durable data management  
- Configuration Files: XML-based configs like dal-config.xml and data-config.xml are used to dynamically control which DAL is active (List/XML), showcasing support for runtime configurability  

---

2️⃣ **BL (Business Logic Layer)**  
- Centralized in the BL folder under BiImplementation and BO (Business Objects)  
- Contains core classes such as Engineer, Task, Milestone, and supporting DTOs like TaskInList, EngineerInTask, TaskInGantt  
- Implements all logic related to:  
  - Assigning engineers to tasks  
  - Handling task dependencies and schedules  
  - Validating data (via custom exceptions in Exceptions.cs)  
  - Generating timelines and suggesting task assignments based on availability and skills  
- Entry point to the BL is abstracted through IBl and its Factory, which allows seamless switching between implementations  

---

3️⃣ **PL (Presentation Layer) – WPF Interface**  
- Built using WPF (Windows Presentation Foundation) with XAML-based GUI definitions  
- Divided into:  
  - General windows: MainWindow.xaml, LoginWindow.xaml, Admin.xaml  
  - Task-specific interfaces: TaskListWindow, EditTask  
  - Engineer-specific views: EngineerWindow, EngineerListWindow, SuggestedTasks  
  - Visualization tools like GanttWindow for project timelines  
- WPF Features Used:  
  - Data Binding and Converters for UI-model synchronization  
  - Custom controls and layout logic  
  - MVVM-aligned separation, although basic, allows clear control over logic/UI  

---

## Technologies Used  
- Languages: C# (.NET Core 7/8)  
- Frameworks:  
  - WPF (UI)  
  - .NET for class libraries and console apps  
- Data Storage: XML-based file persistence  
- Architecture: Three-tier design with full separation of concerns  
- Version Control: Git  
- Testing: Projects like BlTest, DalTest, and stage0 include unit tests and initialization routines  

---

## Additional Highlights  
- Configuration Flexibility: Switching between DAL implementations is as simple as updating dal-config.xml, enabling testing and production scenarios to coexist  
- Tools Layer: Utilities in Tools.cs provide generic helper methods (e.g., deep copies, ID generators)  
- Gantt Support: A task view module mimics real-world Gantt charts for project management  

---

## Testing & Validation  
- BlTest and DalTest provide automated testing capabilities for key modules  
- stage0 provides initial bootstrapping and seeding of the system with default data, ensuring rapid setup and demonstration  
