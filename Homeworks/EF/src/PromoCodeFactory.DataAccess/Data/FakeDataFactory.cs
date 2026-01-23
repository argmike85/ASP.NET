using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static void EnsurePopulated(DataContext context)
        {
            context.Database.Migrate();
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new Role()
                {
                    Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                    Name = "Admin",
                    Description = "Администратор",
                },
                new Role()
                {
                    Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                    Name = "PartnerManager",
                    Description = "Партнерский менеджер"
                });
            }
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(new Employee()
                {
                    Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                    Email = "owner@somemail.ru",
                    FirstName = "Иван",
                    LastName = "Сергеев",
                    Role = context.Roles.FirstOrDefault(x => x.Name == "Admin"),
                    AppliedPromocodesCount = 5
                },
                new Employee()
                {
                    Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                    Email = "andreev@somemail.ru",
                    FirstName = "Петр",
                    LastName = "Андреев",
                    Role = context.Roles.FirstOrDefault(x => x.Name == "PartnerManager"),
                    AppliedPromocodesCount = 10
                });
            }
            if (!context.Preferences.Any())
            {
                context.Preferences.AddRange(
                    new Preference()
                    {
                        Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                        Name = "Театр"
                    },
                    new Preference()
                    {
                        Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                        Name = "Семья"
                    },
                    new Preference()
                    {
                        Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                        Name = "Дети"
                    }
                    );
            }
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                                   
                    new Customer()
                    {
                        Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                    }
                    );
            }
            context.SaveChanges();
            //if (!context.Employees.Any())
            //{
            //    context.Employees.AddRange(

            //                new Emplo()
            //                {
            //                    Name = "mike",
            //                    BirthDate = DateTime.Now.AddDays(-30),
            //                    Email = "argmike85@gmail.com",
            //                    Inn = "1234567890",
            //                    PhoneNumber = "89146404512",
            //                    Orders = new List<Order>
            //        {
            //            new Order(){Name="пряники",},
            //            new Order(){Name="печенье"}
            //      }


            //                },
            //                new Client()
            //                {
            //                    Name = "bob",
            //                    BirthDate = DateTime.Now.AddDays(-60),
            //                    Email = "bob85@gmail.com",
            //                    Inn = "1234567890",
            //                    PhoneNumber = "89146404512",
            //                    Orders = new List<Order>
            //                     {
            //            new Order(){Name="конфеты",},
            //            new Order(){Name="шоколадки"}
            //      }


            //                }

            //    );
            //    context.SaveChanges();
            //}
        }
        
        
    }
}