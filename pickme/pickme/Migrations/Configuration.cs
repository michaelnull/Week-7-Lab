namespace pickme.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<pickme.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(pickme.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!(context.Users.Any(u => u.UserName == "joe@nobody.com")))
            {
                var userToInsert = new ApplicationUser { UserName = "joe@nobody.com" };
                userManager.Create(userToInsert, "Abc123!@#");
            }
            if (!(context.Users.Any(u => u.UserName == "fred@nobody.com")))
            {
                var userToInsert = new ApplicationUser { UserName = "fred@nobody.com" };
                userManager.Create(userToInsert, "Abc123!@#");
            }
            if (!(context.Users.Any(u => u.UserName == "tom@nobody.com")))
            {
                var userToInsert = new ApplicationUser { UserName = "tom@nobody.com" };
                userManager.Create(userToInsert, "Abc123!@#");
            }
            
            ApplicationUser poster = context.Users.FirstOrDefault(x => x.UserName == "joe@nobody.com");
            Pick a = new Pick { PostedOn = DateTime.Now, PictureUrl = "http://gallery.photo.net/photo/6099217-lg.jpg", Description = "Sweet lagoon -Balos beach -Crete." , PostedBy = poster  };

            poster = context.Users.FirstOrDefault(x => x.UserName == "fred@nobody.com");
            Pick b = new Pick { PostedOn = DateTime.Now, PictureUrl = "http://gallery.photo.net/photo/3659572-md.jpg", Description = "House on the Hill" , PostedBy = poster};

            poster = context.Users.FirstOrDefault(x => x.UserName == "tom@nobody.com");
            Pick c = new Pick { PostedOn = DateTime.Now, PictureUrl = "http://gallery.photo.net/photo/6821374-md.jpg", Description = "Invasive TV", PostedBy = poster };

            a.Image = a.GetBytes(a.PictureUrl);
            b.Image = b.GetBytes(b.PictureUrl);
            c.Image = b.GetBytes(c.PictureUrl);

            
            context.Picks.AddOrUpdate(a);
            context.Picks.AddOrUpdate(b);
            context.Picks.AddOrUpdate(c);


        }
    }
}
