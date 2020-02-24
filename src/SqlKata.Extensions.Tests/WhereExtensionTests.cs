using FluentAssertions;
using Xunit;

namespace SqlKata.Extensions.Tests
{
    public class WhereExtensionTests
    {
        public class Location
        {
            public long Latitude { get; set; }
            public long Longitude { get; set; }
        }

        [Fact]
        public void WhereExtension_GenericFilter_ReturnsExpected_NoDefaults()
        {
            //Arrange
            var query =
                new Query<Location>()
                    .Select()
                    .Where<Location>(new Location {Latitude = 100});

            var expected =
                new Query("Location")
                    .Select("Latitude", "Longitude")
                    .Where(new {Latitude = 100})
                    .Compile()
                    .Sql;

            //Act 
            var result = query.Compile();
            var actual = result.Sql;

            actual.Should().Be(expected);

        }

        [Fact]
        public void WhereExtension_GenericFilter_ReturnsExpected_WithDefaults()
        {
            //Arrange
            var query =
                new Query<Location>()
                    .Select()
                    .Where(new Location { Latitude = 100 }, true);

            var expected =
                new Query("Location")
                    .Select("Latitude", "Longitude")
                    .Where(new { Latitude = 100, Longitude = 0})
                    .Compile()
                    .Sql;

            //Act 
            var result = query.Compile();
            var actual = result.Sql;

            actual.Should().Be(expected);

        }

        [Fact] 
        public void WhereExtension_ExpressionWhere_ReturnExpected()
        {
            //Arrange
            var query =
                new Query<Location>()
                    .Select()
                    .Where(l => l.Latitude, Operator.NotEqual, 100)
                    .Where(l => l.Longitude, Operator.Eq, 56);

            var expected =
                new Query("Location")
                    .Select("Latitude", "Longitude")
                    .Where("Latitude","<>",100)
                    .Where(new {Longitude = 56 })
                    .Compile();

            //Act 
            var result = query.Compile();
            var actual = result.Sql;

            actual.Should().Be(expected.Sql);
            result.Bindings.Should().BeEquivalentTo(expected.Bindings);
        }


    }
}
