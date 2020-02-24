using System;
using BenchmarkDotNet.Attributes;

namespace SqlKata.Extensions.Benchmarks
{
    public class GenericSelectVsBase
    {
        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        [Benchmark]
        public Query GenericSelect() =>
            new Query(nameof(User))
                .Select<User>(
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt);

        [Benchmark]
        public Query<User> GenericQuery() =>
            new Query<User>()
                .Select(
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt);

        [Benchmark]
        public Query<User> GenericQueryAllProperties() =>
            new Query<User>()
                .Select();

        [Benchmark]
        public Query<User> GenericQueryGenericSelectAllProperties() =>
            new Query<User>()
                .Select<User>();

        [Benchmark(Baseline = true)]
        public Query Base() =>
            new Query("User")
                .Select("UserId", "UserName", "CreatedAt");




    }
}