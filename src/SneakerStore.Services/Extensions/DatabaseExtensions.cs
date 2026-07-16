using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using SneakerStore.Services.Data;
using System.Runtime.CompilerServices;

namespace SneakerStore.Services.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            using var serverConn = dbContext.CreateConnection("ServerConnection");

            await serverConn.OpenAsync();

            const string sql = @"
                    CREATE DATABASE IF NOT EXISTS SneakerStore;
                    USE SneakerStore;
                ";

            using var cmd = new MySqlCommand(sql,serverConn);
            await cmd.ExecuteNonQueryAsync();

            using var conn = dbContext.CreateConnection();
            await conn.OpenAsync();
            

            await InitializeCategoryTableAsync(conn);
            await InitializeBrandsTableAsync(conn);
            await InitializeUsersTableAsync(conn);
            await InitializeSneakersTableAsync(conn);
            await InitializeSneakerImagesTableAsync(conn);
            await InitializeCartTableAsync(conn);
            await InitializeCartItemsTableAsync(conn);
            await InitializeOrdersTableAsync(conn);
            await InitializeOrderItemsTableAsync(conn);
            await InitializeReviewsTableAsync(conn);
            await InitializeWishlistTableAsync(conn);

            Console.WriteLine("Database Initialization Successfully");
        }

        private static async Task ExecuteAsync(MySqlConnection conn, string sql)
        {
            using var cmd = new MySqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task InitializeCategoryTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS Categories
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL UNIQUE,
                        Description TEXT NOT NULL,
                        ImageUrl VARCHAR(500)
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeBrandsTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                        CREATE TABLE IF NOT EXISTS Brands
                        (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL UNIQUE,
                            LogoUrl VARCHAR(1000)
                        );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeUsersTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                        CREATE TABLE IF NOT EXISTS Users
                        (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            FirstName VARCHAR(100) NOT NULL,
                            LastName VARCHAR(100) NOT NULL,
                            Email VARCHAR(150) NOT NULL UNIQUE,
                            PasswordHash VARCHAR(255) NOT NULL,
                            Phone VARCHAR(20) NOT NULL UNIQUE,
                            Role INT NOT NULL,
                            ProfileImage VARCHAR(1000),
                            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                        );
                ";

            await ExecuteAsync(conn, sql);

        }

        public static async Task InitializeSneakersTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                        
                    CREATE TABLE IF NOT EXISTS Sneakers
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL,
                        Description TEXT NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,
                        DiscountPrice DECIMAL(10,2) DEFAULT 0,
                        Stock INT NOT NULL DEFAULT 0,
                        Color VARCHAR(50),
                        Size VARCHAR(50),
                        Gender INT NOT NULL,
                        CategoryId INT NOT NULL,
                        BrandId INT NOT NULL,
                        IsFeatured BOOLEAN DEFAULT FALSE,
                        IsActive BOOLEAN DEFAULT TRUE,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                            ON UPDATE CURRENT_TIMESTAMP,

                        INDEX idx_sneakers_category (CategoryId),
                        INDEX idx_sneakers_brand (BrandId),
                        INDEX idx_sneakers_featured (IsFeatured),
                        INDEX idx_sneakers_active (IsActive),

                        FOREIGN KEY(CategoryId)
                            REFERENCES Categories(Id)
                            ON DELETE RESTRICT
                            ON UPDATE CASCADE,
                        
                        FOREIGN KEY(BrandId)
                            REFERENCES Brands(Id)
                            ON DELETE RESTRICT
                            ON UPDATE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeSneakerImagesTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS SneakerImages
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        SneakerId INT NOT NULL,
                        ImageUrl VARCHAR(1000) NOT NULL,
                        PublicId VARCHAR(255) NOT NULL,
                        IsPrimary BOOLEAN DEFAULT FALSE,

                        INDEX idx_image_sneaker (SneakerId),

                        FOREIGN KEY(SneakerId)
                            REFERENCES Sneakers(Id)
                            ON DELETE RESTRICT
                            ON UPDATE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeCartTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS Cart
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        UserId INT NOT NULL,
                        TotalAmount DECIMAL(10,2) DEFAULT 0,


                        UNIQUE INDEX idx_cart_user (UserId),

                        FOREIGN KEY(UserId)
                            REFERENCES Users(Id)
                            ON DELETE RESTRICT
                            ON UPDATE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeCartItemsTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS CartItems
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        CartId INT NOT NULL,
                        SneakerId INT NOT NULL,
                        Quantity INT NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,

                        INDEX idx_cartitems_cart (CartId),
                        INDEX idx_cartitems_sneaker (SneakerId),
                        UNIQUE INDEX idx_cart_product (CartId, SneakerId),

                        FOREIGN KEY(CartId)
                            REFERENCES Cart(Id)
                            ON DELETE CASCADE,

                        FOREIGN KEY(SneakerId)
                            REFERENCES Sneakers(Id)
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeOrdersTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS Orders
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        UserId INT NOT NULL,
                        TotalAmount DECIMAL(10,2) NOT NULL,
                        OrderStatus INT NOT NULL,
                        PaymentMethod INT NOT NULL,
                        PaymentStatus INT NOT NULL,
                        OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

                        INDEX idx_orders_user (UserId),
                        INDEX idx_orders_status (OrderStatus),
                        INDEX idx_orders_date (OrderDate),

                        FOREIGN KEY(UserId)
                            REFERENCES Users(Id)
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeOrderItemsTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS OrderItems
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        OrderId INT NOT NULL,
                        SneakerId INT NOT NULL,
                        Quantity INT NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,

                        INDEX idx_orderitems_order (OrderId),
                        INDEX idx_orderitems_sneaker (SneakerId),

                        FOREIGN KEY(OrderId)
                            REFERENCES Orders(Id)
                            ON DELETE CASCADE,

                        FOREIGN KEY(SneakerId)
                            REFERENCES Sneakers(Id)
                            ON DELETE RESTRICT
                            ON UPDATE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeReviewsTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS Reviews
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        SneakerId INT NOT NULL,
                        UserId INT NOT NULL,
                        Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
                        Comment TEXT,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,

                        INDEX idx_reviews_sneaker (SneakerId),
                        INDEX idx_reviews_user (UserId),
                        
                        FOREIGN KEY(SneakerId)
                            REFERENCES Sneakers(Id)
                            ON DELETE CASCADE,
                        
                        FOREIGN KEY(UserId)
                            REFERENCES Users(Id)
                            ON DELETE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }

        public static async Task InitializeWishlistTableAsync(MySqlConnection conn)
        {

            const string sql = @"
                    CREATE TABLE IF NOT EXISTS Wishlist
                    (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        UserId INT NOT NULL,
                        SneakerId INT NOT NULL,

                        INDEX idx_wishlist_user (UserId),
                        INDEX idx_wishlist_sneaker (SneakerId),
                        UNIQUE INDEX idx_wishlist_unique (UserId,SneakerId),

                        FOREIGN KEY(UserId)
                            REFERENCES Users(Id)
                            ON DELETE CASCADE,

                        FOREIGN KEY(SneakerId)
                            REFERENCES Sneakers(Id)
                            ON DELETE CASCADE
                    );
                ";

            await ExecuteAsync(conn, sql);
        }
    }


}
