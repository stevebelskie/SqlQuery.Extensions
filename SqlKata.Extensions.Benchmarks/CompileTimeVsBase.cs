using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using SqlKata.Compilers;

namespace SqlKata.Extensions.Benchmarks
{
    public class CompileTimeVsBase
    {
        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        private SqlServerCompiler _compiler;
        private Query _base;
        private Query<User> _generic;

        [GlobalSetup]
        public void Setup()
        {
            _compiler = new SqlServerCompiler();

            _base = new Query("User")
                    .Select("UserId", "UserName", "CreatedAt");

            _generic =
                new Query<User>()
                    .Select(
                        u => u.UserId,
                        u => u.UserName,
                        u => u.CreatedAt);

        }

        [Benchmark(Baseline = true)]
        public SqlResult BaseCompile()
        {
            return _compiler.Compile(_base);
        }

        [Benchmark]
        public SqlResult GenericCompile()
        {
            return _compiler.Compile(_generic);
        }
    }
}
