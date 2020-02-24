using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace SqlKata.Extensions.Benchmarks
{
    public class ManySelectVsBase
    {
        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        [Benchmark]
        public Query GenericSelectMany() =>
            new Query(nameof(User))
                .Select<User>(
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt,
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt,
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt,
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt,
                    u => u.UserId,
                    u => u.UserName,
                    u => u.CreatedAt);

        [Benchmark(Baseline = true)]
        public Query BaseMany() =>
            new Query("User")
                .Select(
                    "UserId",
                    "UserName",
                    "CreatedAt",
                    "UserId",
                    "UserName",
                    "CreatedAt",
                    "UserId",
                    "UserName",
                    "CreatedAt",
                    "UserId",
                    "UserName",
                    "CreatedAt",
                    "UserId",
                    "UserName",
                    "CreatedAt");
    }
}
