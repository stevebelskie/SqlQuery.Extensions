using System;
using System.ComponentModel.DataAnnotations.Schema;
using FluentAssertions;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Extensions;
using Xunit;

namespace SqlKata.Extensions.Tests
{
    public static class Helper
    {
        private static readonly SqlServerCompiler Compiler = new SqlServerCompiler();
        public static SqlResult Compile(this Query query) => Compiler.Compile(query);
        public static SqlResult Compile<T>(this Query<T> query) => Compiler.Compile(query);

    }
    public class SelectExtensionTests
    {
        
        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        [Fact]
        public void SelectExtensionWithExpressionReturnsExpected()
        {
            //Arrange
            var query = new Query(nameof(User))
                .Select<User>(
                    u => u.UserId,
                    u => u.UserName);

            var expected = 
                new Query("User")
                    .Select("UserId","UserName")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SelectExtensionWithNoExpressionReturnsAll_StandardProperties()
        {
            //Arrange
            var query = new Query(nameof(User)).Select<User>();

            var expected =
                new Query("User")
                    .Select("UserId", "UserName", "CreatedAt")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        public class PersonNotMapped
        {
            [NotMapped] 
            public string FullName
            {
                get => FirstName + " " + LastName;
                set { }
            }

            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [Fact]
        public void SelectExtension_WithNoExpression_ReturnsAllExcept_NotMapped()
        {
            //Arrange - Person class with FullName Property with NotMappedAttribute
            var query = new Query("Person").Select<PersonNotMapped>();

            var expected =
                new Query("Person")
                    .Select("UserId", "FirstName", "LastName")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        public class PersonComputed
        {
            
            public string FullName => FirstName + " " + LastName;
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [Fact]
        public void SelectExtension_WithNoExpression_ReturnsAllExcept_Computed()
        {
            //Arrange - Person class with FullName Property with no setter (i.e. computed based on other values).
            var query = new Query("Person").Select<PersonComputed>();

            var expected =
                new Query("Person")
                    .Select("UserId", "FirstName", "LastName")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SelectExtension_WithGenericQuery_ReturnsExpected()
        {
            //Arrange
            var query = new Query<User>()
                .Select(
                    u => u.UserId,
                    u => u.UserName);

            var expected =
                new Query("User")
                    .Select("UserId", "UserName")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SelectExtension_WithGenericQuery_AndNoExpressions_ReturnsAll()
        {
            //Arrange
            var query = new Query<User>().Select();

            var expected =
                new Query("User")
                    .Select("UserId", "UserName", "CreatedAt")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SelectExtension_WithGenericQueryAndGenericSelect_AndNoExpressions_ReturnsAll()
        {
            //Arrange
            var query = new Query<User>().Select<User>();

            var expected =
                new Query("User")
                    .Select("UserId", "UserName", "CreatedAt")
                    .Compile()
                    .Sql;

            //Act
            var result = query.Compile();
            var actual = result.Sql;

            //Assert
            actual.Should().Be(expected);
        }
    }
}
