#if UNITY_EDITOR
namespace Termway.Tests
{
    using Helper;

    using NUnit.Framework;

    using System;

    public class StatsTest
    {
        [Test]
        public void Stats_ZeroPopulationSize()
        {
            Assert.Throws<ArgumentException>(() => new Stats(0));
        }

        [Test]
        public void Stats_MaxPopulationSize()
        {
            Assert.Throws<OverflowException>(() => new Stats(uint.MaxValue));
        }


        [Test]
        public void Stats_AddNextSameValue()
        {
            Stats stats = new Stats(10);
            for(int i = 0; i < 11; i++)
                stats.AddNext(1);
        }

        [Test]
        public void Stats_PopulationSize()
        {
            for (uint i = 1; i < 100; i += 10)
            {
                Stats stats = new Stats(i);
                Assert.AreEqual(i, stats.PopulationSize, "i:" + i);
                for(uint j = 0; j < i; j++)
                    stats.AddNext(j);
                Assert.AreEqual(i, stats.PopulationSize, "i:" + i);
            }
        }

        [Test]
        public void Stats_CurrentIndex()
        {
            for (uint i = 1; i < 100; i += 10)
            {
                Stats stats = new Stats(i);
                for (uint j = 0; j < i; j++)
                {
                    Assert.AreEqual(j, stats.CurrentIndex, "i:" + i + " j:" + j);
                    stats.AddNext(j);
                }
                Assert.Zero(stats.CurrentIndex, "i:" + i);
            }
        }

        [Test]
        public void Stats_TotalIteration()
        {
            for (uint i = 1; i < 100; i += 10)
            {
                Stats stats = new Stats(i);
                for (uint j = 0; j < i * 10; j++)
                {
                    Assert.AreEqual(j / i, stats.TotalIteration, "i:" + i + " j:" + j);
                    stats.AddNext(j);
                }
                Assert.AreEqual(10, stats.TotalIteration, "i:" + i);
            }
        }

        [Test]
        public void Stats_IsPopulated()
        {
            for (uint i = 1; i < 100; i += 10)
            {
                Stats stats = new Stats(i);
                for (uint j = 0; j < i; j++)
                {
                    Assert.IsFalse(stats.IsPopulated, "i:" + i + " j:" + j);
                    stats.AddNext(j);
                }
                Assert.IsTrue(stats.IsPopulated, "i:" + i);
            }
        }

        [Test]
        public void Stats_LastAdded()
        {
            Stats stats = new Stats(100);
            Assert.Zero(stats.LastAdded);

            for (int i = 0; i < 100; i++)
            {
                stats.AddNext(i);
                Assert.AreEqual(i, stats.LastAdded, "i:" + i);
            }
        }

        [Test]
        public void Stats_Last_InvalidRange()
        {
            Stats stats = new Stats(1);
            Assert.Throws<ArgumentException>(() => stats.Last(1));
            stats.AddNext(10);
            Assert.Throws<ArgumentException>(() => stats.Last(1));
            Assert.Throws<ArgumentException>(() => stats.Last(2));

            stats = new Stats(1000);
            Assert.Throws<ArgumentException>(() => stats.Last(1000));
            stats.AddNext(10);
            Assert.Throws<ArgumentException>(() => stats.Last(1000));
        }

        [Test]
        public void Stats_Last()
        {
            Stats stats = new Stats(1);
            Assert.Zero(stats.Last());

            stats = new Stats(10);
            Assert.Zero(stats.Last());

            for (int i = 0; i < 10; i++)
            {
                stats.AddNext(i);
                for(uint j = 0; j < i; j++)
                    Assert.AreEqual(i - j, stats.Last(j), "i:" + i + " j:" + j);
            }
        }


        [Test]
        public void Stats_Min_OneElement()
        {
            Stats stats = new Stats(1);
            Assert.Zero(stats.Min);

            stats.AddNext(10);
            Assert.AreEqual(10, stats.Min);
        }

        [Test]
        public void Stats_Max_OneElement()
        {
            Stats stats = new Stats(1);
            Assert.Zero(stats.Max);

            stats.AddNext(10);
            Assert.AreEqual(10, stats.Max);
        }
        
        [Test]
        public void Stats_Min_HundredElements()
        {
            Stats stats = new Stats(100);
            Assert.Zero(stats.Min);

            for (int i = 0; i < 100; i++)
            {
                stats.AddNext(i);
                Assert.Zero(stats.Min, "i:" + i);
            }
        }

        [Test]
        public void Stats_Max_HundredElements()
        {
            Stats stats = new Stats(100);
            Assert.Zero(stats.Max);

            for (int i = 0; i < 100; i++)
            {
                stats.AddNext(i);
                Assert.AreEqual(i, stats.Max, "i:" + i);
            }
        }

        [Test]
        public void Stats_Average_OneElementNotPopulated()
        {
            Stats stats = new Stats(1);
            Assert.Zero(stats.Average());
            Assert.Zero(stats.Average(1));
            Assert.Zero(stats.Average(2));
        }

        [Test]
        public void Stats_Average_OneElementPopulated()
        {
            Stats stats = new Stats(1);
            stats.AddNext(10);
            Assert.AreEqual(10, stats.Average());
            for (uint i = 0; i < 3; i++)
                Assert.AreEqual(10, stats.Average(i), "i:" + i);

            stats.AddNext(20);
            Assert.AreEqual(20, stats.Average());
            for (uint i = 0; i < 3; i++)
                Assert.AreEqual(20, stats.Average(i), "i:" + i);
        }

        [Test]
        public void Stats_Average_TwoElementsNotPopulated()
        {
            Stats stats = new Stats(3);
            Assert.Zero(stats.Average());
            for (uint i = 0; i < 4; i++)
                Assert.Zero(stats.Average(i), "i:" + i);

            stats.AddNext(10); 
            Assert.AreEqual(10, stats.Average());
            for (uint i = 0; i < 4; i++)
                Assert.AreEqual(10, stats.Average(i), "i:" + i);

            stats.AddNext(20);
            Assert.AreEqual(15, stats.Average());
            Assert.AreEqual(15, stats.Average(0));
            Assert.AreEqual(20, stats.Average(1));
            Assert.AreEqual(15, stats.Average(2));
            Assert.AreEqual(15, stats.Average(3));
            Assert.AreEqual(15, stats.Average(4));
        }

        [Test]
        public void Stats_Average_TwoElementsPopulated()
        {
            Stats stats = new Stats(3);
            stats.AddNext(10); 
            stats.AddNext(20); 
            stats.AddNext(30); 
            Assert.AreEqual(20, stats.Average());
            Assert.AreEqual(20, stats.Average(0));
            Assert.AreEqual(30, stats.Average(1));
            Assert.AreEqual(25, stats.Average(2));
            Assert.AreEqual(20, stats.Average(3));
            Assert.AreEqual(20, stats.Average(4));

            stats.AddNext(40);
            Assert.AreEqual(30, stats.Average());
            Assert.AreEqual(30, stats.Average(0));
            Assert.AreEqual(40, stats.Average(1));
            Assert.AreEqual(35, stats.Average(2));
            Assert.AreEqual(30, stats.Average(3));
            Assert.AreEqual(30, stats.Average(4));

            stats.AddNext(50);
            Assert.AreEqual(40, stats.Average());
            Assert.AreEqual(40, stats.Average(0));
            Assert.AreEqual(50, stats.Average(1));
            Assert.AreEqual(45, stats.Average(2));
            Assert.AreEqual(40, stats.Average(3));
            Assert.AreEqual(40, stats.Average(4));
        }

        [Test]
        public void Stats_Average_ThousandElements()
        {
            Stats stats = new Stats(1000);
            float average = 0;
            for (int i = 0; i < 1000; i++)
            {
                stats.AddNext(i);
                average += i;
                Assert.AreEqual(average / (i + 1), stats.Average(), "i:" + i);
                Assert.AreEqual(average / (i + 1), stats.Average(0), "i:" + i);
                Assert.AreEqual(i, stats.Average(1), "i:" + i);
            }
        }

        [Test]
        public void Stats_Percentile_InvalidParameter()
        {
            Stats stats = new Stats(1);
            Assert.Throws<ArgumentException>(() => stats.Percentile(-1));
            Assert.Throws<ArgumentException>(() => stats.Percentile(101));
            Assert.Throws<ArgumentException>(() => stats.Percentile(float.MinValue));
            Assert.Throws<ArgumentException>(() => stats.Percentile(float.MaxValue));
        }

        [Test]
        public void Stats_Percentile_OneElementNotPopulated()
        {
            Stats stats = new Stats(1);
            Assert.Zero(stats.Percentile(0));
            Assert.Zero(stats.Percentile(1));
            Assert.Zero(stats.Percentile(10));
            Assert.Zero(stats.Percentile(100));
        }

        [Test]
        public void Stats_Percentile_OneElementPopulated()
        {
            Stats stats = new Stats(1);
            stats.AddNext(10);

            Assert.AreEqual(10, stats.Percentile(0));
            Assert.AreEqual(10, stats.Percentile(1));
            Assert.AreEqual(10, stats.Percentile(10));
            Assert.AreEqual(10, stats.Percentile(100));

            stats.AddNext(20);
            Assert.AreEqual(20, stats.Percentile(0));
            Assert.AreEqual(20, stats.Percentile(1));
            Assert.AreEqual(20, stats.Percentile(10));
            Assert.AreEqual(20, stats.Percentile(100));
        }

        [Test]
        public void Stats_Percentile_ThousandElementsNotPopulated()
        {
            Stats stats = new Stats(1000);
            for (int i = 1; i <= 100; i++) //Only 100 elements on 1000
                stats.AddNext(i);

            for (int p = 1; p < 100; p++)
                Assert.AreEqual(100 - p + 1, stats.Percentile(p), "p:" + p);            
        }

        [Test]
        public void Stats_Percentile_ThousandElementsPopulated()
        {
            Stats stats = new Stats(1000);
            for (int i = 1; i <= 1000; i++)
                stats.AddNext(i);

            for (int p = 1; p < 1000; p++)
                Assert.AreEqual(1000 - p + 1, stats.Percentile(p / 10f), "p:" + p);            
        }
    }
}
#endif