using System;
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
        public void SelectExtensionWithNoExpressReturnsAll()
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
    }
}
