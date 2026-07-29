# E-Commerce Platform

A full-stack e-commerce system where users can browse products, add them to a
cart, and place orders. Includes an admin panel for managing products and
orders.

## Tech Stack

Built as a coursework project that required building an API from scratch:
**ASP.NET Core Web API** on the backend, **Entity Framework Core** for data
access, and **JWT** for authentication. Endpoints were tested with Swagger
throughout development.

The frontend uses **ASP.NET Core MVC** with **Bootstrap**.

## Architecture

### Backend

- Domain models: Users, Products, Categories, CartItems, Orders, OrderItems
- Relationships and tables defined via `DbContext`
- Controllers: Auth, Products, Cart, Orders, Categories
- Business logic isolated in a service layer behind interfaces
- JWT-based authentication
- CORS configured so the frontend can reach the API

### Frontend

- Product listing, search, and category filtering (MVC)
- Product detail, cart, and order pages
- Admin panel for product and order management
- A dedicated service class handles all API calls from the frontend

### Database

Six tables: Users, Categories, Products, CartItems, Orders, OrderItems, with
foreign-key relationships (category–product, user–cart, order–order items).

## How it was built

Backend first — models, migrations, and the API — then the MVC frontend wired
up to it.

The two things that took the most iteration: CORS configuration, and where to
store the JWT on the frontend (ended up using session storage).

## How it works

The user logs in and receives a token, which is attached to every subsequent
request. From there they can browse products, view details, add items to
the cart, and place orders. Admins get a separate panel for managing products
and orders.

## Future Improvements

- Refresh tokens instead of a single long-lived JWT
- Unit tests for the service layer
- Pagination on the product listing endpoint
