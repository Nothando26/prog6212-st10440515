Contract Monthly Claim System (CMCS)

GitHub Repository 
https://github.com/Nothando26/prog6212-st10440515.git

Youtube Link
https://youtu.be/YX71XXjyyuo


About the Application

The Claims Management and Coordination System (CMCS) is a web-based ASP.NET Core MVC application developed to simplify and automate the process of handling lecturer claims within an academic institution. The system allows lecturers to submit claims for hours worked, upload supporting documents, and track their claim status. Programme coordinators and academic managers are able to review, verify, and approve or reject these claims efficiently, ensuring that the entire process is transparent and well-documented.

The application supports three main user roles — Lecturer, Programme Coordinator, and Academic Manager. Lecturers can submit claims by entering their hours worked and hourly rate, while also uploading a supporting document in PDF, DOCX, or XLSX format. Coordinators review the claims and can either accept, reject, or request further verification. Managers perform the final review and approval of each claim. Both the coordinator and manager have access to download and view the uploaded files.

The system uses ASP.NET Core MVC for structure, Entity Framework Core for database interaction, and SQL Server for data storage. Session-based authentication ensures that only authorized users can access their respective dashboards. Error handling and input validation are included, particularly for file uploads, which have restrictions on both file type and size.

A set of unit tests has been included to verify the functionality of key system components such as claim creation, amount calculation, role-based access, and dashboard rendering. These tests were written using xUnit and can be executed through Visual Studio’s Test Explorer.

To use the system, users must log in with their role credentials. Lecturers can create and submit new claims, coordinators can manage and review claims, and managers can perform final approvals. The system is designed to provide a reliable and efficient workflow for managing academic claims while maintaining data integrity and accountability.
