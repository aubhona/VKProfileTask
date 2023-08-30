# VKProfileTask

Profile task for selections in Vk internship.

Eng
Technical specification:
It is necessary to implement the API application on ASP.NET Core (5 or
later version).
Business logic requirements and constraints:
The request/response format must be JSON.
- API methods must be asynchronous.
- It is necessary to use PostgreSQL as a DBMS.
- You need to use EntityFrameworkCore as an ORM.
- The following entities should be used as data models:
-- user (id, login, password, created_date, user_group_id, user_state_id
-- user_group (id, code, description) Possible values for code (Admin, User)
-- user_state (id, code, description) Possible values for code (Active, Blocked).
- The application should allow you to add/remove/receive users. You can get both one and all users (adding/removing only one at a time). When receiving users, full information about them should be returned (with user_group and user_state).
- The system should not allow to have more than one user user_group.code = "Admin".
- After successful registration of a new user, he must be assigned the status "Active". Adding a new user should take 5 seconds. During this time, an error should be returned when trying to add a user with the same login.
- The user should be deleted not by physical deletion from the table, but by setting the "Blocked" status of the user.
- It is allowed to add auxiliary data to existing tables. 
- Implement pagination to get multiple users.
_____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________
Rus:
Техническое задание:
Необходимо реализовать АРІ приложение на ASP.NET Core (5 ли
более поздняя версия).
Требования бизнес-логики и ограничения:
-	Формат запроса/ответа должен быть JSON.
-	Методы АРІ должны быть асинхронными.
-	В качестве СУБД необходимо использовать PostgreSQL.
-	В качестве ORM необходимо использовать EntityFrameworkCore.
-	В качестве моделей данных должны использоваться следующие сущности:
--	user (id, login, password, created_date, user_group_id, user_state_id
--	user_group (id, code, description) Возможные значения для code (Admin, User)
--	user_state (id, code, description) Возможные значения для code (Active, Blocked).
-	Приложение должно позволять добавлять/удалять/получать пользователей. Получить можно как одного, так и всех пользователей (добавление/удаление только по одному). При получении пользователей должна возвращаться полная информация о них (с user_group и user_state).
- Система должна не позволять иметь более одного пользователя
user_group.code = "Admin".
- После успешной регистрации нового пользователя, ему должен быть выставлен статус "Active". Добавление нового пользователя должно занимать 5 сек. За это время при попытке добавления пользователя с таким же login должна возвращаться ошибка.
- Удаление пользователя должно осуществляться не путём физического удаления из таблицы, а путём выставления статуса
"Blocked" у пользователя.
- Допускается добавлять вспомогательные данные в существующие таблицы.
- Реализовать пагинацию для получения нескольких пользователей.

