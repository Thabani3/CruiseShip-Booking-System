namespace CruzeShipBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CruzeAppVerbose : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AdminID);
            
            CreateTable(
                "dbo.BookingPackages",
                c => new
                    {
                        BookingPackageId = c.Int(nullable: false, identity: true),
                        BookingTypeId = c.Int(nullable: false),
                        BookingPackageDescription = c.String(),
                        PackagePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackageAcomodationQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingPackageId)
                .ForeignKey("dbo.BookingTypes", t => t.BookingTypeId, cascadeDelete: true)
                .Index(t => t.BookingTypeId);
            
            CreateTable(
                "dbo.BookingTypes",
                c => new
                    {
                        BookingTypeId = c.Int(nullable: false, identity: true),
                        BookingTypeName = c.String(),
                        BookingTypeDescription = c.String(),
                    })
                .PrimaryKey(t => t.BookingTypeId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        CustomerEmail = c.String(),
                        BookingPackageId = c.Int(nullable: false),
                        DateBookingFor = c.DateTime(nullable: false),
                        DateBooked = c.DateTime(nullable: false),
                        BookingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BookingStatus = c.String(),
                        ReferenceNumber = c.String(),
                        Food = c.Boolean(nullable: false),
                        Beverages = c.Boolean(nullable: false),
                        Alcohol = c.Boolean(nullable: false),
                        Decor = c.String(),
                        NumGuest = c.Int(nullable: false),
                        TotalFood = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDrinks = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAcohol = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDecoration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.BookingPackages", t => t.BookingPackageId, cascadeDelete: true)
                .Index(t => t.BookingPackageId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        ContactNumber = c.String(),
                        AlternativeNumber = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageName = c.String(),
                        Description = c.String(),
                        Picture = c.Binary(),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "BookingPackageId", "dbo.BookingPackages");
            DropForeignKey("dbo.BookingPackages", "BookingTypeId", "dbo.BookingTypes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Bookings", new[] { "BookingPackageId" });
            DropIndex("dbo.BookingPackages", new[] { "BookingTypeId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Packages");
            DropTable("dbo.Customers");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookingTypes");
            DropTable("dbo.BookingPackages");
            DropTable("dbo.Admins");
        }
    }
}
