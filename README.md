# ShopSystem.Api

Boilerplate ASP.NET Core 8 Web API for an online shopping / e-commerce class project.
Built around the spec: **Actors** (Customer, Seller, Admin, Courier), **Use Cases**
(browse, add to cart, checkout, track order, review), and the **Sequence**
add to cart → checkout → payment → order fulfillment → delivery.

## Project structure

```
ShopSystem.Api/
├── Controllers/        # One controller per use-case area
├── Models/              # Entities + shared enums
├── Data/AppDbContext.cs # EF Core DbContext
├── Program.cs
├── appsettings.json
└── ShopSystem.Api.csproj
```

## Domain model

| Entity      | Purpose                                                            |
|-------------|---------------------------------------------------------------------|
| `User`      | All four actors, distinguished by `Role` (Customer/Seller/Admin/Courier) |
| `Product`   | Listed by a Seller                                                  |
| `Cart` / `CartItem` | One cart per user, holds items pre-checkout                |
| `Order` / `OrderItem` | Created from the cart at checkout                        |
| `Payment`   | One per order                                                       |
| `Shipment`  | One per order, assigned to a Courier, tracks delivery status        |
| `Review`    | A user's rating/comment on a product                                |

## 1. Prerequisites

- .NET 8 SDK
- MySQL Server running locally (or a remote instance)
- Optional: MySQL Workbench / DBeaver for viewing the database, Postman for testing without Swagger

## 2. Set up the connection string

Open `appsettings.json` and edit the connection string to match your MySQL setup:

```json
"DefaultConnection": "server=localhost;port=3306;database=shopsystem;user=root;password=YOUR_PASSWORD"
```

You don't need to create the `shopsystem` database by hand — EF Core migrations will do that.

## 3. Restore packages

From inside the `ShopSystem.Api` folder:

```bash
dotnet restore
```

## 4. Install the EF Core CLI tool (one-time, if you don't already have it)

```bash
dotnet tool install --global dotnet-ef
```

## 5. Create and apply the migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This generates the `shopsystem` database with all 9 tables from the model above.

## 6. Run the API

```bash
dotnet run
```

The console will print the local URL (something like `http://localhost:5xxx`). Open:

```
http://localhost:5xxx/swagger
```

to get an interactive page for every endpoint — this is the easiest way to demo the
project without building a frontend first.

## 7. Demo flow (matches the assignment's "Sequence Idea")

1. `POST /api/users/register` — create a Seller account, then a Customer account
2. `POST /api/products` — as the Seller, list a product
3. `GET /api/products` — as the Customer, browse products
4. `POST /api/cart/add` — **add to cart**
5. `POST /api/orders/checkout` — **checkout** (turns the cart into an Order)
6. `POST /api/payments/pay` — **payment** (marks the order Paid, auto-creates a Shipment)
7. `PUT /api/shipments/{id}/assign-courier` then `PUT /api/shipments/{id}/status` — **order fulfillment**
8. `GET /api/orders/user/{userId}` or `GET /api/shipments/order/{orderId}` — **track order**
9. `POST /api/reviews` — review the product after delivery

## What's intentionally left simple

This is a starting point, not a finished submission — a few things are simplified so
you can build on top of them instead of fighting a huge boilerplate:

- **No authentication/authorization yet.** `Login`/`Register` just check credentials
  and hash passwords with BCrypt; there's no JWT/session token, so every endpoint is
  currently open. If your rubric wants role restrictions (e.g. only a Seller can post
  products, only a Courier can update shipment status), add `[Authorize(Roles = "...")]`
  once you wire up JWT — that's a natural next step.
- **Payment is simulated.** `PaymentsController.Pay` always succeeds instantly; there's
  no real payment gateway integration.
- **No DTOs/validation layer.** Controllers accept the EF entities directly for
  brevity. For a larger project you'd normally add request/response DTOs so you're not
  exposing your database shape directly over the API.
- **No frontend.** This is API-only. Swagger is enough to demo it, or you can build a
  simple frontend (Blazor, React, plain HTML/JS) that calls these endpoints.

## Extending it further

- Add JWT auth (`Microsoft.AspNetCore.Authentication.JwtBearer`) and lock down
  controllers by role
- Add image upload for products (store files or use a cloud bucket, save the URL)
- Add pagination to `GET /api/products` once you have more sample data
- Add an Admin-only endpoint to view all orders/users across the system
