using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ToDoListClassLibrary;

namespace ToDoListUnitTests
{
    [TestClass]
    public class ToDoItemTests
    {
       private static ToDoContext connection;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlite("Data Source=/workspaces/TodoList/TodoList.API/todolist.db;")
                .Options;

            connection = new ToDoContext(options);
            connection.Database.EnsureCreated();
        }


    [TestMethod]
public void Test_SaveNewToDoItemWithEmptyDescription()
{
    SaveToDoItemAndAssertDescription("", "");
}

[TestMethod]
public void Test_SaveNewToDoItemWithNullDescription()
{
    SaveToDoItemAndAssertDescription(null, null);
}

[TestMethod]
public void Test_SaveNewToDoItemWithLongDescription()
{
    string longDescription = "This is a long description for testing purposes. It should exceed the typical length limit.";
    SaveToDoItemAndAssertDescription(longDescription, longDescription);
}

[TestMethod]
public void Test_SaveNewToDoItemWithSpecialCharactersInDescription()
{
    string specialCharsDescription = "Description with special characters: ~!@#$%^&*()_+{}|:\"<>?`-=[]\\;',./";
    SaveToDoItemAndAssertDescription(specialCharsDescription, specialCharsDescription);
}

private void SaveToDoItemAndAssertDescription(string description, string expectedDescription)
{
    // Create a new ToDoItem with the provided description
    var newItem = new ToDoItem { Description = description };

    // Add the new item to the database context
    connection.Add(newItem);

    // Save changes to the database
    connection.SaveChanges();

    // Retrieve the item from the database using LINQ
    var itemFromDb = connection.ToDoItems.FirstOrDefault(item => item.Description == expectedDescription);

    // Assert that the item retrieved from the database is not null
    Assert.IsNotNull(itemFromDb);

    // Assert that the description of the item from the database matches the expected description
    Assert.AreEqual(expectedDescription, itemFromDb.Description);
}
    }

    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }
    }
}