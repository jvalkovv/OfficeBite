# OfficeBite
# URL: <a href="https://gndstore.hopto.org:4433/"> OFFICEBITEAPP </a>
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
Имплементирани са CI and CD

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

