# 🎯 GuestInfo API

A simple RESTful API built with ASP.NET Core for managing guest records in SQL Server.

---

## ✨ Features

✅ List all guests  
✅ Query guest by mobile number or name  
✅ Add new guest records  
✅ Update guest information (PUT, PATCH)  
✅ Delete guests  
✅ Check existence (HEAD)  
✅ Discover allowed methods (OPTIONS)  
✅ Secured with API Key authentication  

---

## 🛠️ Technologies Used

- ASP.NET Core Web API (.NET 6 or later)
- C#
- SQL Server (Local or Network)
- Microsoft.Data.SqlClient

---

## 🚀 API Endpoints

| Method   | Endpoint                                 | Description                                   |
|----------|-------------------------------------------|-----------------------------------------------|
| GET      | `/api/guestinfo`                        | List all guests                              |
| POST     | `/api/guestinfo/name-by-mobile`         | Get guest name by mobile number              |
| POST     | `/api/guestinfo/mobile-by-name`         | Get mobile number by guest name              |
| POST     | `/api/guestinfo/add`                    | Add a new guest                              |
| PUT      | `/api/guestinfo/update-name`            | Update guest name by mobile number           |
| PATCH    | `/api/guestinfo/update`                 | Partially update guest record                |
| DELETE   | `/api/guestinfo/delete`                 | Delete guest by mobile number                |
| HEAD     | `/api/guestinfo/exists/{mobileNumber}`  | Check if guest exists by mobile number       |
| OPTIONS  | `/api/guestinfo`                        | Retrieve supported HTTP methods              |

---

## 🔐 API Security

All endpoints require an API Key in headers:


You can configure the key in `appsettings.json`:

