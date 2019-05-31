using Microsoft.EntityFrameworkCore;
using CsharpBelt.Models;
using System.Linq;

namespace CsharpBelt.Models {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Activityy> Activities {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Participant> Participants {get;set;}

        public int Create(User u)
        {
            Users.Add(u);
            SaveChanges();
            return u.UserId;
        }
        
        public User GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int UserId)
        {
            return Users.FirstOrDefault(u => u.UserId == UserId);
        }

        public void Create(Activityy a)
        {
            Add(a);
            SaveChanges();
        }
        public void Create(Participant p)
        {
            Add(p);
            SaveChanges();
        }
        public Activityy GetActivityById(int ActivityId)
        {
            return Activities.Where(a => a.ActivityId == ActivityId).FirstOrDefault();
        }
        public void Remove(int a)
        {
            Remove(GetActivityById(a));
            SaveChanges();
        }
        public void Remove(int actId, int partId)
        {
            Participant Participant = Participants.Where(a => a.ActivityId == actId).Where(p => p.UserId == partId).FirstOrDefault();
            Remove(Participant);
            SaveChanges();
        }
    }
}