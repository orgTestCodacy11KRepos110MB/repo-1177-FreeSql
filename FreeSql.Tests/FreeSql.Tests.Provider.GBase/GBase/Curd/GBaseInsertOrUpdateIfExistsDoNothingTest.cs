using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FreeSql.Tests.GBase
{
    public class GBaseInsertOrUpdateIfExistsDoNothingTest
    {
        IFreeSql fsql => g.gbase;

        [Fact]
        public void InsertOrUpdate_OnlyPrimary()
        {
            fsql.Delete<tbioudb01>().Where("1=1").ExecuteAffrows();
            var iou = fsql.InsertOrUpdate<tbioudb01>().IfExistsDoNothing().SetSource(new tbioudb01 { id = 1 });
            var sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb01 t1 
USING (SELECT 1 as id FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id) 
  values (t2.id)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb01>().IfExistsDoNothing().SetSource(new tbioudb01 { id = 1 });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb01 t1 
USING (SELECT 1 as id FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id) 
  values (t2.id)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb01>().IfExistsDoNothing().SetSource(new tbioudb01 { id = 2 });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb01 t1 
USING (SELECT 2 as id FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id) 
  values (t2.id)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb01>().IfExistsDoNothing().SetSource(new[] { new tbioudb01 { id = 1 }, new tbioudb01 { id = 2 }, new tbioudb01 { id = 3 }, new tbioudb01 { id = 4 } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb01 t1 
USING (SELECT 1 as id FROM dual 
UNION ALL
 SELECT 2 FROM dual 
UNION ALL
 SELECT 3 FROM dual 
UNION ALL
 SELECT 4 FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id) 
  values (t2.id)", sql);
            Assert.Equal(2, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb01>().IfExistsDoNothing().SetSource(new[] { new tbioudb01 { id = 1 }, new tbioudb01 { id = 2 }, new tbioudb01 { id = 3 }, new tbioudb01 { id = 4 } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb01 t1 
USING (SELECT 1 as id FROM dual 
UNION ALL
 SELECT 2 FROM dual 
UNION ALL
 SELECT 3 FROM dual 
UNION ALL
 SELECT 4 FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id) 
  values (t2.id)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());
        }
        class tbioudb01
        {
            public int id { get; set; }
        }

        [Fact]
        public void InsertOrUpdate_OnePrimary()
        {
            fsql.Delete<tbioudb02>().Where("1=1").ExecuteAffrows();
            var iou = fsql.InsertOrUpdate<tbioudb02>().IfExistsDoNothing().SetSource(new tbioudb02 { id = 1, name = "01" });
            var sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb02 t1 
USING (SELECT 1 as id, '01' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb02>().IfExistsDoNothing().SetSource(new tbioudb02 { id = 1, name = "011" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb02 t1 
USING (SELECT 1 as id, '011' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb02>().IfExistsDoNothing().SetSource(new tbioudb02 { id = 2, name = "02" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb02 t1 
USING (SELECT 2 as id, '02' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb02>().IfExistsDoNothing().SetSource(new[] { new tbioudb02 { id = 1, name = "01" }, new tbioudb02 { id = 2, name = "02" }, new tbioudb02 { id = 3, name = "03" }, new tbioudb02 { id = 4, name = "04" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb02 t1 
USING (SELECT 1 as id, '01' as name FROM dual 
UNION ALL
 SELECT 2, '02' FROM dual 
UNION ALL
 SELECT 3, '03' FROM dual 
UNION ALL
 SELECT 4, '04' FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(2, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb02>().IfExistsDoNothing().SetSource(new[] { new tbioudb02 { id = 1, name = "001" }, new tbioudb02 { id = 2, name = "002" }, new tbioudb02 { id = 3, name = "003" }, new tbioudb02 { id = 4, name = "004" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb02 t1 
USING (SELECT 1 as id, '001' as name FROM dual 
UNION ALL
 SELECT 2, '002' FROM dual 
UNION ALL
 SELECT 3, '003' FROM dual 
UNION ALL
 SELECT 4, '004' FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());
            var lst = fsql.Select<tbioudb02>().Where(a => new[] { 1, 2, 3, 4 }.Contains(a.id)).ToList();
            Assert.Equal(4, lst.Where(a => a.name == "0" + a.id).Count());
        }
        class tbioudb02
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        [Fact]
        public void InsertOrUpdate_OnePrimaryAndIdentity()
        {
            fsql.Delete<tbioudb022>().Where("1=1").ExecuteAffrows();
            var iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { id = 1, name = "01" });
            var sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 1 as id, '01' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { id = 1, name = "011" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 1 as id, '011' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { id = 2, name = "02" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 2 as id, '02' as name FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new[] { new tbioudb022 { id = 1, name = "01" }, new tbioudb022 { id = 2, name = "02" }, new tbioudb022 { id = 3, name = "03" }, new tbioudb022 { id = 4, name = "04" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 1 as id, '01' as name FROM dual 
UNION ALL
 SELECT 2, '02' FROM dual 
UNION ALL
 SELECT 3, '03' FROM dual 
UNION ALL
 SELECT 4, '04' FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(2, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new[] { new tbioudb022 { id = 1, name = "001" }, new tbioudb022 { id = 2, name = "002" }, new tbioudb022 { id = 3, name = "003" }, new tbioudb022 { id = 4, name = "004" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 1 as id, '001' as name FROM dual 
UNION ALL
 SELECT 2, '002' FROM dual 
UNION ALL
 SELECT 3, '003' FROM dual 
UNION ALL
 SELECT 4, '004' FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());
            var lst = fsql.Select<tbioudb022>().Where(a => new[] { 1, 2, 3, 4 }.Contains(a.id)).ToList();
            //Assert.Equal(4, lst.Where(a => a.name == "00" + a.id).Count());

            //--no primary
            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { name = "01" });
            sql = iou.ToSql();
            Assert.Equal(@"INSERT INTO tbioudb022(name) VALUES('01')", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { name = "011" });
            sql = iou.ToSql();
            Assert.Equal(@"INSERT INTO tbioudb022(name) VALUES('011')", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new tbioudb022 { name = "02" });
            sql = iou.ToSql();
            Assert.Equal(@"INSERT INTO tbioudb022(name) VALUES('02')", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new[] { new tbioudb022 { name = "01" }, new tbioudb022 { name = "02" }, new tbioudb022 { name = "03" }, new tbioudb022 { name = "04" } });
            sql = iou.ToSql();
            Assert.Equal(@"INSERT INTO tbioudb022(name)
SELECT * FROM (
 SELECT '01' FROM dual 
UNION ALL
 SELECT '02' FROM dual 
UNION ALL
 SELECT '03' FROM dual 
UNION ALL
 SELECT '04' FROM dual
) ftbtmp", sql);
            Assert.Equal(4, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new[] { new tbioudb022 { name = "001" }, new tbioudb022 { name = "002" }, new tbioudb022 { name = "003" }, new tbioudb022 { name = "004" } });
            sql = iou.ToSql();
            Assert.Equal(@"INSERT INTO tbioudb022(name)
SELECT * FROM (
 SELECT '001' FROM dual 
UNION ALL
 SELECT '002' FROM dual 
UNION ALL
 SELECT '003' FROM dual 
UNION ALL
 SELECT '004' FROM dual
) ftbtmp", sql);
            Assert.Equal(4, iou.ExecuteAffrows());

            //--no primary and yes
            iou = fsql.InsertOrUpdate<tbioudb022>().IfExistsDoNothing().SetSource(new[] { new tbioudb022 { id = 1, name = "100001" }, new tbioudb022 { name = "00001" }, new tbioudb022 { id = 2, name = "100002" }, new tbioudb022 { name = "00002" }, new tbioudb022 { id = 3, name = "100003" }, new tbioudb022 { name = "00003" }, new tbioudb022 { id = 4, name = "100004" }, new tbioudb022 { name = "00004" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb022 t1 
USING (SELECT 1 as id, '100001' as name FROM dual 
UNION ALL
 SELECT 2, '100002' FROM dual 
UNION ALL
 SELECT 3, '100003' FROM dual 
UNION ALL
 SELECT 4, '100004' FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name) 
  values (t2.id, t2.name)

;

INSERT INTO tbioudb022(name)
SELECT * FROM (
 SELECT '00001' FROM dual 
UNION ALL
 SELECT '00002' FROM dual 
UNION ALL
 SELECT '00003' FROM dual 
UNION ALL
 SELECT '00004' FROM dual
) ftbtmp", sql);
            Assert.Equal(4, iou.ExecuteAffrows());
            lst = fsql.Select<tbioudb022>().Where(a => new[] { 1, 2, 3, 4 }.Contains(a.id)).ToList();
            //Assert.Equal(4, lst.Where(a => a.name == "10000" + a.id).Count());
        }
        class tbioudb022
        {
            [Column(IsIdentity = true)]
            public int id { get; set; }
            public string name { get; set; }
        }

        [Fact]
        public void InsertOrUpdate_TwoPrimary()
        {
            fsql.Delete<tbioudb03>().Where("1=1").ExecuteAffrows();
            var iou = fsql.InsertOrUpdate<tbioudb03>().IfExistsDoNothing().SetSource(new tbioudb03 { id1 = 1, id2 = "01", name = "01" });
            var sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb03 t1 
USING (SELECT 1 as id1, '01' as id2, '01' as name FROM dual ) t2 ON (t1.id1 = t2.id1 AND t1.id2 = t2.id2) 
WHEN NOT MATCHED THEN 
  insert (id1, id2, name) 
  values (t2.id1, t2.id2, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb03>().IfExistsDoNothing().SetSource(new tbioudb03 { id1 = 1, id2 = "01", name = "011" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb03 t1 
USING (SELECT 1 as id1, '01' as id2, '011' as name FROM dual ) t2 ON (t1.id1 = t2.id1 AND t1.id2 = t2.id2) 
WHEN NOT MATCHED THEN 
  insert (id1, id2, name) 
  values (t2.id1, t2.id2, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb03>().IfExistsDoNothing().SetSource(new tbioudb03 { id1 = 2, id2 = "02", name = "02" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb03 t1 
USING (SELECT 2 as id1, '02' as id2, '02' as name FROM dual ) t2 ON (t1.id1 = t2.id1 AND t1.id2 = t2.id2) 
WHEN NOT MATCHED THEN 
  insert (id1, id2, name) 
  values (t2.id1, t2.id2, t2.name)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb03>().IfExistsDoNothing().SetSource(new[] { new tbioudb03 { id1 = 1, id2 = "01", name = "01" }, new tbioudb03 { id1 = 2, id2 = "02", name = "02" }, new tbioudb03 { id1 = 3, id2 = "03", name = "03" }, new tbioudb03 { id1 = 4, id2 = "04", name = "04" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb03 t1 
USING (SELECT 1 as id1, '01' as id2, '01' as name FROM dual 
UNION ALL
 SELECT 2, '02', '02' FROM dual 
UNION ALL
 SELECT 3, '03', '03' FROM dual 
UNION ALL
 SELECT 4, '04', '04' FROM dual ) t2 ON (t1.id1 = t2.id1 AND t1.id2 = t2.id2) 
WHEN NOT MATCHED THEN 
  insert (id1, id2, name) 
  values (t2.id1, t2.id2, t2.name)", sql);
            Assert.Equal(2, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb03>().IfExistsDoNothing().SetSource(new[] { new tbioudb03 { id1 = 1, id2 = "01", name = "001" }, new tbioudb03 { id1 = 2, id2 = "02", name = "002" }, new tbioudb03 { id1 = 3, id2 = "03", name = "003" }, new tbioudb03 { id1 = 4, id2 = "04", name = "004" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb03 t1 
USING (SELECT 1 as id1, '01' as id2, '001' as name FROM dual 
UNION ALL
 SELECT 2, '02', '002' FROM dual 
UNION ALL
 SELECT 3, '03', '003' FROM dual 
UNION ALL
 SELECT 4, '04', '004' FROM dual ) t2 ON (t1.id1 = t2.id1 AND t1.id2 = t2.id2) 
WHEN NOT MATCHED THEN 
  insert (id1, id2, name) 
  values (t2.id1, t2.id2, t2.name)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());
            var lst = fsql.Select<tbioudb03>().Where(a => a.id1 == 1 && a.id2 == "01" || a.id1 == 2 && a.id2 == "02" || a.id1 == 3 && a.id2 == "03" || a.id1 == 4 && a.id2 == "04").ToList();
            Assert.Equal(4, lst.Where(a => a.name == "0" + a.id1).Count());
        }
        class tbioudb03
        {
            [Column(IsPrimary = true)]
            public int id1 { get; set; }
            [Column(IsPrimary = true)]
            public string id2 { get; set; }
            public string name { get; set; }
        }

        [Fact]
        public void InsertOrUpdate_OnePrimaryAndVersionAndCanUpdate()
        {
            fsql.Delete<tbioudb04>().Where("1=1").ExecuteAffrows();
            var iou = fsql.InsertOrUpdate<tbioudb04>().IfExistsDoNothing().SetSource(new tbioudb04 { id = 1, name = "01" });
            var sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb04 t1 
USING (SELECT 1 as id, '01' as name, 0 as version, current as CreateTime FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name, version, CreateTime) 
  values (t2.id, t2.name, t2.version, t2.CreateTime)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb04>().IfExistsDoNothing().SetSource(new tbioudb04 { id = 1, name = "011" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb04 t1 
USING (SELECT 1 as id, '011' as name, 0 as version, current as CreateTime FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name, version, CreateTime) 
  values (t2.id, t2.name, t2.version, t2.CreateTime)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb04>().IfExistsDoNothing().SetSource(new tbioudb04 { id = 2, name = "02" });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb04 t1 
USING (SELECT 2 as id, '02' as name, 0 as version, current as CreateTime FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name, version, CreateTime) 
  values (t2.id, t2.name, t2.version, t2.CreateTime)", sql);
            Assert.Equal(1, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb04>().IfExistsDoNothing().SetSource(new[] { new tbioudb04 { id = 1, name = "01" }, new tbioudb04 { id = 2, name = "02" }, new tbioudb04 { id = 3, name = "03" }, new tbioudb04 { id = 4, name = "04" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb04 t1 
USING (SELECT 1 as id, '01' as name, 0 as version, current as CreateTime FROM dual 
UNION ALL
 SELECT 2, '02', 0, current FROM dual 
UNION ALL
 SELECT 3, '03', 0, current FROM dual 
UNION ALL
 SELECT 4, '04', 0, current FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name, version, CreateTime) 
  values (t2.id, t2.name, t2.version, t2.CreateTime)", sql);
            Assert.Equal(2, iou.ExecuteAffrows());

            iou = fsql.InsertOrUpdate<tbioudb04>().IfExistsDoNothing().SetSource(new[] { new tbioudb04 { id = 1, name = "001" }, new tbioudb04 { id = 2, name = "002" }, new tbioudb04 { id = 3, name = "003" }, new tbioudb04 { id = 4, name = "004" } });
            sql = iou.ToSql();
            Assert.Equal(@"MERGE INTO tbioudb04 t1 
USING (SELECT 1 as id, '001' as name, 0 as version, current as CreateTime FROM dual 
UNION ALL
 SELECT 2, '002', 0, current FROM dual 
UNION ALL
 SELECT 3, '003', 0, current FROM dual 
UNION ALL
 SELECT 4, '004', 0, current FROM dual ) t2 ON (t1.id = t2.id) 
WHEN NOT MATCHED THEN 
  insert (id, name, version, CreateTime) 
  values (t2.id, t2.name, t2.version, t2.CreateTime)", sql);
            Assert.Equal(0, iou.ExecuteAffrows());
            var lst = fsql.Select<tbioudb04>().Where(a => new[] { 1, 2, 3, 4 }.Contains(a.id)).ToList();
            Assert.Equal(4, lst.Where(a => a.name == "0" + a.id).Count());
        }
        class tbioudb04
        {
            public int id { get; set; }
            public string name { get; set; }
            [Column(IsVersion = true)]
            public int version { get; set; }
            [Column(CanUpdate = false, ServerTime = DateTimeKind.Local)]
            public DateTime CreateTime { get; set; }
        }
    }
}
