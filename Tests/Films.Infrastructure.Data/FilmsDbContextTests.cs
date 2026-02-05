using System;
using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Films.Infrastructure.Data.Tests
{
    public class FilmsDbContextTests
    {
        private readonly DbContextOptions<FilmsDbContext> _options;

        public FilmsDbContextTests()
        {
            _options = new DbContextOptionsBuilder<FilmsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void DbContext_HasAllRequiredDbSets()
        {
            // Arrange & Act
            using var context = new FilmsDbContext(_options);

            // Assert
            Assert.NotNull(context.Actors);
            Assert.NotNull(context.DirectedBys);
            Assert.NotNull(context.Films);
            Assert.NotNull(context.RefAFs);
            Assert.NotNull(context.RefDAFs);
            Assert.NotNull(context.Rights);
            Assert.NotNull(context.Sexes);
            Assert.NotNull(context.TypeUsers);
            Assert.NotNull(context.Users);
            Assert.NotNull(context.UserRights);
        }

        [Fact]
        public void CanAddAndRetrieveActor()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var actor = new Actor
            {
                FirstName = "Tom",
                LastName = "Hanks",
                DateOfBirth = new DateTime(1956, 7, 9)
            };

            // Act
            context.Actors.Add(actor);
            context.SaveChanges();

            // Assert
            using var newContext = new FilmsDbContext(_options);
            var retrievedActor = newContext.Actors.Find(actor.Id);
            Assert.NotNull(retrievedActor);
            Assert.Equal("Tom", retrievedActor.FirstName);
            Assert.Equal("Hanks", retrievedActor.LastName);
            Assert.Equal(new DateTime(1956, 7, 9), retrievedActor.DateOfBirth);
        }

        [Fact]
        public void CanAddAndRetrieveFilm()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var film = new Film
            {
                Name = "Inception",
                Year = 2010,
                Description = "A thief who steals corporate secrets"
            };

            // Act
            context.Films.Add(film);
            context.SaveChanges();

            // Assert
            using var newContext = new FilmsDbContext(_options);
            var retrievedFilm = newContext.Films.Find(film.Id);
            Assert.NotNull(retrievedFilm);
            Assert.Equal("Inception", retrievedFilm.Name);
            Assert.Equal(2010, retrievedFilm.Year);
            Assert.Equal("A thief who steals corporate secrets", retrievedFilm.Description);
        }

        [Fact]
        public void CanAddAndRetrieveDirectedBy()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var director = new DirectedBy
            {
                Name = "Christopher Nolan"
            };

            // Act
            context.DirectedBys.Add(director);
            context.SaveChanges();

            // Assert
            using var newContext = new FilmsDbContext(_options);
            var retrievedDirector = newContext.DirectedBys.Find(director.Id);
            Assert.NotNull(retrievedDirector);
            Assert.Equal("Christopher Nolan", retrievedDirector.Name);
        }

        [Fact]
        public void CanAddAndRetrieveUserWithRelationships()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var typeUser = new TypeUser { Name = "Admin" };
            context.TypeUsers.Add(typeUser);
            context.SaveChanges();

            var user = new User
            {
                Username = "admin",
                Password = "password",
                Email = "admin@example.com",
                TypeUserId = typeUser.Id
            };

            // Act
            context.Users.Add(user);
            context.SaveChanges();

            // Assert
            using var newContext = new FilmsDbContext(_options);
            var retrievedUser = newContext.Users
                .Include(u => u.TypeUser)
                .FirstOrDefault(u => u.Id == user.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal("admin", retrievedUser.Username);
            Assert.Equal("password", retrievedUser.Password);
            Assert.Equal("admin@example.com", retrievedUser.Email);
            Assert.NotNull(retrievedUser.TypeUser);
            Assert.Equal("Admin", retrievedUser.TypeUser.Name);
        }

        [Fact]
        public void CanAddAndRetrieveActorFilmRelationship()
        {
            // Arrange
            using var context = new FilmsDbContext(_options);
            var actor = new Actor { FirstName = "Tom", LastName = "Hanks" };
            var film = new Film { Name = "Forrest Gump", Year = 1994 };

            context.Actors.Add(actor);
            context.Films.Add(film);
            context.SaveChanges();

            var refAF = new RefAF
            {
                ActorId = actor.Id,
                FilmId = film.Id
            };

            // Act
            context.RefAFs.Add(refAF);
            context.SaveChanges();

            // Assert
            using var newContext = new FilmsDbContext(_options);
            var retrievedRef = newContext.RefAFs
                .Include(r => r.Actor)
                .Include(r => r.Film)
                .FirstOrDefault(r => r.Id == refAF.Id);

            Assert.NotNull(retrievedRef);
            Assert.Equal(actor.Id, retrievedRef.ActorId);
            Assert.Equal(film.Id, retrievedRef.FilmId);
            Assert.NotNull(retrievedRef.Actor);
            Assert.NotNull(retrievedRef.Film);
            Assert.Equal("Tom", retrievedRef.Actor.FirstName);
            Assert.Equal("Hanks", retrievedRef.Actor.LastName);
            Assert.Equal("Forrest Gump", retrievedRef.Film.Name);
            Assert.Equal(1994, retrievedRef.Film.Year);
        }
    }
}