# CS_WPF_Lab9_Rental_Housing

## Описание приложения

Это программа-агрегатор для аренды квартир. Она предоставляет возможность хранить данные о квартирах (включая фотографии, до 10 фотографий). Для добавления квартиры необходимо добавить данные о жилом доме, в котором она находится. Один жилой дом может включать несколько квартир. Данные доступны для редактирования и удаления. Реализована валидация вводимых данных. При удалении дома удаляются все связанные квартиры и фотографии квартир.

При выборе дома отображается подробная информация о доме и список доступных в нем квартир. Выбор квартиры отображает подробную информацию о доме и квартире, а также фотографии этой квартиры, которые можно просматривать с помощью интерактивных кнопок. Навигационные кнопки фотографий доступны, если курсор находится над блоком информации и если есть фотографии для отображения. Кнопки не доступны, если курсор не находится над блоком информации или фотографий нет. Если пролистан весь список фотографий, кнопки деактивируются.

![Снимок экрана 1](https://github.com/user-attachments/assets/5169f4d9-723d-460a-aa7e-b043de478533)
![Снимок экрана 2](https://github.com/user-attachments/assets/df3098ba-93d5-44d1-9bbb-c0d266f1d802)
![Снимок экрана 3](https://github.com/user-attachments/assets/7ea9a599-dc6a-4556-b070-9aa7b93e10ee)
![Снимок экрана 4](https://github.com/user-attachments/assets/e1962cf0-c358-416c-8d5f-8c0257945eaa)

## Многоуровневая архитектура приложения

Приложение разделено на 4 проекта:
1. **Приложение** - [CS_WPF_Lab9_Rental_Housing](https://github.com/AlexPol1ak/CS_WPF_Lab9_Rental_Housing)
2. **Бизнес логика** - [CS_WPF_Lab9_Rental_Housing.Business](https://github.com/AlexPol1ak/CS_WPF_Lab9_Rental_Housing.Business)
3. **Доступ к базе данных** - [CS_WPF_Lab9_Rental_Housing.DAL](https://github.com/AlexPol1ak/CS_WPF_Lab9_Rental_Housing.DAL)
4. **Сущности приложения** - [CS_WPF_Lab9_Rental_Housing.Domain](https://github.com/AlexPol1ak/CS_WPF_Lab9_Rental_Housing.Domain)

## Используемые пакеты

- Microsoft.Xaml.Behaviors.Wpf (1.1.122)
- MaterialDesignThemes (5.1.0)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.7)
- Microsoft.Extensions.Configuration.Json (8.0.0)
