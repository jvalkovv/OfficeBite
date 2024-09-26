# OfficeBite
# URL: <a href="https://officebite.freeddns.org/"> OFFICEBITEAPP ON IIS Deploy on own Windows Server Host</a>
# URL: <a href="https://officebite.azurewebsites.net/"> OFFICEBITEAPP ON Azure Deploy on Virtual Host</a>

# OfficeBite

**OfficeBite** is a web-based application for managing the food menu in offices or factories. The application allows users to view menus for specific dates, order meals, generate reports by staff, and includes an administrative panel for role management by administrators.

## Features

### User Section
- **Viewing Menus for Specific Dates:**  
  Users can browse the menus for specific dates and see the offered dishes.
- **Selecting and Ordering Lunch Meals:**  
  Users can choose and order meals from the available menus for specified dates.
- **Registration and Login:**  
  Users can register and log in to the system.

### Administrative Panel
- **User Role Management:**  
  Administrators can manage user roles and permissions within the application.

### Staff Panel
Users with the **"Staff"** role have access to additional functionalities:
- Add a new dish to the existing offerings.
- Edit dishes.
- Hide dishes.
- Add menus for selected dates.
- View and process orders.
- View all dishes.
- View hidden dishes.
- Generate reports for all requests.

## Technologies Used

- **ASP.NET Core MVC:** For creating the web application and managing user requests.
- **Entity Framework Core:** For interacting with the database and accessing information about menus and orders.
- **HTML, CSS, and JavaScript:** For developing the front-end and creating the user interface.
- **Microsoft SQL Server:** For storing data related to menus, orders, and user accounts.
- **Identity Framework:** For managing user accounts and roles.

## Hosting

The application is hosted on a configured server using Windows Server 2022 and is published via Internet Information Services (IIS).

## Registration and Login

You can use the following credentials in the live environment:

- **Username:** `employee`  
  **Password:** `Employee@123`

You can also use the following credentials after downloading and starting the application:

1. **Username:** `user`  
   **Password:** `User@123`

2. **Username:** `employee`  
   **Password:** `Employee@123`

3. **Username:** `manager`  
   **Password:** `Manager@123`

4. **Username:** `admin`  
   **Password:** `admin@123`

## About

This application provides convenience and efficiency for both users and administrators, enabling easy and fast management of the lunch menu in the office.

# OfficeBite

OfficeBite е уеб базирано приложение за управление на хранителното меню в офиси или заводи.
Приложението позволява на потребителите да разглеждат менютата за определени дати, да поръчват ястия, включва генерирането на справки от персонала и административен панел за управление на ролите от администраторите.
    
## Възможности

### Потребителска част
- Преглед на менюта за конкретни дати: 
Потребителите могат да разглеждат менютата за конкретни дати и да видят предлаганите ястия.
- Избор и поръчване на ястия за обяд:
Потребителите могат да избират и поръчват ястия от наличните менюта за определени дати.
- Регистрация и вход в системата

### Административен панел
- Управление на потребителските роли

### Панел на персонала
Потребители с роля **"Staff"**
- Добавяне на ново ястие към предлаганите до момента.
- Редактиране на ястия
- Скриване на ястия
- Добяване на менюта за избрани дати
- Преглед и обработка на поръчките
- Преглед на всички ястия
- Преглед на скритите ястия
- Справка за всички заявки
  
## Използвани Технологии

- **ASP.NET Core MVC**: За създаване на уеб приложението и управление на заявките на потребителите.
- **Entity Framework Core**: За взаимодействие с базата данни и достъп до информация за менюта и поръчките.
- **HTML, CSS и JavaScript**: За разработка на фронтенд частта и създаване на потребителския интерфейс.
- **Microsoft SQL Server**: За съхранение на данни свързани с менюта, поръчките и потребителските акаунти.
- **Identity Framework**: За управление на потребителските акаунти и ролите.

## Хостване

Приложението е хостнато на конфигуриран сървър - Windows Server 2022 и е публикувано чрез Internet Information Services (IIS).

## Регистрация и Вход:
- Може да изпозлвате потребителско име и парола в реалната среда:
  * Потребителско име: employee
  * Парола за вход: Employee@123
  
- Може да изпозлвате потребителско име и парола, след като изтеглите и стартирате приложението:
  * 1.Потребителско име: user
  * 1.Парола за вход: User@123
    
  * 2.Потребителско име: employee
  * 2.Парола за вход: Employee@123
    
  * 3.Потребителско име: manager
  * 3.Парола за вход: Manager@123
 
  * 4.Потребителско име: admin
  * 4.Парола за вход: admin@123


Това приложение предоставя удобство и ефективност както за потребителите, така и за администраторите, 
като осигурява лесно и бързо управление на обедното меню в офиса.

