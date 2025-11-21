github link:
youtube video link:https://youtu.be/AAo5FLiUSkM


The Contract Monthly Claim System (CMCS) is a web-based application developed using ASP.NET MVC and C#. The system is designed to streamline the process of lecturer claims by allowing lecturers to submit their worked hours along with supporting documents. Coordinators and academic managers can review, approve, reject, or request further verification for submitted claims. By automating the claim workflow, CMCS improves transparency, reduces manual paperwork, and allows real-time tracking of claim statuses using colored badges for clear visibility.

The application supports multiple user roles with distinct functionalities. Lecturers can submit claims, upload supporting documents, and track the status of their claims, such as Pending, Accepted, Rejected, or Further Verification. Coordinators can access the Coordinator Dashboard to review claims submitted by lecturers and decide whether to accept, reject, or mark claims for further verification. Academic managers perform the final review of claims, ensuring accountability and compliance with organizational standards. Additionally, HR users have access to tools for creating and managing user accounts, inputting hourly rates for lecturers, and generating reports summarizing approved claims. These reports can be downloaded as PDF invoices, helping with payment processing and record keeping.

The login and registration system ensures that only authorized users can access the application. New users must provide their details and select their role, while existing users can log in using their email and password. The system implements Cookie Authentication and ASP.NET Session Management to secure user data and maintain role-based access control. Once logged in, users are directed to dashboards tailored to their role, allowing them to perform their respective tasks efficiently.

From a technical perspective, the application leverages Entity Framework Core for database operations and stores data in SQL Server. The frontend is built with HTML5, CSS3, and Bootstrap, providing a clean and responsive user interface. PDF reports are generated using the QuestPDF library, which allows HR to download comprehensive summaries of approved claims. The system also uses TempData to communicate success and error messages between controller actions and views.

Throughout the development process, several resources and references were used to implement features and follow best practices. These include the official Microsoft documentation for ASP.NET Core MVC and Entity Framework Core, online tutorials such as the ASP.NET MVC Crash Course on YouTube, QuestPDF’s official documentation, and general web development references such as W3Schools and TutorialsPoint. These resources guided the design and functionality of the CMCS system, ensuring a robust and user-friendly application.

References
Microsoft. (2023) ASP.NET Core Documentation. Microsoft Docs. Available at: https://learn.microsoft.com/en-us/aspnet/core/
 (Accessed: 16 November 2025).

 Esposito, D. (2020) Modern Web Development with ASP.NET Core MVC. Microsoft Press: Redmond.

 TutorialsTeacher. (2025) ASP.NET MVC Controller and View. Available at: https://www.tutorialsteacher.com/mvc
 (Accessed: 19 November 2025).

 QuestPDF. (2025) QuestPDF Documentation. Available at: https://www.questpdf.com/
 (Accessed: 19 November 2025).

 Techie Expert. (2023) How to Generate PDF in ASP.NET Core MVC using QuestPDF. YouTube. Available at: https://www.youtube.com/watch?v=3qEPv-67iRg
 (Accessed:17 November 2025).

 Stack Overflow, 2025. How to use session in ASP.NET Core MVC. [online] Available at: https://stackoverflow.com/questions/51073861/asp-net-core-session-example
 [Accessed 16 November 2025].

 W3Schools, 2025. HTML Forms. [online] Available at: https://www.w3schools.com/html/html_forms.asp
 [Accessed 16 November 2025].

 W3Schools, 2025. Bootstrap 5 Forms. [online] Available at: https://www.w3schools.com/bootstrap5/bootstrap_forms.php
 [Accessed 16 November 2025].

 
