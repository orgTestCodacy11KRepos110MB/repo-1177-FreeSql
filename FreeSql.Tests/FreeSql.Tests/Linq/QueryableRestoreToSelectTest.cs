﻿using FreeSql.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FreeSql.Tests.Linq
{
    public class QueryableRestoreToSelectTest
    {
        class qt01
        {
            [Column(IsIdentity = true)]
            public int id { get; set; }
            public string name { get; set; }

            [Navigate(nameof(qt01_item.qt01id))]
            public List<qt01_item> items { get; set; }
        }
        class qt01_item
        {
            [Column(IsIdentity = true)]
            public int id { get; set; }
            public string title { get; set; }
            public int qt01id { get; set; }
        }
        IFreeSql fsql => g.sqlite;

        [Fact]
        public void RestoreToSelect()
        {
            fsql.Insert(new qt01[] { new qt01 { name = "001" }, new qt01 { name = "001" } }).ExecuteAffrows();
            Assert.Equal(fsql.Select<qt01>().Skip(2).First(a => a.name), fsql.Select<qt01>().AsQueryable().Skip(2).Take(1).RestoreToSelect().First(a => a.name));
            Assert.Equal(fsql.Select<qt01>().Skip(2).First(a => new { a.name }).name, fsql.Select<qt01>().AsQueryable().Skip(2).Take(1).RestoreToSelect().First(a => new { a.name }).name);
        }
    }

}
