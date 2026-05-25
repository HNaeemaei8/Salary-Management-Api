# Salary Management API

یک سیستم جامع مدیریت حقوق و دستمزد طراحی شده با **ASP.NET Core Web API**. این پروژه با رعایت اصول معماری تمیز (Clean Architecture) پیاده‌سازی شده تا مقیاس‌پذیری، نگهداری آسان و تست‌پذیری بالا را تضمین کند.

##سس ویژگی‌های کلیدی

-  **مدیریت پرسنل:** ثبت، ویرایش و مدیریت اطلاعات کارمندان.
-  **محاسبه هوشمند حقوق:** محاسبه خودکار مبالغ حقوق، اضافه‌کاری، کسورات و بیمه.
-  **مستندات تعاملی:** استفاده از **Swagger** برای تست آسان APIها.
-  **معماری ماژولار:** پیاده‌سازی بر اساس الگوی Clean Architecture (Domain, Application, Infrastructure, API).
-  **تست‌پذیری:** دارای مجموعه‌ای از Unit Testها برای اطمینان از صحت عملکرد.
-  **پشتیبانی از Docker:** امکان اجرای سریع پروژه در محیط ایزوله.

## تکنولوژی‌های استفاده‌شده

- **Backend:** ASP.NET Core 8/9 Web API
- **Database:** SQL Server + Entity Framework Core + Dapper
- **Documentation:** Swagger / OpenAPI
- **Testing:** xUnit / Moq
- **Containerization:** Docker

## پیش‌نیازها

قبل از شروع، اطمینان حاصل کنید که ابزارهای زیر روی سیستم شما نصب هستند:

- [.NET SDK 8.0 یا بالاتر](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) یا [VS Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (اختیاری)

##  نحوه اجرا
## کلون کردن پروژه
```bash
git clone https://github.com/HNaeemaei8/Salary-Management-Api.git
cd Salary-Management-Api