namespace Termway.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Compute simple statistic.
    /// Use a array and sorted list to guarantee a O(1) percentile computation.
    /// </summary>
    public class Stats
    {
        /// <summary>
        /// Max population.
        /// </summary>
        public uint PopulationSize { get; private set; }

        /// <summary>
        /// Current index used to populate <see cref="timeframes"/>.
        /// </summary>
        public uint CurrentIndex { get; private set; }

        /// <summary>
        /// Number of times that timeframes data have been replaced.
        /// </summary>
        public uint TotalIteration { get; private set; }

        /// <summary>
        /// Store consecutive timeframes by frame count index value in ms.
        /// </summary>
        float[] timeframes;

        /// <summary>
        /// Sorted times values of <see cref="timeframes"/> via insert sort.
        /// Stats are computed from thoses sorted timeframes value.
        /// </summary>
        List<float> sortedTimeframes;

        /// <summary>
        /// Create the stats object with a given population size..
        /// </summary>
        /// <param name="populationSize">Must be > 0.</param>
        public Stats(uint populationSize)
        {
            if (populationSize == 0)
                throw new ArgumentException("Population size must be at least 1.");

            PopulationSize = populationSize;
            timeframes = new float[populationSize];
            sortedTimeframes = new List<float>((int) populationSize);
        }

        /// <summary>
        /// Pass to true when all data are populated in <see cref="timeframes"/>.
        /// </summary>
        public bool IsPopulated { get { return TotalIteration > 0; } }

        /// <summary>
        /// Recover the last added value.
        /// </summary>
        public float LastAdded { get; private set; }

        /// <summary>
        /// Min recorded value.
        /// </summary>
        public float Min { get { return sortedTimeframes.Any() ? sortedTimeframes.First() : 0; } }

        /// <summary>
        /// Max recorded value.
        /// </summary>
        public float Max { get { return sortedTimeframes.Any() ? sortedTimeframes.Last() : 0; } }

        /// <summary>
        /// Add the value.
        /// </summary>
        /// <param name="value"></param>
        public void AddNext(float value)
        {
            if (IsPopulated && sortedTimeframes.Contains(timeframes[CurrentIndex]))
                sortedTimeframes.Remove(timeframes[CurrentIndex]);

            LastAdded = value;
            timeframes[CurrentIndex] = value;

            //Search the index of the new value in the sorted list and insert it at the right place (insert sort).
            int index = sortedTimeframes.BinarySearch(timeframes[CurrentIndex]);
            index = index >= 0 ? index : ~index;
            sortedTimeframes.Insert(index, timeframes[CurrentIndex]);

            CurrentIndex++;
            if (CurrentIndex >= PopulationSize)
            {
                CurrentIndex = CurrentIndex % PopulationSize;
                TotalIteration++;
            }
        }

        public float Last(uint lastNValues = 0)
        {
            if (lastNValues == 0)
                return LastAdded;

            if(lastNValues >= PopulationSize)
                throw new ArgumentException("lastNValues must be between below PopulationSize. " + lastNValues + "<" + PopulationSize);

            int index = (int) (CurrentIndex - lastNValues - 1);
            return timeframes[(index + 2 * PopulationSize) % PopulationSize];          
        }


        /// <summary>
        /// Compute the average of the timeframe. O(n).
        /// </summary>
        /// <returns>Value is clamped between [1, <see cref="PopulationSize"/>]. 0 means average on all values. </returns>
        public float Average(uint ulastNValues = 0)
        {
            int lastNValues = (int) Math.Min(ulastNValues, PopulationSize);
            lastNValues = lastNValues == 0 ? (int) PopulationSize : lastNValues;  //0 is the same as all values. 
                
            int index = (int) CurrentIndex;

            if (PopulationSize == 1)
                return timeframes[0];

            float average = 0;
            //No negative value for not populated timeframe.
            int minIndexLastNValue = IsPopulated ? index - lastNValues : Math.Max(0, index - lastNValues);
            int minIndex = IsPopulated ? (int) PopulationSize : index;
            int startingIndex = lastNValues == 0 ? Math.Max(0, index - minIndex) : minIndexLastNValue;

            int number = 0;
            for (int tf = startingIndex; tf < index; tf++, number++)
                average += timeframes[(tf + PopulationSize) % PopulationSize];
            return average / Math.Max(1, number);
        }

        /// <summary>
        /// Compute percentile value using index value. O(1).
        /// </summary>
        /// <param name="percentile">Percentage must be between 0 and 100. [0, 100].</param>
        /// <returns></returns>
        public float Percentile(float percentile)
        {
            if (percentile < 0 || percentile > 100)
                throw new ArgumentException("Percentage must be between 0 and 100. Was " + percentile);

            percentile = 100 - percentile;
            uint realRange = IsPopulated ? PopulationSize : CurrentIndex;
            //Round to avoid precision truncate for high range value.
            uint index = (uint) Math.Round(realRange * percentile / 100f); 
            if (sortedTimeframes.Count > index)
                return sortedTimeframes.ElementAt((int)index);
            return timeframes[CurrentIndex];
        }
    }
}
