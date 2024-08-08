![Снимок экрана 2024-08-07 230736](https://github.com/user-attachments/assets/5169f4d9-723d-460a-aa7e-b043de478533)
![Снимок экрана 2024-08-07 230840](https://github.com/user-attachments/assets/df3098ba-93d5-44d1-9bbb-c0d266f1d802)
![Снимок экрана 2024-08-07 231237](https://github.com/user-attachments/assets/7ea9a599-dc6a-4556-b070-9aa7b93e10ee)
![Снимок экрана 2024-08-07 231030](https://github.com/user-attachments/assets/e1962cf0-c358-416c-8d5f-8c0257945eaa)

The program is an aggregator of rental apartments. It provides an opportunity to store apartment data (including photos, up to 10 photos). To add an apartment, you need to add data about the apartment building in which the apartment is located. One apartment building can include several apartments. The data is available for editing and deleting. Validation of input data is implemented. When you delete a house, all associated apartments and apartment-related photos are deleted.
Selecting a building displays detailed information about the building and a list of available apartments in the building. Selecting an apartment displays detailed information about the building and the apartment, as well as photos of that apartment, which can be scrolled through using interactive buttons. The photo navigation buttons are available if you put the cursor over the block with information and if there are photos to display. The buttons are not available if the cursor is not over the information block or there are no photos. If the entire list of photos is scrolled through, the buttons are disabled.
Multiple selection is supported when adding photos.
The "Edit" and "Delete" buttons are available if an apartment or a house is selected. Adding a new record is done through the "Add" window and is called by pressing the "Add" button. If a house is selected, pressing "Add" opens the window of adding a house. If a house and an apartment are selected, a dialog box opens with a choice to add a house or an apartment. The "Edit" button works on the same principle.

The application has a multi-layered architecture and is divided into 4 projects:
1.	CS_WPF_Lab9_Rental_Housing (Application),
2.	CS_WPF_Lab9_Rental_Housing.Business (Business logic of the application),
3. CS_WPF_Lab9_Rental_Housing.DAL (Database Access),
4.	CS_WPF_Lab9_Rental_Housing.Domain (Application Entities).
A detailed description of the projects (layers) and files is provided in paragraph 2 "Solution. Code Listing." In the main window, the display of information is realized in the "Master-Slave" style. The database is created in the "Code First" style. Deletion of records is cascaded. Images are stored in the Images folder, deleting images is performed asynchronously. Converters are used to display information correctly. The application implements the MVVM template. It uses data binding to display information from models and a command mechanism to react to user actions.
The packages used are:
1. Microsoft.Xaml.Behaviors.Wpf (1.1.122)
2. MaterialDesignThemes (5.1.0)
3. Microsoft.EntityFrameworkCore.SqlServer (8.0.7)
4. Microsoft.Extensions.Configuration.Json (8.0.0)
