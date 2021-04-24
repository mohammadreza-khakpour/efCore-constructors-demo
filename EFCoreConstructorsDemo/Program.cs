using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreConstructorsDemo
{
    class Program
    {
        static RezaDataContext db = new RezaDataContext();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //AddBlogsAndPosts();
            var db02 = db.Blogs.Include("Posts");
            Blog theBlog = db02.First(_=>_.Id==1);
            Console.WriteLine(theBlog.ToString());
            Console.WriteLine("Bye Bye World!\n\tenter to exit");
            Console.ReadLine();
        }
        static void AddBlogsAndPosts()
        {
            db.Blogs.AddRange(
                new Blog
                {
                    Title = "blog-01",
                    Posts = new List<Post>
                    {
                        new Post {
                            Content = "blog-01-content-01"
                        },
                        new Post {
                            Content = "blog-01-content-02"
                        }
                    }
                },
                new Blog
                {
                    Title = "blog-02",
                    Posts = new List<Post>
                    {
                        new Post {
                            Content = "blog-02-content-01"
                        },
                        new Post {
                            Content = "blog-02-content-02"
                        }
                    }
                }
            );
            db.SaveChanges();
        }
    }

    class RezaDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=.;initial catalog=efconstructors;integrated security=true");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
    class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Post> Posts { get; set; }
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
        public int Id { get; set; }
        public string Content { get; set; }
        public Blog Blog { get; set; }
        public override string ToString()
        {
            return $"my id is: {Id}, my content is: {Content}";
        }

    }
}
