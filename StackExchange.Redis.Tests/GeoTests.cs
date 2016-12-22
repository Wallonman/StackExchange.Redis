using System.Linq;
using System.Runtime.Remoting;
using NUnit.Framework;

namespace StackExchange.Redis.Tests
{
    [TestFixture]
    public class GeoTests : TestBase
    {
        [Test]
        public void GeoAddEveryWay()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var added1 = db.GeoAdd("Sicily", 14.361389, 39.115556, "PalermoPlusOne");
                var geo1 = new GeoEntry(13.361389, 38.115556, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily",new GeoEntry[] {geo1,geo2});
                Assert.IsTrue(added1 & (added2==2));
            }
        }

        [Test]
        public void GetGeoDist()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var geo1 = new GeoEntry(13.361389, 38.115556, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily", new GeoEntry[] { geo1, geo2 });
                var val = db.GeoDistance("Sicily", "Palermo", "Catania",GeoUnit.Meters);
                Assert.AreEqual((int)166274, (int)val);
            }
        }

        [Test]
        public void GeoRadius()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var geo1 = new GeoEntry(13.361389, 38.115556, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily", new GeoEntry[] { geo1, geo2 });
                var val = db.GeoRadius("Sicily",
                    new GeoRadius("Sicily", new GeoPosition(13.361389, 38.115556), 100, -1, GeoUnit.Meters));
                Assert.IsNotEmpty(val);
                Assert.AreEqual(1, val.Length);
            }
        }

        [Test]
        public void GeoRadiusWithMaxReturnCount()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var geo1 = new GeoEntry(13.361389, 38.115556, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily", new GeoEntry[] { geo1, geo2 });
                var val = db.GeoRadius("Sicily",
                    new GeoRadius("Sicily", new GeoPosition(13.361389, 38.115556), 1000, 1, GeoUnit.Kilometers));
                Assert.IsNotEmpty(val);
                Assert.AreEqual(1, val.Length);
            }
        }

        [Test]
        public void GeoRadiusWithCoordinates()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var geo1 = new GeoEntry(13.361389, 38.115556395496299, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily", new GeoEntry[] { geo1, geo2 });
                var val = db.GeoRadius("Sicily",
                    new GeoRadius("Sicily", new GeoPosition(13.361389, 38.115556), 100, -1, GeoUnit.Meters, GeoRadiusOptions.WithCoordinates));
                Assert.IsNotEmpty(val);
                Assert.IsNotNull(val.First().GeoPosition);
                Assert.AreEqual(38.115556395496299, val.First().GeoPosition.Value.Latitude);
            }
        }

        [Test]
        public void GeoRadiusWithDistance()
        {
            using (var conn = Create())
            {
                var db = conn.GetDatabase(3);
                Assert.IsTrue(db.KeyDelete("Sicily"));
                var geo1 = new GeoEntry(13.361389, 38.115556395496299, "Palermo");
                var geo2 = new GeoEntry(15.087269, 37.502669, "Catania");
                var added2 = db.GeoAdd("Sicily", new GeoEntry[] { geo1, geo2 });
                var val = db.GeoRadius("Sicily",
                    new GeoRadius("Sicily", new GeoPosition(13.361389, 38.115556395496299), 100, -1, GeoUnit.Meters, GeoRadiusOptions.WithDistance));
                Assert.IsNotEmpty(val);
                Assert.IsNotNull(val.First().DistanceFromCenter);
                Assert.IsTrue(val.First().DistanceFromCenter.Value < 0.1);
            }
        }

//        [Test]
//        public void AddSetEveryWay()
//        {
//            using (var conn = Create())
//            {
//                var db = conn.GetDatabase(3);
//
//                RedisKey key = Me();
//                db.KeyDelete(key);
//                db.SetAdd(key, "a");
//                db.SetAdd(key, new RedisValue[] { "b" });
//                db.SetAdd(key, new RedisValue[] { "c", "d" });
//                db.SetAdd(key, new RedisValue[] { "e", "f", "g" });
//                db.SetAdd(key, new RedisValue[] { "h", "i", "j", "k" });
//
//                var vals = db.SetMembers(key);
//                string s = string.Join(",", vals.OrderByDescending(x => x));
//                Assert.AreEqual("k,j,i,h,g,f,e,d,c,b,a", s);
//            }
//        }
//
//        [Test]
//        public void AddSetEveryWayNumbers()
//        {
//            using (var conn = Create())
//            {
//                var db = conn.GetDatabase(3);
//
//                RedisKey key = Me();
//                db.KeyDelete(key);
//                db.SetAdd(key, "a");
//                db.SetAdd(key, new RedisValue[] { "1" });
//                db.SetAdd(key, new RedisValue[] { "11", "2" });
//                db.SetAdd(key, new RedisValue[] { "10", "3", "1.5" });
//                db.SetAdd(key, new RedisValue[] { "2.2", "-1", "s", "t" });
//
//                var vals = db.SetMembers(key);
//                string s = string.Join(",", vals.OrderByDescending(x => x));
//                Assert.AreEqual("t,s,a,11,10,3,2.2,2,1.5,1,-1", s);
//            }
//        }
    }
}
