using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreConstructorsDemo02
{
    class Program
    {
        static RezaDataContext db = new RezaDataContext();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AddBlogsAndPosts();
            var db02 = db.Blogs.Include("Posts");
            Blog theBlog = db02.First(_ => _.Id == 2);
            Console.WriteLine(theBlog.ToString());
            Console.WriteLine("Bye Bye World!\n\tenter to exit");
            Console.ReadLine();
        }
        static void AddBlogsAndPosts()
        {
            Post post01 = new Post(12, "blog-40-content-01");
            Post post02 = new Post(13, "blog-40-content-02");
            Post post03 = new Post(14, "blog-50-content-01");
            Post post04 = new Post(15, "blog-50-content-02");
            Blog blog01 = new Blog() { Title = "blog-40" };
            blog01.Posts.Add(post01);
            blog01.Posts.Add(post02);
            Blog blog02 = new Blog() { Title = "blog-50" };
            blog02.Posts.Add(post03);
            blog02.Posts.Add(post04);

            db.Blogs.Add(blog01);
            db.Blogs.Add(blog02);

            // to get able to insert id explicitly for posts
            // codes achieved from: https://entityframeworkcore.com/saving-data-identity-insert
            db.Database.OpenConnection();
            db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts ON");
            db.SaveChanges();
            db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts OFF");
        }
    }
    class RezaDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=.;initial catalog=secondEfconstructors;integrated security=true");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
    class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"i'm a blog. my id is: {Id}, my title is: {Title} and my posts are:");
            foreach (var item in this.Posts)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();

        }
    }
    class Post
    {
        // it is cruical to have parameter and property with same name !!!WTF
        public Post(int id, string content)
        {
            Id = id;
            Content = content;
        }
        public int Id { get; set; }
        public string Content { get; set; }
        public Blog Blog { get; set; }
        public override string ToString()
        {
            return $"my id is: {Id}, my content is: {Content}";
        }

    }
}
